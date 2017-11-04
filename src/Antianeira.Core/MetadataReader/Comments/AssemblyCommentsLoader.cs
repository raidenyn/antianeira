using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader.Comments
{
    internal class AssemblyCommentsLoader
    {
        public ICommentsProvider Load(Assembly assembly)
        {
            var result = new CommentsStore();
            XDocument xml = LoadComments(assembly);
            if (xml == null)
            {
                return result;
            }

            var members = (from element in xml.Descendants("member")
                           let name = element.Attribute("name")
                           where !String.IsNullOrWhiteSpace(name?.Value)
                           let parts = name.Value.Split(':')
                           where parts.Length == 2
                           select new
                           {
                               Kind = parts[0],
                               Type = parts[1],
                               Comments = element
                           }).ToArray();

            result.Types = (from member in members
                            where member.Kind == "T"
                            select member).ToDictionary(member => member.Type, member => Create(member.Comments));

            result.Properties = (from member in members
                                 where member.Kind == "P"
                                 select member).ToDictionary(member => member.Type, member => Create(member.Comments));

            result.Fields = (from member in members
                             where member.Kind == "F"
                             select member).ToDictionary(member => member.Type, member => Create(member.Comments));

            return result;
        }

        public Comment Create(XElement element)
        {
            return new Comment
            {
                Summary = element.Descendants("summary").FirstOrDefault()?.Value.Trim().Replace("\n", "")
            };
        }

        [CanBeNull]
        private XDocument LoadComments(Assembly assembly)
        {
            var xmlpath = Path.ChangeExtension(assembly.Location, "xml");
            if (!File.Exists(xmlpath))
            {
                return null;
            }
            using (var file = File.OpenRead(xmlpath))
            {
                return XDocument.Load(file);
            }
        }
    }
}
