using Antianeira.Formatters.Filters;
using Antianeira.Schema;
using Antianeira.Schema.Api;
using System;
using System.Collections.Generic;
using System.IO;

namespace Antianeira.Formatters
{
    public interface IFormatterTemplates
    {
        void Render<TObject>(TextWriter writer, TObject variable);
    }

    public class FormatterTemplates : IFormatterTemplates
    {
        private static readonly IDictionary<Type, FormatterTemplate> _templates = new Dictionary<Type, FormatterTemplate>
        {
            [typeof(Class)] =
              new FormatterTemplate("class", new[] { typeof(CommentFilters), typeof(ClassFilters) }),
            [typeof(ClassProperty)] =
              new FormatterTemplate("property", new[] { typeof(CommentFilters), typeof(ClassPropertyFilters), typeof(TypeFilters) }),
            [typeof(Interface)] =
              new FormatterTemplate("interface", new[] { typeof(InterfaceFilters), typeof(CommentFilters), typeof(InterfacePropertyFilters) }),
            [typeof(InterfaceProperty)] =
              new FormatterTemplate("property", new[] { typeof(CommentFilters), typeof(InterfacePropertyFilters), typeof(TypeFilters) }),
            [typeof(InterfaceMethod)] =
              new FormatterTemplate("interface_method", new[] { typeof(InterfaceMethodFilters), typeof(CommentFilters), typeof(TypeFilters) }),
            [typeof(InterfaceMethodParameter)] =
              new FormatterTemplate("interface_method_parameter", new[] { typeof(CommentFilters), typeof(TypeFilters) }),
            [typeof(Comment)] =
              new FormatterTemplate("comment", new[] { typeof(CommentFilters) }),
            [typeof(TypeDefinition)] =
              new FormatterTemplate("type", new[] { typeof(TypeDefinitionFilters), typeof(CommentFilters) }),
            [typeof(Schema.Enum)] =
              new FormatterTemplate("enum", new[] { typeof(EnumFilters), typeof(CommentFilters) }),
            [typeof(EnumField)] =
              new FormatterTemplate("enum_field", new[] { typeof(EnumField), typeof(CommentFilters) }),
            [typeof(Definitions)] =
              new FormatterTemplate("definitions", new[] { typeof(DefinitionsFilters) }),
            [typeof(ServiceClient)] =
              new FormatterTemplate("client", new[] { typeof(ServiceClientFilters), typeof(CommentFilters), typeof(ClassFilters) }),
            [typeof(Method)] =
              new FormatterTemplate("client_method", new[] { typeof(ServiceClientMethodFilters), typeof(CommentFilters), typeof(ClassFilters) }),
        };

        private FormatterTemplates() { }

        public static IFormatterTemplates Current { get; set; } = new FormatterTemplates();

        public FormatterTemplate GetTemplate<TObject>()
        {
            return _templates[typeof(TObject)];
        }

        public void Render<TObject>(TextWriter writer, TObject variable)
        {
            GetTemplate<TObject>().Render(writer, variable);
        }
    }
}
