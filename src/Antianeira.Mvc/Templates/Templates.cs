using System;
using System.IO;
using System.Reflection;

namespace Antianeira.Templates
{
    public static class Templates
    {
        private static readonly Assembly Assembly = typeof(Templates).GetTypeInfo().Assembly;

        private static readonly string Namespace = typeof(Templates).Namespace;

        public static string Load(string name)
        {
            var resourceName = $"{Namespace}.{name}.liquid";
            using (var template = Assembly.GetManifestResourceStream(resourceName))
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
