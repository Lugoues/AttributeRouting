using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Routing;
using AttributeRouting.Framework;
using AttributeRouting.Web.Http.Framework;

namespace AttributeRouting.Web.Http.WebHost
{
    /// <summary>
    /// Extensions to the System.Web.Routing.RouteCollection
    /// </summary>
    public static class RouteCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for all routes defined with AttributeRouting attributes,
        /// using the default conventions.
        /// </summary>
        public static void MapHttpAttributeRoutes(this RouteCollection routes)
        {
            var configuration = new HttpWebAttributeRoutingConfiguration();
            configuration.AddRoutesFromAssembly(Assembly.GetCallingAssembly());

            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="configurationAction">The initialization action that builds the configuration object</param>
        public static void MapHttpAttributeRoutes(this RouteCollection routes, Action<HttpWebAttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new HttpWebAttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);
            
            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configuration">The configuration object</param>
        public static void MapHttpAttributeRoutes(this RouteCollection routes, HttpWebAttributeRoutingConfiguration configuration)
        {
            routes.MapHttpAttributeRoutesInternal(configuration);
        }

        private static void MapHttpAttributeRoutesInternal(this RouteCollection routes, HttpWebAttributeRoutingConfiguration configuration)
        {
            var generatedRoutes = new RouteBuilder(configuration)
                .BuildAllRoutes()
                .Cast<HttpAttributeRoute>()
                .ToList();

            var mvcRoutes = RouteTable.Routes;
            generatedRoutes.ForEach(r =>
            {
                var mvcRoute = mvcRoutes.MapHttpRoute(r.RouteName, r.Url, r.Defaults, r.Constraints, r.Handler);
                mvcRoute.DataTokens = new RouteValueDictionary(r.DataTokens);
                mvcRoute.RouteHandler = routeHandler;
            });
        }
    }
}