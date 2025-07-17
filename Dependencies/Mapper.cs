using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Generic_object_mapper;

public static class Mapper
{
    public static TResult Map<TSource, TResult>(TSource obj)
    {
        Type sourceType = typeof(TSource);
        Type resultType = typeof(TResult);

        if (obj == null)
        {
            throw new ArgumentNullException(sourceType.Name);
        }

        bool isConstrAvailable = resultType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes) is not null;

        if (!isConstrAvailable) throw new ArgumentException($"The type {resultType.FullName} doesn't have parameterless constructor.");

        PropertyInfo[] properties = sourceType.GetProperties();

        TResult result = Activator.CreateInstance<TResult>();

        AssignProperties(properties, resultType, obj, result);

        return result;
    }

    public static TResult Map<TSource, TResult>(TSource obj, Func<TSource, TResult> func)
    {
        Type sourceType = typeof(TSource);
        Type resultType = typeof(TResult);

        if (obj == null)
        {
            throw new ArgumentNullException(sourceType.Name);
        }

        PropertyInfo[] properties = sourceType.GetProperties();

        TResult result = func.Invoke(obj);

        AssignProperties(properties, resultType, obj, result);

        return result;
    }

    private static void AssignProperties<TResult, TSource>(PropertyInfo[] properties, Type resultType, TSource obj, TResult result)
    {
        foreach (PropertyInfo property in properties)
        {
            PropertyInfo? resultProperty = resultType.GetProperty(property.Name);
            if (resultProperty is null) continue;
            if (resultProperty.PropertyType == property.PropertyType)
            {
                resultProperty.SetValue(result, property.GetValue(obj));
            }
            else if (resultProperty.GetCustomAttribute<RequiredAttribute>() is not null)
            {
                throw new InvalidOperationException($"Trying to assign null value to the required property {resultProperty.Name}");
            }
        }
    }
}
