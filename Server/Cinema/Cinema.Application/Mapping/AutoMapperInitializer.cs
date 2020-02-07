using AutoMapper;

namespace Cinema.Application.Mapping
{
    /// <summary>
    /// Registra todos os mapas configurados na aplicação.
    /// </summary>
    public class AutoMapperInitializer
    {
        private Mapper _mapper;

        public AutoMapperInitializer()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(System.Reflection.Assembly.GetExecutingAssembly())));
        }
        public Mapper GetMapper()
        {
            return _mapper;
        }
    }
}
