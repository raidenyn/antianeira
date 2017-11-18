using Antianeira.Schema;
using JetBrains.Annotations;
using System.Reflection;
using Antianeira.MetadataReader.Comments;

namespace Antianeira.MetadataReader
{
    public interface IInterfacePropertyMapper
    {
        [CanBeNull]
        InterfaceProperty MapProperty([NotNull] PropertyInfo property, [NotNull] InterfacePropertyMappingContext context);
    }

    public class DefaultInterfacePropertyMapper: IInterfacePropertyMapper
    {
        private readonly IPropertyNameMapper _propertyNameMapper;
        private readonly IPropertyTypeMapper _propertyTypeMapper;
        private readonly ICommentsProvider _commentsProvider;

        public DefaultInterfacePropertyMapper(
            IPropertyNameMapper propertyNameMapper,
            IPropertyTypeMapper propertyTypeMapper,
            ICommentsProvider commentsProvider
        ) {
            _propertyNameMapper = propertyNameMapper;
            _propertyTypeMapper = propertyTypeMapper;
            _commentsProvider = commentsProvider;
        }

        public InterfaceProperty MapProperty(PropertyInfo property, InterfacePropertyMappingContext context) {
            var name = _propertyNameMapper.GetPropertyName(property);

            return new InterfaceProperty(name)
            {
                Source = property,
                Type = _propertyTypeMapper.GetPropertyType(property, context.GetTypeReferenceContext()),
                Comment = _commentsProvider.GetComment(property)
            };
        }
    }
}
