using AttributeRouting.Framework;
using AttributeRouting.Web.Http.SelfHost.Framework.Factories;

namespace AttributeRouting.Web.Http.SelfHost
{
    public class HttpAttributeRoutingConfiguration : HttpAttributeRoutingConfigurationBase
    {
        public HttpAttributeRoutingConfiguration()
        {
            AttributeRouteFactory = new AttributeRouteFactory(this);
            RouteConstraintFactory = new RouteConstraintFactory(this);
            ParameterFactory = new RouteParameterFactory();

            // Must turn on AutoGenerateRouteNames and use the Unique RouteNameBuilder for this to work out-of-the-box.
            AutoGenerateRouteNames = true;
            RouteNameBuilder = RouteNameBuilders.Unique;
        }
    }
}
