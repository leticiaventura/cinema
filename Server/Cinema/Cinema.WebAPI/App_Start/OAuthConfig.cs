using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using Cinema.WebAPI.AuthProvider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Cinema.WebAPI.App_Start
{
    [ExcludeFromCodeCoverage]
    public class OAuthConfig
    {
        /// <summary>
        /// Configurações de geração e validação do token para autenticação dos usuários.
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureOAuth(IAppBuilder app)
        {
            ConfigureOAuthTokenGeneration(app);
        }

        private static void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),
                Provider = new OAuthProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}