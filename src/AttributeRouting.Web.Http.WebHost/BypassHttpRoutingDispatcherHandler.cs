using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Dispatcher;

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
            var dispatcher = new HttpControllerDispatcher(request.GetConfiguration());
            var invoker = new HttpMessageInvoker(dispatcher);

            return invoker.SendAsync(request, cancellationToken);
        }
    }
}