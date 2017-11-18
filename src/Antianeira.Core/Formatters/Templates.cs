using System;
using System.IO;
using System.Reflection;

namespace Antianeira.Formatters
{
    public static class Templates
    {
        public static string Load(Assembly assembly, string name)
        {
            var resourceName = $"Antianeira.Templates.{name}.liquid";
            using (var template = assembly.GetManifestResourceStream(resourceName))
            {
                if (template == null)
                {
                    throw new Exception($"Template '{resourceName}' is not found in the assembly.");
                }

                using (var reader = new StreamReader(template))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
