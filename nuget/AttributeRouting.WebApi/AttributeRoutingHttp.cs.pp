using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.AttributeRoutingHttp), "Start")]

namespace $rootnamespace$.App_Start {
    public static class AttributeRoutingHttp {
		public static void RegisterRoutes(HttpRouteCollection routes) {
            
			// See http://attributerouting.net for full documentation.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            routes.MapHttpAttributeRoutes();
		}

        public static void Start() {
		
            GlobalConfiguration.Configuration.MessageHandlers.Add(new BypassHttpRoutingDispatcherHandler());
        
			RegisterRoutes(GlobalConfiguration.Configuration.Routes);
        }
    }
}
