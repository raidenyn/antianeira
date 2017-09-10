﻿using System.Reflection;
using Antianeira.Utils;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public interface IMethodNameMapper
    {
        [NotNull]
        string GetMethodName([NotNull] MethodInfo method);
    }

    public class CamelCaseMethodName : IMethodNameMapper
    {
        public string GetMethodName(MethodInfo method)
        {
            var name = method.Name;
            if (name.EndsWith("Async")) {
                name = name.Substring(0, name.Length - 4);
            }

            return StringUtils.ToCamelCase(name);
        }
    }
}