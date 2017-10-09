﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Monkeyspeak
{
    internal class ReflectionHelper
    {
        public static Type[] GetAllTypesWithAttributeInMembers<T>(Assembly assembly) where T : Attribute
        {
            return assembly.GetTypes().Where(type => type.GetMembers().Any(member => member.GetCustomAttribute<T>() != null)).ToArray();
        }

        public static IEnumerable<T> GetAllAttributesFromMethod<T>(MethodInfo methodInfo) where T : Attribute
        {
            var attributes = (T[])methodInfo.GetCustomAttributes(typeof(T), false);
            if (attributes != null && attributes.Length > 0)
                for (int k = 0; k <= attributes.Length - 1; k++)
                {
                    yield return attributes[k];
                }
        }

        public static IEnumerable<MethodInfo> GetAllMethods(Type type)
        {
            MethodInfo[] methods = type.GetMethods();
            for (int j = 0; j <= methods.Length - 1; j++)
            {
                yield return methods[j];
            }
        }

        public static bool TryLoad(string assemblyFile, out Assembly asm)
        {
            try
            {
                asm = Assembly.LoadFile(Path.GetFullPath(assemblyFile));
                return true;
            }
#if DEBUG
            catch (Exception ex)
#else
            catch
#endif
            {
                asm = null;
#if DEBUG
                throw ex;
#else
                return false;
#endif
            }
        }
    }
}