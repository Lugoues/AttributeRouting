using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http.Hosting;

namespace System.Web.Http.WebHost
{
    internal static class HttpControllerHandlerHelper
    {
        public const string HttpContextBaseKey = "MS_HttpContext";

        private static readonly Lazy<IHostBufferPolicySelector> _bufferPolicySelector =
            new Lazy<IHostBufferPolicySelector>(() => GlobalConfiguration.Configuration.Services.GetHostBufferPolicySelector());

        private static readonly Func<HttpRequestMessage, X509Certificate2> _retrieveClientCertificate = 
            new Func<HttpRequestMessage, X509Certificate2>(RetrieveClientCertificate);

        private static void AddHeaderToHttpRequestMessage(HttpRequestMessage httpRequestMessage, string headerName, string[] headerValues)
        {
            if (httpRequestMessage == null)
            {
                throw new ArgumentNullException("httpRequestMessage");
            }
            if (headerName == null)
            {
                throw new ArgumentNullException("headerName");
            }
            if (headerValues == null)
            {
                throw new ArgumentNullException("headerValues");
            }

            if (!httpRequestMessage.Headers.TryAddWithoutValidation(headerName, headerValues))
            {
                httpRequestMessage.Content.Headers.TryAddWithoutValidation(headerName, headerValues);
            }
        }

        internal static HttpRequestMessage ConvertRequest(HttpContextBase httpContextBase)
        {
            if (httpContextBase == null)
            {
                throw new ArgumentNullException("httpContextBase");
            }

            HttpRequestBase requestBase = httpContextBase.Request;
            HttpMethod method = HttpMethodHelper.GetHttpMethod(requestBase.HttpMethod);
            Uri uri = requestBase.Url;
            HttpRequestMessage request = new HttpRequestMessage(method, uri);

            // Choose a buffered or bufferless input stream based on user's policy
            IHostBufferPolicySelector policySelector = _bufferPolicySelector.Value;
            bool isInputBuffered = policySelector == null ? true : policySelector.UseBufferedInputStream(httpContextBase);
            Stream inputStream = isInputBuffered
                                    ? requestBase.InputStream
                                    : httpContextBase.ApplicationInstance.Request.GetBufferlessInputStream();

            request.Content = new StreamContent(inputStream);
            foreach (string headerName in requestBase.Headers)
            {
                string[] values = requestBase.Headers.GetValues(headerName);
                AddHeaderToHttpRequestMessage(request, headerName, values);
            }

            // Add context to enable route lookup later on
            request.Properties.Add(HttpContextBaseKey, httpContextBase);

            // Add the retrieve client certificate delegate to the property bag to enable lookup later on
            request.Properties.Add(HttpPropertyKeys.RetrieveClientCertificateDelegateKey, _retrieveClientCertificate);

            // Add information about whether the request is local or not
            request.Properties.Add(HttpPropertyKeys.IsLocalKey, new Lazy<bool>(() => requestBase.IsLocal));

            // Add information about whether custom errors are enabled for this request or not
            request.Properties.Add(HttpPropertyKeys.IncludeErrorDetailKey, new Lazy<bool>(() => !httpContextBase.IsCustomErrorEnabled));

            return request;
        }

        private static X509Certificate2 RetrieveClientCertificate(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            X509Certificate2 result = null;

            HttpContextBase httpContextBase;
            if (request.Properties.TryGetValue(HttpContextBaseKey, out httpContextBase))
            {
                if (httpContextBase.Request.ClientCertificate.Certificate != null && httpContextBase.Request.ClientCertificate.Certificate.Length > 0)
                {
                    result = new X509Certificate2(httpContextBase.Request.ClientCertificate.Certificate);
                }
            }

            return result;
        }

    }
}
