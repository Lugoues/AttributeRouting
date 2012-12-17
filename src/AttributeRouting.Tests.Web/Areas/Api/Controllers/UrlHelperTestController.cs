using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http;

namespace AttributeRouting.Tests.Web.Areas.Api.Controllers
{
    [RoutePrefix("api/v1/users")]
    public class UrlHelperTestController : ApiController
    {
        [GET("?{urlId}", RouteName = "GetByQueryString")]
        public string GetByQueryString(string urlId)
        {
            return "You called GetByQueryString";

        }

        [GET("links")]
        public List<Link> GetLinks()
        {
            var urlHelper = Request.GetUrlHelper();
            var links = new List<Link>
            {
                new Link { Name = "GetById", Url = urlHelper.Link("GetById", new { userId = "123" }) },
                new Link { Name = "GetByQueryString", Url = urlHelper.Link("GetByQueryString", new { urlId = "123" }) }
            };

            return links;
        }

        [GET("{userId}", RouteName = "GetById")]
        public string GetById(string userId)
        {
            return "You called GetById";
        }

        public class Link
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
    }
}
