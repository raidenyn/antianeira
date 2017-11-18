using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Antianeira.Schema;
using Antianeira.Utils;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IMethodUrlMapper
    {
        [NotNull]
        MethodUrl MapMethodUrl([NotNull] MethodInfo methodInfo, [NotNull, ItemNotNull] IReadOnlyCollection<ServiceMethodParameter> parameters);
    }

    public class MethodUrlMapper: IMethodUrlMapper
    {
        private readonly IUrlTemplateMapper _urlTemplateMapper;
        private readonly Regex _urlPathParameter = new Regex(@"{(\w+).*?}", RegexOptions.Compiled);

        public MethodUrlMapper(IUrlTemplateMapper urlTemplateMapper)
        {
            _urlTemplateMapper = urlTemplateMapper;
        }

        public MethodUrl MapMethodUrl(MethodInfo methodInfo, IReadOnlyCollection<ServiceMethodParameter> parameters)
        {
            var tempalte = _urlTemplateMapper.MapUrlTemplate(methodInfo);

            var urlParts = tempalte.Split("/", StringSplitOptions.RemoveEmptyEntries);

            var methodUrl = new MethodUrl();

            var excludes = new HashSet<string>();

            foreach (var urlPart in urlParts)
            {
                var segment = Segment(urlPart, parameters);

                if (segment != null)
                {
                    excludes.Add(segment.SourceName);
                    methodUrl.Segments.Add(segment);
                }
            }

            var query = parameters.Where(p => p.PassOver == ParameterPassOver.Query)
                                  .SelectMany(AsQuery)
                                  .Where(item => !excludes.Contains(item.SourceName));
            methodUrl.QueryItems.AddRange(query);

            return methodUrl;
        }

        [CanBeNull]
        private IMethodUrlPathSegment Segment([NotNull] string urlPart, [NotNull, ItemNotNull] IReadOnlyCollection<ServiceMethodParameter> parameters)
        {
            var match = _urlPathParameter.Match(urlPart);
            if (!match.Success)
            {
                return AsConst(urlPart);
            }

            var paramName = match.Groups[1].Value;

            return AsParameter(paramName, parameters) ?? AsProperty(paramName, parameters);
        }

        [NotNull, ItemNotNull]
        private IEnumerable<IMethodUrlQueryItem> AsQuery([NotNull] ServiceMethodParameter parameter)
        {
            var properties = GetProperties(parameter).ToArray();

            if (properties.Any())
            {
                foreach (var property in properties)
                {
                    yield return new ValueQueryItem(parameter, property);
                }
            }
            else
            {
                yield return new ParameterQueryItem(parameter);
            }
        }

        [NotNull, ItemNotNull]
        private IEnumerable<Property> GetProperties([NotNull] ServiceMethodParameter parameter)
        {
            return from structure in parameter.Type.Types.GetStructures()
                from property in structure.Properties
                group property by property.Name into g
                select g.First();
        }

        [NotNull]
        private IMethodUrlPathSegment AsConst([NotNull] string urlPart)
        {
            return new ConstSegment { Value = urlPart };
        }

        [CanBeNull]
        private IMethodUrlPathSegment AsParameter([NotNull] string paramName, [NotNull, ItemNotNull] IReadOnlyCollection<ServiceMethodParameter> parameters)
        {
            var param = parameters.FirstOrDefault(parameter => String.Equals(parameter.Source?.Name ?? parameter.Name, paramName, StringComparison.OrdinalIgnoreCase));

            return param != null ? new ParameterSegment(param) : null;
        }

        [CanBeNull]
        private IMethodUrlPathSegment AsProperty([NotNull] string paramName, [NotNull, ItemNotNull] IReadOnlyCollection<ServiceMethodParameter> parameters)
        {
            return (from parameter in parameters
                from anyType in parameter.Type.Types
                let customType = anyType as CustomType
                let structure = customType?.Type as IStructure
                let properties = structure?.Properties
                where properties != null
                from property in properties
                where String.Equals(property.Source?.Name ?? property.Name, paramName, StringComparison.OrdinalIgnoreCase)
                select new PropertySegment
                {
                    Property = property,
                    Parameter = parameter
                }).FirstOrDefault();
        }
    }
}
