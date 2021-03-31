using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Encrypt_Decrypt
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
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Decrypt",
                routeTemplate: "decrypt/{controller}/{cipherText}",
                defaults: new { cipherText = RouteParameter.Optional }
            );

           config.Routes.MapHttpRoute(
             name: "Encrypt",
               routeTemplate: "encrypt/{controller}/{str}",
               defaults: new { str = RouteParameter.Optional }
           );
        }
    }
}
