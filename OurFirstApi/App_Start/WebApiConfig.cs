using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OurFirstApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes (my defined)
            config.MapHttpAttributeRoutes();

            // to recognize the default routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                          // GET api/values      /5
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
