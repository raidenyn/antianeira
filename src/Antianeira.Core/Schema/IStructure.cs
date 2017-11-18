using System.Collections.Generic;

namespace Antianeira.Schema
{
    public interface IStructure: ITsType
    {
        IEnumerable<Property> Properties { get; }
    }
}
