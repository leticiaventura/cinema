using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Web.Http;
using Cinema.WebAPI.App_Start;
using Cinema.WebAPI.IoC;

[assembly: OwinStartup(typeof(Cinema.WebAPI.Startup))]
namespace Cinema.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SimpleInjectorContainer.Initialize();
            HttpConfiguration config = new HttpConfiguration();
            app.UseCors(CorsOptions.AllowAll);
            OAuthConfig.ConfigureOAuth(app);
            app.UseWebApi(config);
        }
    }
}