using System;

namespace Antianeira.MetadataReader
{
    public interface IInterfaceNameMapper
    {
        string GetInterfaceName(Type type);
    }

    public class DefaultInterfaceNameMapper : IInterfaceNameMapper
    {
        public string GetInterfaceName(Type type)
        {
            var name = type.Name;

            if (type.IsGenericType)
            {
                name = name.Substring(0, name.IndexOf('`'));
            }

            if (name[0] != 'I')
            {
                return "I" + name;
            }

            if (name.Length > 1)
            {
                var secondVar = name[1].ToString();
                if (secondVar != secondVar.ToUpper())
                {
                    return "I" + name;
                }
            }

            return name;
        }
    }
}