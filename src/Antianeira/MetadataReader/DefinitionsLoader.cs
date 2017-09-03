using Antianeira.Schema;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public class DefinitionsLoader
    {
        private readonly MappingSettings _settings;

        public DefinitionsLoader(MappingSettings settings = null) {
            _settings = settings ?? new MappingSettings();
        }

        public void Add(TypeInfo type, Definitions definitions) {
            _settings.DefinitionsMapper.ConvertType(type, definitions);
        }
    }
}
