using System.Web.Http;

namespace ChessAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{move}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    move = RouteParameter.Optional
                }
            );
        }
    }
}
