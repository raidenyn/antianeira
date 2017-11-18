using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface IInterfacePropertiesProvider
    {
        [NotNull, ItemNotNull]
        IEnumerable<PropertyInfo> GetProperties([NotNull] Type type);
    }

    public class OnlyOwnInterfacePropertiesProvider : IInterfacePropertiesProvider
    {
        IEnumerable<PropertyInfo> IInterfacePropertiesProvider.GetProperties(Type type)
        {
            var nestedPropertyNames = type.GetInterfaces().SelectMany(i => i.GetProperties()).Select(p=>p.Name).ToHashSet();

            return type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                       .Where(property => !nestedPropertyNames.Contains(property.Name));
        }
    }
}
