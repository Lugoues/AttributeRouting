using System.Web.Http.SelfHost;
using AttributeRouting.Web.Http.SelfHost;

namespace $rootnamespace$ {
    public static class AttributeRouting {

		// Call this static method from a start up class in your applicaton (e.g.Program.cs)
		// Pass in the configuration you're using for your self-hosted Web API
		public static void RegisterRoutes(HttpSelfHostConfiguration config) {
            
			// See http://attributerouting.net for full documentation.
			// To debug routes locally, you can log to Console.Out (or any other TextWriter) like so:
			//     config.Routes.Cast<HttpRoute>().LogTo(Console.Out);

			config.MessageHandlers.Add(new BypassHttpRoutingDispatcherHandler());
            config.Routes.MapHttpAttributeRoutes();
		}
    }
}