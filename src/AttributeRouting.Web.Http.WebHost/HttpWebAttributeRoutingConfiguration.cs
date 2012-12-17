using System;
using System.Web.Http.Controllers;
using System.Web.Http.WebHost;
using AttributeRouting.Web.Http.WebHost.Framework.Factories;

namespace AttributeRouting.Web.Http.WebHost
{
    public class HttpWebAttributeRoutingConfiguration : WebAttributeRoutingConfigurationBase
    {
        public HttpWebAttributeRoutingConfiguration()
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            RouteConstraintFactory = new RouteConstraintFactory(this);
            ParameterFactory = new RouteParameterFactory();

            RouteHandlerFactory = () => HttpControllerRouteHandler.Instance;
        }

        /// <summary>
        /// The controller type applicable to this context.
        /// </summary>
        public override Type FrameworkControllerType
        {
            get { return typeof(IHttpController); }
        }

        /// <summary>
        /// Appends the routes from the specified controller type to the end of route collection.
        /// </summary>
        /// <typeparam name="T">The controller type.</typeparam>
        public void AddRoutesFromController<T>() where T : IHttpController
        {
            AddRoutesFromController(typeof(T));
        }

        /// <summary>
        /// Appends the routes from all controllers that derive from the specified controller type to the route collection.
        /// </summary>
        /// <typeparam name="T">The base controller type.</typeparam>
        public void AddRoutesFromControllersOfType<T>() where T : IHttpController
        {
            AddRoutesFromControllersOfType(typeof(T));
        }
    }
}
