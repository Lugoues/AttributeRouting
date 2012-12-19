using System.Web.Routing;
using AttributeRouting.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.AttributeRouting), "Start")]

namespace $rootnamespace$.App_Start {
    public static class AttributeRouting {
		public static void RegisterRoutes(RouteCollection routes) {
            
			// See http://attributerouting.net for full documentation.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd
            
			routes.MapAttributeRoutes();
		}

        public static void Start() {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
