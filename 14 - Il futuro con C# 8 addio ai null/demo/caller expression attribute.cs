using System;
using System.Runtime.CompilerServices;

public class C {
    public void M() {
        int r = 1;
        Assert(r == 1);
    }
    
     public static void Assert(bool condition, [CallerArgumentExpression("condition")] string message = null)
     {
     }
}







namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}