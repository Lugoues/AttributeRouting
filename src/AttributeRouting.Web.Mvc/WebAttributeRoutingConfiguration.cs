using System;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc.Framework.Factories;

namespace AttributeRouting.Web.Mvc
{
    public class WebAttributeRoutingConfiguration : WebAttributeRoutingConfigurationBase
    {
        public WebAttributeRoutingConfiguration()
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            RouteConstraintFactory = new RouteConstraintFactory(this);
            ParameterFactory = new RouteParameterFactory();

            RouteHandlerFactory = () => new MvcRouteHandler();
        }

        /// <summary>
        /// The controller type applicable to this context.
        /// </summary>
        public override Type FrameworkControllerType
        {
            get { return typeof(IController); }
        }

        /// <summary>
        /// Appends the routes from the specified controller type to the end of route collection.
        /// </summary>
        /// <typeparam name="T">The controller type.</typeparam>
        public void AddRoutesFromController<T>() where T : IController
        {
            AddRoutesFromController(typeof(T));
        }

        /// <summary>
        /// Appends the routes from all controllers that derive from the specified controller type to the route collection.
        /// </summary>
        /// <typeparam name="T">The base controller type.</typeparam>
        public void AddRoutesFromControllersOfType<T>() where T : IController
        {
            AddRoutesFromControllersOfType(typeof(T));
        }
    }
}
