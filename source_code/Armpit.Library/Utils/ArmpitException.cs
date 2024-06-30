using System.Diagnostics;

namespace Armpit.Library.Utils;

public class ArmpitException : Exception
{

    public ArmpitException() { }

    public ArmpitException(string? message) : base(message) { }

    public ArmpitException(string? message, Exception innerException) : base(message, innerException) { }

    /// <summary>
    /// Throws <see cref="ArmpitException"/> with message containing class name and method name which called this static method.
    /// </summary>
    /// <param name="ex">Original exception.</param>
    /// <exception cref="ArmpitException"></exception>
    public static ArmpitException GetExceptionWithMethodName(Exception ex)
    {
        var methodInfo = new StackTrace().GetFrame(1).GetMethod();
        return new ArmpitException(ExceptionHelper.GetExceptionOfTypeCaughtInWithMessageString(methodInfo, ex));
    }

}
