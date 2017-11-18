using System.Reflection;
using Antianeira.Schema;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Antianeira.MetadataReader
{
    public interface IParameterPassOverMapper
    {
        ParameterPassOver MapPassOver([NotNull] ParameterInfo parameter);
    }

    public class AttributeParameterPassOverMapper: IParameterPassOverMapper
    {
        public virtual ParameterPassOver MapPassOver(ParameterInfo parameter)
        {
            if (parameter.GetCustomAttribute<FromBodyAttribute>() != null)
            {
                return ParameterPassOver.Body;
            }
            if (parameter.GetCustomAttribute<FromFormAttribute>() != null)
            {
                return ParameterPassOver.Form;
            }
            if (parameter.GetCustomAttribute<FromHeaderAttribute>() != null)
            {
                return ParameterPassOver.Header;
            }
            if (parameter.GetCustomAttribute<FromQueryAttribute>() != null)
            {
                return ParameterPassOver.Query;
            }
            if (parameter.GetCustomAttribute<FromRouteAttribute>() != null)
            {
                return ParameterPassOver.Route;
            }
            if (parameter.GetCustomAttribute<FromServicesAttribute>() != null)
            {
                return ParameterPassOver.Service;
            }

            return ParameterPassOver.Query;
        }
    }
}
