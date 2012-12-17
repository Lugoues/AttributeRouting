using System.Collections.Generic;
using System.Web.Routing;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Framework;

namespace AttributeRouting.Web.Http.WebHost.Framework
{
    /// <summary>
    /// Mimics the AttributeRouting.Web.Framework.WebAttributeRoute class to work better for Web API scenarios. 
    /// The only difference between the base class and this class is that this one will match only when
    /// a special "httproute" key is specified when generating URLs. There is no special behavior
    /// for incoming URLs.
    /// </summary>    
    public class HttpWebAttributeRoute : WebAttributeRoute
    {
        private const string HttpRouteKey = "httproute";

        /// <summary>
        /// Route used by the AttributeRouting framework in web projects.
        /// </summary>
        public HttpWebAttributeRoute(string url,
                                     RouteValueDictionary defaults,
                                     RouteValueDictionary constraints,
                                     RouteValueDictionary dataTokens,
                                     WebAttributeRoutingConfigurationBase configuration)
            : base(url, defaults, constraints, dataTokens, configuration) { }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // Only perform URL generation if the "httproute" key was specified. This allows these
            // routes to be ignored when a regular MVC app tries to generate URLs. Without this special
            // key an HTTP route used for Web API would normally take over almost all the routes in a
            // typical app.
            if (!values.ContainsKey("httproute"))
            {
                return null;
            }
            // Remove the value from the collection so that it doesn't affect the generated URL
            RouteValueDictionary newValues = GetRouteDictionaryWithoutHttpRouteKey(values);

            return base.GetVirtualPath(requestContext, newValues);
        }

        private static RouteValueDictionary GetRouteDictionaryWithoutHttpRouteKey(IDictionary<string, object> routeValues)
        {
            var newRouteValues = new RouteValueDictionary();
            foreach (var routeValue in routeValues)
            {
                if (!routeValue.Key.ValueEquals(HttpRouteKey))
                {
                    newRouteValues.Add(routeValue.Key, routeValue.Value);
                }
            }
            return newRouteValues;
        }
    }
}
