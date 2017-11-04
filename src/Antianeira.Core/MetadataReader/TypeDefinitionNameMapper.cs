using System;

namespace Antianeira.MetadataReader
{
    public interface ITypeDefinitionNameMapper
    {
        string GetTypeName(Type type);
    }

    public class TypeDefinitionNameMapper : ITypeDefinitionNameMapper
    {
        public string GetTypeName(Type type)
        {
            return type.Name;
        }
    }
}