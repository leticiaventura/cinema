using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.UriParser;
using Microsoft.OData;
using System.Web.Http.Cors;
using System.Diagnostics.CodeAnalysis;

namespace Cinema.WebAPI
{
    [ExcludeFromCodeCoverage]
    public static class WebApiConfig
    {
        /// <summary>
        /// Registra configurações da API
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.MapApiRoutes();
            config.EnableOdata();
            config.ConfigureJsonSerialization();
            config.ConfigureXMLSerialization();
        }

        /// <summary>
        /// Habilita utilização de OData nos controllers
        /// </summary>
        /// <param name="config"></param>
        private static void EnableOdata(this HttpConfiguration config)
        {
            config.Count().Select().Filter().OrderBy().MaxTop(null);
            config.AddODataQueryFilter();
            config.EnableDependencyInjection(builder =>
            {
                builder.AddService<ODataUriResolver>(ServiceLifetime.Singleton, sp => new StringAsEnumResolver() { EnableCaseInsensitive = true });
            });
        }

        /// <summary>
        /// Configura rotas da API
        /// </summary>
        /// <param name="config"></param>
        private static void MapApiRoutes(this HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "Cinema.API",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    action = RouteParameter.Optional
                }
            );
        }

        /// <summary>
        /// Configuração para retornos JSON da API.
        /// </summary>
        /// <param name="config"></param>
        private static void ConfigureJsonSerialization(this HttpConfiguration config)
        {
            var jsonSerializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSerializerSettings.Formatting = Formatting.None;
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        /// <summary>
        /// Configuração para retornos XML da API.
        /// </summary>
        /// <param name="config"></param>
        private static void ConfigureXMLSerialization(this HttpConfiguration config)
        {
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
        }
    }
}
