using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public class MappingSettings
    {
        public MappingSettings() {
            DefinitionsMapper = new DefinitionsMapper(this);
            PropertyTypeMapper = new PropertyTypeMapper(this);
        }

        [NotNull]
        public IDefinitionsMapper DefinitionsMapper { get; set; }

        [NotNull]
        public IPropertyTypeMapper PropertyTypeMapper { get; set; }

        [NotNull]
        public IPropertyNameMapper PropertyNameMapper { get; set; } = new CamelCasePropertyTypeName();

        [NotNull]
        public IEnumFieldMapper EnumFieldMapper { get; set; } = new CamelCaseEnumFieldMapper();

        [NotNull]
        public ICommentsProvider CommentsProvider { get; set; } = new CommentsProvider();

        [NotNull]
        public IMethodNameMapper MethodNameMapper { get; set; } = new CamelCaseMethodName();
    }
}
