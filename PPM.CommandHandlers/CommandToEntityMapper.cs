using AutoMapper;

namespace PPM.CommandHandlers
{
    public static class CommandToEntityMapper
    {
        private static readonly IMapper mapper;

        static CommandToEntityMapper()
        {
            var cfg = new MapperConfiguration(c => c.CreateMissingTypeMaps = true);
            mapper = cfg.CreateMapper();
        }

        public static TDest MapToEntity<TDest>(this object source)
        {
            return mapper.Map<TDest>(source);
        }

        public static void PopulateEntity<TSource, TDest>(this TSource source, TDest dest) where TSource : class
        {
            mapper.Map(source, dest);
        }
    }
}