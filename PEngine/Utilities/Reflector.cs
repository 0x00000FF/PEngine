using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace PEngine.Utilities
{
    public static class Reflector
    {
        public static D GetPrivateMethod<D>(this object? target, string name) where D : Delegate
        {
            var method = target?.GetType()
                                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                                .FirstOrDefault(m => m.Name == name);

            if (method is null)
                throw new InvalidOperationException();

            return method.CreateDelegate<D>(target);
        }
    }
}
