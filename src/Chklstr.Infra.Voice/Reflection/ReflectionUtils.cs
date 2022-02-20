using System.Reflection;

namespace Chklstr.Infra.Voice.Reflection;

public static class ReflectionUtils
{
    public static object? CallNonPublic(this object? target, string methodName, object?[]? parameters)
    {
        if (target == null)
        {
            throw new ArgumentException($"Target cannot be null");
        }

        var methodInfo = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (methodInfo == null)
        {
            throw new ArgumentException($"Method {methodName} not found on {target.GetType()}");
        }

        return methodInfo.Invoke(target, parameters);
    }
    
    public static object? CallStatic(this Type targetType, string methodName, object?[]? parameters)
    {
        var methodInfo = targetType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        if (methodInfo == null)
        {
            throw new ArgumentException($"Method {methodName} not found on {targetType}");
        }

        return methodInfo.Invoke(null, parameters);
    }

    public static T? GetPropertyValue<T>(this object target, string propertyName) where T : class
    {
        var propInfo = target.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (propInfo == null)
        {
            throw new ArgumentException($"Property {propertyName} not found on {target}");
        }

        return propInfo.GetValue(target) as T;
    }
    
    public static T? GetFieldValue<T>(this object target, string fieldName) where T : class
    {
        var fieldInfo = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        if (fieldInfo == null)
        {
            throw new ArgumentException($"Property {fieldName} not found on {target}");
        }

        return fieldInfo.GetValue(target) as T;
    }

}