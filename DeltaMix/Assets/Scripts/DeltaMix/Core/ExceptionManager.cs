using System;
using System.Runtime.CompilerServices;

namespace DeltaMix.Core
{
    public static class ExceptionManager
    {
        public static void Throw<T>(string message = "", [CallerMemberName] string callerName = "")
            where T : Exception, new()
        {
            string eMessage = $"{ typeof(T) }:{ message } from { callerName }";
            throw Activator.CreateInstance(typeof(T), eMessage) as T;
        }
    }
}
