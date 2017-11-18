using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Antianeira.MetadataReader
{
    public interface IActionChecker
    {
        bool IsAvailableAction([NotNull] MethodInfo method);
    }

    public class MvcCoreActionChecker : IActionChecker
    {
        public bool IsAvailableAction(MethodInfo method)
        {
            return method.IsPublic && !typeof(IActionResult).IsAssignableFrom(method.ReturnType);
        }
    }
}
