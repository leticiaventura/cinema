using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Cinema.WebAPI.IoC;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using Microsoft.Owin.Security.OAuth;
using SimpleInjector.Lifestyles;

namespace Cinema.WebAPI.AuthProvider
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        public OAuthProvider() : base()
        {

        }

        /// <summary>
        /// Método que deve ser configurado para validar se a aplicação está registrada para utilizar a API de autenticação com base no valor do ClientId.
        /// Neste fluxo p request será sempre validado, dispensando neste momento a configuração de um ClientId.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Método responsável por validar as credenciais passadas pelo usuário no request (username e password).
        /// Quando válidas a API retorna um Bearer Token para ser utilizados nas requisições dos controllers. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Acess-Control-Allow-Origin", new[] { "*" });
            var user = default(User);
            try
            {
                using (AsyncScopedLifestyle.BeginScope(SimpleInjectorContainer.ContainerInstance))
                {
                    var authService = SimpleInjectorContainer.ContainerInstance.GetInstance<IUserService>();
                    user = authService.Login(context.UserName, context.Password);
                }
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", ex.Message);
                return Task.CompletedTask;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            SetRole(user, identity);

            context.Validated(identity);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Identifica e adiciona no token o nível de permissão do usuário autenticado. Necessário para autorizar o acesso a controllers específicos.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="identity"></param>
        private static void SetRole(User user, ClaimsIdentity identity)
        {
            switch (user.Permission)
            {
                case EnumPermissionLevel.Admin:
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                    break;
                case EnumPermissionLevel.Employee:
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Employee"));
                    break;
                case EnumPermissionLevel.Customer:
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Customer"));
                    break;
                default:
                    break;
            }
        }
    }
}