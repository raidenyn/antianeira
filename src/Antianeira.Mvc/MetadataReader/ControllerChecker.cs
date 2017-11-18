using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Antianeira.MetadataReader
{
    public interface IControllerChecker
    {
        bool IsAvailableContoller([NotNull] Type type);
    }

    public class MvcControllerChecker: IControllerChecker
    {
        public bool IsAvailableContoller(Type type)
        {
            return typeof(Controller).IsAssignableFrom(type);
        }
    }
}
