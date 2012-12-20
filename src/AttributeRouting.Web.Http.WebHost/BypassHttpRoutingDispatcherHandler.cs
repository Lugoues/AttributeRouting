using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System.Web.Routing;
using AttributeRouting.Helpers;

namespace AttributeRouting.Web.Http.WebHost
{
    /// <summary>
    /// The underlying HttpRoutingDispatcher expects a route of type HttpWebRoute,
    /// which is internal to the System.Web.Http.WebHost assembly. AttributeRouting
    /// drops an HttpWebAttributeRoute into the route table, but this route cannot be
    /// cast to the internal HttpWebRoute. So when the dispatcher runs, it throws a null reference exception.
    /// This handler simply bypasses the route dispatcher and invokes the controller dispatcher directly.
    /// </summary>
    public class BypassHttpRoutingDispatcherHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            // Lookup route data, or if not found as a request property then we look it up in the route table
            IHttpRouteData routeData;
            if (!request.Properties.TryGetValue(HttpPropertyKeys.HttpRouteDataKey, out routeData))
            {
                // TODO: Replace this with our own, that honors HttpWebAttributeRoute
                routeData = request.GetConfiguration().Routes.GetRouteData(request);
                if (routeData != null)
                {
                    request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);
                }
                else
                {
                    var response = request.CreateErrorResponse(
                        HttpStatusCode.NotFound,
                        "No HTTP resource was found that matches the request URI '{0}'.".FormatWith(request.RequestUri));

                    var tcs = new TaskCompletionSource<HttpResponseMessage>();
                    tcs.SetResult(response);
                    return tcs.Task;
                }
            }

            RemoveOptionalRoutingParameters(routeData.Values);

            // This guards against the exception thrown when web-host web api finds a matching route that is not an HttpWebRoute.
            if (routeData.Route != null)
            {
                var invoker = routeData.Route.Handler == null
                                  ? new HttpMessageInvoker(GlobalConfiguration.DefaultHandler)
                                  : new HttpMessageInvoker(routeData.Route.Handler, disposeHandler: false);

                return invoker.SendAsync(request, cancellationToken);
            }
            else
            {
                // Do our workaround
                var dispatcher = new HttpControllerDispatcher(request.GetConfiguration());
                var invoker = new HttpMessageInvoker(dispatcher);

                return invoker.SendAsync(request, cancellationToken);
            }
        }

        private static void RemoveOptionalRoutingParameters(IDictionary<string, object> routeValueDictionary)
        {
            if (routeValueDictionary == null)
            {
                throw new ArgumentNullException("routeValueDictionary");
            }

            // Get all keys for which the corresponding value is 'Optional'.
            // Having a separate array is necessary so that we don't manipulate the dictionary while enumerating.
            // This is on a hot-path and linq expressions are showing up on the profile, so do array manipulation.
            int max = routeValueDictionary.Count;
            int i = 0;
            string[] matching = new string[max];
            foreach (KeyValuePair<string, object> kv in routeValueDictionary)
            {
                if (kv.Value == RouteParameter.Optional)
                {
                    matching[i] = kv.Key;
                    i++;
                }
            }
            for (int j = 0; j < i; j++)
            {
                string key = matching[j];
                routeValueDictionary.Remove(key);
            }
        }
    }
}