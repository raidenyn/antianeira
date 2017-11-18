using JetBrains.Annotations;
using System;

namespace Antianeira.MetadataReader
{
    public interface IExportStrategy
    {
        bool IsExported([NotNull] Type type);
    }

    public class AlwaysExportStrategy: IExportStrategy
    {
        public bool IsExported(Type type)
        {
            return true;
        }
    }
}
