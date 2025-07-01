using System;
using Yarn.Unity;

namespace Thimble
{
    public static class FunctionHandler
    {
        #region Method Function Creators

        public static Function CreateFunction<T1>(string functionOrigin, string functionName, Func<T1> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier1Function<T1> functionT1 = new Tier1Function<T1>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT1;
        }

        public static Function CreateFunction<T1, T2>(string functionOrigin, string functionName, Func<T1, T2> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier2Function<T1, T2> functionT2 = new Tier2Function<T1, T2>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT2;
        }

        public static Function CreateFunction<T1, T2, T3>(string functionOrigin, string functionName, Func<T1, T2, T3> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier3Function<T1, T2, T3> functionT3 = new Tier3Function<T1, T2, T3>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT3;
        }

        public static Function CreateFunction<T1, T2, T3, T4>(string functionOrigin, string functionName, Func<T1, T2, T3, T4> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier4Function<T1, T2, T3, T4> functionT4 = new Tier4Function<T1, T2, T3, T4>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT4;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier5Function<T1, T2, T3, T4, T5> functionT5 = new Tier5Function<T1, T2, T3, T4, T5>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT5;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier6Function<T1, T2, T3, T4, T5, T6> functionT6 = new Tier6Function<T1, T2, T3, T4, T5, T6>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT6;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier7Function<T1, T2, T3, T4, T5, T6, T7> functionT7 = new Tier7Function<T1, T2, T3, T4, T5, T6, T7>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT7;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier8Function<T1, T2, T3, T4, T5, T6, T7, T8> functionT8 = new Tier8Function<T1, T2, T3, T4, T5, T6, T7, T8>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT8;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier9Function<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionT9 = new Tier9Function<T1, T2, T3, T4, T5, T6, T7, T8, T9>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT9;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier10Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionT10 = new Tier10Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            return functionT10;
        }

        #endregion

        #region Tier Function Creators

        public static Function CreateFunction<T1>(Tier1Function<T1> functionT1) => functionT1;

        public static Function CreateFunction<T1, T2>(Tier2Function<T1, T2> functionT2) => functionT2;

        public static Function CreateFunction<T1, T2, T3>(Tier3Function<T1, T2, T3> functionT3) => functionT3;

        public static Function CreateFunction<T1, T2, T3, T4>(Tier4Function<T1, T2, T3, T4> functionT4) => functionT4;

        public static Function CreateFunction<T1, T2, T3, T4, T5>(Tier5Function<T1, T2, T3, T4, T5> functionT5) => functionT5;

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6>(Tier6Function<T1, T2, T3, T4, T5, T6> functionT6) => functionT6;

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7>(Tier7Function<T1, T2, T3, T4, T5, T6, T7> functionT7) => functionT7;

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8>(Tier8Function<T1, T2, T3, T4, T5, T6, T7, T8> functionT8) => functionT8;

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Tier9Function<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionT9) => functionT9;

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Tier10Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionT10) => functionT10;

        #endregion

        #region Function Data Methods

        public static void AddFunction(FunctionData functionData, Function function, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            functionData.runner = runner;
            functionData.AddFunction(function);
        }

        public static void RemoveFunction(FunctionData functionData, Function function, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            functionData.runner = runner;
            functionData.RemoveFunction(function);
        }

        public static void RemoveFunction(FunctionData functionData, string functionName, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            functionData.runner = runner;
            functionData.RemoveFunction(functionName);
        }

        public static void ActivateFunction(FunctionData functionData, Function function, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            functionData.runner = runner;
            functionData.ActivateFunction(function);
        }

        public static void DeactivateFunction(FunctionData functionData, Function function, DialogueRunner runner = null)
        {
            // Set the runner if it is not already set
            functionData.runner = runner;
            functionData.DeactivateFunction(function);
        }

        #endregion
    }
}
