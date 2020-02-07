using System.Web.Http;
using AutoMapper;
using Cinema.Application.Features.Lounges;
using Cinema.Application.Features.Movies;
using Cinema.Application.Features.Sessions;
using Cinema.Application.Features.Snacks;
using Cinema.Application.Features.Users;
using Cinema.Application.Mapping;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.Domain.Features.Sessions.Interfaces;
using Cinema.Domain.Features.Snacks.Interfaces;
using Cinema.Domain.Features.Users.Interfaces;
using Cinema.Infra.ORM.Base;
using Cinema.Infra.ORM.Features.Lounges;
using Cinema.Infra.ORM.Features.Movies;
using Cinema.Infra.ORM.Features.Sessions;
using Cinema.Infra.ORM.Features.Snacks;
using Cinema.Infra.ORM.Features.Users;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace Cinema.WebAPI.IoC
{
    /// <summary>
    /// Container para injeção de dependêcias nos construtores.
    /// </summary>
    public class SimpleInjectorContainer
    {
        public static Container ContainerInstance { get; private set; }

        public static void Initialize()
        {

            ContainerInstance = new Container();

            ContainerInstance.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            RegisterServices(ContainerInstance);

            ContainerInstance.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            ContainerInstance.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(ContainerInstance);
        }


        /// <summary>
        /// Registra todos os serviços que podem ser injetados
        /// </summary>
        /// <param name="container"></param>
        private static void RegisterServices(Container container)
        {
            Mapper mapped = new AutoMapperInitializer().GetMapper();

            container.Register<IUserService, UserService>();
            container.Register<IUserRepository, UserRepository>(); 
            
            container.Register<ILoungeService, LoungeService>();
            container.Register<ILoungeRepository, LoungeRepository>();
            
            container.Register<IMovieService, MovieService>();
            container.Register<IMovieRepository, MovieRepository>();
            
            container.Register<ISnackService, SnackService>();
            container.Register<ISnackRepository, SnackRepository>();
            
            container.Register<ISessionService, SessionService>();
            container.Register<ISessionRepository, SessionRepository>();

            container.Register<Mapper>(() => mapped, Lifestyle.Singleton);
            container.Register<BaseContext>(() => new BaseContext(), Lifestyle.Scoped);
        }
    }
}