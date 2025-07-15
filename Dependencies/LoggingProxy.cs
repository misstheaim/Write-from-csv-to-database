using ImpromptuInterface;
using System.Dynamic;
using System.Text;
using System.Reflection;
using Stream_Processing;

namespace Logging_Proxy;

public class LoggingProxy<T> : DynamicObject where T : class
{
    private T? obj;

    private readonly Logger _logger;

    public LoggingProxy(Logger logger)
    {
        _logger = logger;
    }

    public T CreateInstance(T obj) 
    {
        if (obj == null) throw new ArgumentNullException("Logging object cannot be null");
        this.obj = obj;
        return this.ActLike<T>();
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = null;
        PropertyInfo? property = typeof(T).GetProperty(binder.Name);
        if (property != null)
        {
            result = property.GetValue(obj);
            _logger.Log($"The property {binder.Name} of Type is accessed.");
            return true;
        }
        return false;
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (value is not null)
        {
            PropertyInfo? property = typeof(T).GetProperty(binder.Name);
            if (property != null && property.PropertyType == value.GetType())
            {
                property.SetValue(obj, value);
                _logger.Log($"The property {binder.Name} of Type is assigned with value {value}.");
                return true;
            }
            return false;
        }
        return false;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        result = null;
        StringBuilder consoleMessage = new();

        consoleMessage.Append($"You are invoking method {binder.Name}");
        if (args != null && args.Length > 0)
        {
            consoleMessage.Append($", with parameters: ");
            foreach (var arg in args)
            {
                consoleMessage.Append(arg?.ToString() + "; ");
            }
        }

        _logger.Log(consoleMessage.ToString());

        Type type = typeof(T);
        MethodInfo? method = type.GetMethods().Where(m => m.Name.Equals(binder.Name)
                && m.GetParameters().Length == (args == null ? 0 : args.Length)).FirstOrDefault(m =>
        {
            ParameterInfo[] parameters = m.GetParameters();
            bool isParamsTypesEqual = true;
            int index = 0;
            foreach (var param in parameters)
            {
                if (args[index] != null && !param.ParameterType.IsAssignableFrom(args[index].GetType())) isParamsTypesEqual = false;
                index++;
            };
            return isParamsTypesEqual;
        });

        bool isVoid = false;

        if (method != null)
        {
            try
            {
                result = method.Invoke(obj, args);
            }
            catch (TargetInvocationException ex)
            {
                _logger.Log($"Target invocation {ex}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
            }

            isVoid = method.ReturnType == typeof(void);

        }
        
        if (result is not null || isVoid)
        {
            _logger.Log($"Method { binder.Name} is successfully invoked.");
            return true;
        }
        _logger.Log($"Method {binder.Name} invocation is failed.");
        return false;
    }
}
