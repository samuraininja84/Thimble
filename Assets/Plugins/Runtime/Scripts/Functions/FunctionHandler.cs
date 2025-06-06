using System;
using System.Collections.Generic;
using Yarn.Unity;

namespace Thimble
{
    public static class FunctionHandler
    {
        #region Method Function Creators

        public static Function CreateFunction<T1>(string functionOrigin, string functionName, Func<T1> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier1Function<T1> functionT1 = new Tier1Function<T1>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT1;
            return function;
        }

        public static Function CreateFunction<T1, T2>(string functionOrigin, string functionName, Func<T1, T2> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier2Function<T1, T2> functionT2 = new Tier2Function<T1, T2>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT2;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3>(string functionOrigin, string functionName, Func<T1, T2, T3> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier3Function<T1, T2, T3> functionT3 = new Tier3Function<T1, T2, T3>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT3;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4>(string functionOrigin, string functionName, Func<T1, T2, T3, T4> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier4Function<T1, T2, T3, T4> functionT4 = new Tier4Function<T1, T2, T3, T4>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT4;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier5Function<T1, T2, T3, T4, T5> functionT5 = new Tier5Function<T1, T2, T3, T4, T5>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT5;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier6Function<T1, T2, T3, T4, T5, T6> functionT6 = new Tier6Function<T1, T2, T3, T4, T5, T6>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT6;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier7Function<T1, T2, T3, T4, T5, T6, T7> functionT7 = new Tier7Function<T1, T2, T3, T4, T5, T6, T7>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT7;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier8Function<T1, T2, T3, T4, T5, T6, T7, T8> functionT8 = new Tier8Function<T1, T2, T3, T4, T5, T6, T7, T8>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT8;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier9Function<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionT9 = new Tier9Function<T1, T2, T3, T4, T5, T6, T7, T8, T9>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT9;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionMethod, string functionDescription, string functionSyntax)
        {
            Tier10Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionT10 = new Tier10Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(functionOrigin, functionName, functionMethod, functionDescription, functionSyntax);
            Function function = functionT10;
            return function;
        }

        #endregion

        #region Tier Function Creators

        public static Function CreateFunction<T1>(Tier1Function<T1> functionT1)
        {
            Function function = functionT1;
            return function;
        }

        public static Function CreateFunction<T1, T2>(Tier2Function<T1, T2> functionT2)
        {
            Function function = functionT2;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3>(Tier3Function<T1, T2, T3> functionT3)
        {
            Function function = functionT3;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4>(Tier4Function<T1, T2, T3, T4> functionT4)
        {
            Function function = functionT4;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5>(Tier5Function<T1, T2, T3, T4, T5> functionT5)
        {
            Function function = functionT5;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6>(Tier6Function<T1, T2, T3, T4, T5, T6> functionT6)
        {
            Function function = functionT6;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7>(Tier7Function<T1, T2, T3, T4, T5, T6, T7> functionT7)
        {
            Function function = functionT7;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8>(Tier8Function<T1, T2, T3, T4, T5, T6, T7, T8> functionT8)
        {
            Function function = functionT8;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Tier9Function<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionT9)
        {
            Function function = functionT9;
            return function;
        }

        public static Function CreateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Tier10Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionT10)
        {
            Function function = functionT10;
            return function;
        }

        #endregion

        #region Function Data Methods

        public static void AddFunction(DialogueRunner runner, FunctionData functionData, Function function, List<Function> functionList = null)
        {
            functionData.AddFunction(runner, function);
            if (functionList != null && !functionList.Contains(function)) functionList.Add(function);
        }

        public static void RemoveFunction(DialogueRunner runner, FunctionData functionData, Function function, List<Function> functionList = null)
        {
            functionData.RemoveFunction(runner, function);
            if (functionList != null && functionList.Contains(function)) functionList.Remove(function);
        }

        public static void RemoveFunction(DialogueRunner runner, FunctionData functionData, string functionName, List<Function> functionList = null)
        {
            functionData.RemoveFunction(runner, functionName);
            if (functionList != null)
            {
                Function function = functionList.Find(c => c.functionName == functionName);
                if (function != null) functionList.Remove(function);
            }
        }

        public static void ActivateFunction(DialogueRunner runner, FunctionData functionData, Function function)
        {
            functionData.ActivateFunction(runner, function);
        }

        public static void DeactivateFunction(DialogueRunner runner, FunctionData functionData, Function function)
        {
            functionData.DeactivateFunction(runner, function);
        }

        #endregion
    }
}
