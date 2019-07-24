using AutoMapper;
using Qowaiv.Genealogy.Commands;
using Qowaiv.Genealogy.Events;

namespace Qowaiv.Genealogy.Mapping
{
    public static class Mapper
    {
        public static TDestination Map<TDestination>(this object source) => Get().Map<TDestination>(source);

        private static IMapper Get()
        {
            if (_mapper is null)
            {
                _mapper = _config.CreateMapper();
            }
            return _mapper;
        }


        private static readonly MapperConfiguration _config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreatePerson, PersonUpdated>();
            cfg.CreateMap<UpdatePerson, PersonUpdated>();
        });
        private static IMapper _mapper;
    }
}
