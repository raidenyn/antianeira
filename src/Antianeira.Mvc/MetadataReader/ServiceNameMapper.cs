using System;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IServiceNameMapper
    {
        [NotNull]
        string MapServiceName([NotNull] Type type);
    }

    public class DefaultServiceNameMapper: IServiceNameMapper
    {
        public string MapServiceName(Type type)
        {
            return type.Name.Replace("Controller", "");
        }
    }
}
