using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Armpit.Library.Utils;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? item)
    {
        if(item == null)
            return true;
        if(item.Length == 0)
            return true;
        return false;
    }
}

public static class ILoggerExtensions
{
    public static void LogError_ExceptionWithMethodName(this ILogger logger, Exception ex)
    {
        var methodInfo = new StackTrace().GetFrame(1).GetMethod();
        logger.LogInformation(ExceptionHelper.GetExceptionOfTypeCaughtInWithMessageString(methodInfo, ex));
    }
}