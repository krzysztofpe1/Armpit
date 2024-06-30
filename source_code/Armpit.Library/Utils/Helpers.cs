using System.Diagnostics;
using System.Reflection;

namespace Armpit.Library.Utils;

internal static class ExceptionHelper
{
    /// <summary>
    /// Call new StackTrace().GetFrame(1).GetMethod() and the pass result to this method.
    /// </summary>
    /// <param name="method"></param>
    /// <returns><see cref="string"/>: "Exception caught in [className].[methodName]"</returns>
    public static string GetExceptionCaughtInString(MethodBase? method)
    {
        var className = method.ReflectedType.Name;
        var methodName = method.Name;

        return $"Exception caught in {className}.{methodName}.";
    }

    /// <summary>
    /// Call new StackTrace().GetFrame(1).GetMethod() and pass the result and exception to this method.
    /// </summary>
    /// <param name="method">Original method information</param>
    /// <returns><see cref="string"/>: "Exception of type [exceptionType] caught in [className].[methodName]"</returns>
    public static string GetExceptionOfTypeCaughtInString(MethodBase? method, Exception ex)
    {
        var className = method.ReflectedType.Name;
        var methodName = method.Name;

        return $"Exception of type {ex.GetType().Name} caught in {className}.{methodName}.";
    }

    public static string GetExceptionOfTypeCaughtInWithMessageString(MethodBase? method, Exception ex)
    {
        var className = method.ReflectedType.Name;
        var methodName = method.Name;

        return $"Exception of type {ex.GetType().Name} caught in {className}.{methodName}. Exception message: {ex.Message}";
    }
}
