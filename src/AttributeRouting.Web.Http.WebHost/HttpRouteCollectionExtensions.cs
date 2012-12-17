using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Routing;

namespace AttributeRouting.Web.Http.WebHost
{
    /// <summary>
    /// Extensions to the System.Web.Http.HttpRouteCollection
    /// </summary>
    public static class HttpRouteCollectionExtensions
    {
        /// <summary>
        /// Scans the calling assembly for all routes defined with AttributeRouting attributes,
        /// using the default conventions.
        /// </summary>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes)
        {
            var configuration = new HttpWebAttributeRoutingConfiguration();
            configuration.AddRoutesFromAssembly(Assembly.GetCallingAssembly());

            MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="configurationAction">The initialization action that builds the configuration object</param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, Action<HttpWebAttributeRoutingConfiguration> configurationAction)
        {
            var configuration = new HttpWebAttributeRoutingConfiguration();
            configurationAction.Invoke(configuration);
            
            MapHttpAttributeRoutesInternal(configuration);
        }

        /// <summary>
        /// Scans the specified assemblies for all routes defined with AttributeRouting attributes,
        /// and applies configuration options against the routes found.
        /// </summary>
        /// <param name="routes"> </param>
        /// <param name="configuration">The configuration object</param>
        public static void MapHttpAttributeRoutes(this HttpRouteCollection routes, HttpWebAttributeRoutingConfiguration configuration)
        {
            MapHttpAttributeRoutesInternal(configuration);
        }

        private static void MapHttpAttributeRoutesInternal(HttpWebAttributeRoutingConfiguration configuration)
        {
            // For web-hosted routes, the global config returns a thin wrapper over the route table.
            // For our purposes, just create these routes directly.
            RouteTable.Routes.MapHttpAttributeRoutes(configuration); 
        }
    }
}