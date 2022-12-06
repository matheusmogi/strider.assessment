using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Strider.Posterr.Common;


[ExcludeFromCodeCoverage]
public static class Check
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ArgumentNotNull(
        object argument,
        [CallerArgumentExpression("argument")] string paramName = null)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
 
}