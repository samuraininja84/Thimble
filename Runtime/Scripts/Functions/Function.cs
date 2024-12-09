using System;
using Yarn.Unity;

namespace Thimble
{
    [Serializable]
    public class Function : IRegisterFunctions
    {
        public FunctionState functionState = FunctionState.Inactive;
        public FunctionTier functionTier = FunctionTier.Tier0;
        public string functionOrigin;
        public string functionName;
        public string functionDescription;
        public string functionSyntax;

        #region Virtual Methods

        public virtual void AddFunction(DialogueRunner dialogueRunner) { }

        public virtual void RemoveFunction(DialogueRunner dialogueRunner) { }

        #endregion

        #region Public Methods

        public void SetFunctionState(FunctionState state)
        {
            functionState = state;
        }

        public string GetFunctionInfo()
        {
            return $"Command Tier: {FunctionTierToString()}\nFunction Origin: {functionOrigin}\nFunction Name: {functionName}\nFunction Description: {functionDescription}\nFunction Syntax: {functionSyntax}";
        }

        public string FunctionTierToString()
        {
            return functionTier.ToString().Insert(4, " ");
        }

        public bool FunctionActive()
        {
            return functionState == FunctionState.Active;
        }

        #endregion
    }

    public enum FunctionState
    {
        Inactive,
        Active
    }

    public enum FunctionTier
    {
        Tier0,
        Tier1,
        Tier2,
        Tier3,
        Tier4,
        Tier5,
        Tier6,
        Tier7,
        Tier8,
        Tier9,
        Tier10
    }

    #region Function Tiers

    [Serializable]
    public class Tier1Function<T1> : Function
    {
        public Func<T1> functionMethod;

        public Tier1Function(string functionOrigin, string functionName, Func<T1> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier1;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier2Function<T1, T2> : Function
    {
        public Func<T1, T2> functionMethod;

        public Tier2Function(string functionOrigin, string functionName, Func<T1, T2> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier2;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier3Function<T1, T2, T3> : Function
    {
        public Func<T1, T2, T3> functionMethod;

        public Tier3Function(string functionOrigin, string functionName, Func<T1, T2, T3> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier3;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier4Function<T1, T2, T3, T4> : Function
    {
        public Func<T1, T2, T3, T4> functionMethod;

        public Tier4Function(string functionOrigin, string functionName, Func<T1, T2, T3, T4> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier4;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier5Function<T1, T2, T3, T4, T5> : Function
    {
        public Func<T1, T2, T3, T4, T5> functionMethod;

        public Tier5Function(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier5;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier6Function<T1, T2, T3, T4, T5, T6> : Function
    {
        public Func<T1, T2, T3, T4, T5, T6> functionMethod;

        public Tier6Function(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier6;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier7Function<T1, T2, T3, T4, T5, T6, T7> : Function
    {
        public Func<T1, T2, T3, T4, T5, T6, T7> functionMethod;

        public Tier7Function(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier7;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier8Function<T1, T2, T3, T4, T5, T6, T7, T8> : Function
    {
        public Func<T1, T2, T3, T4, T5, T6, T7, T8> functionMethod;

        public Tier8Function(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier8;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier9Function<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Function
    {
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionMethod;

        public Tier9Function(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier9;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    [Serializable]
    public class Tier10Function<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Function
    {
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionMethod;

        public Tier10Function(string functionOrigin, string functionName, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> functionMethod, string functionDescription, string functionSyntax)
        {
            this.functionOrigin = functionOrigin;
            this.functionName = functionName;
            this.functionMethod = FunctionMethod(functionMethod);
            this.functionDescription = functionDescription;
            this.functionSyntax = functionSyntax;
            functionTier = FunctionTier.Tier10;
        }

        public override void AddFunction(DialogueRunner dialogueRunner)
        {
            dialogueRunner.AddFunction(functionName, functionMethod);
            functionState = FunctionState.Active;
        }

        public override void RemoveFunction(DialogueRunner dialogueRunner)
        {
            functionState = FunctionState.Inactive;
            dialogueRunner.RemoveFunction(functionName);
        }
    }

    #endregion

    public abstract class IRegisterFunctions
    {
        public Func<T1> FunctionMethod<T1>(Func<T1> function) => function;
        public Func<T1, T2> FunctionMethod<T1, T2>(Func<T1, T2> function) => function;
        public Func<T1, T2, T3> FunctionMethod<T1, T2, T3>(Func<T1, T2, T3> function) => function;
        public Func<T1, T2, T3, T4> FunctionMethod<T1, T2, T3, T4>(Func<T1, T2, T3, T4> function) => function;
        public Func<T1, T2, T3, T4, T5> FunctionMethod<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> function) => function;
        public Func<T1, T2, T3, T4, T5, T6> FunctionMethod<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6> function) => function;
        public Func<T1, T2, T3, T4, T5, T6, T7> FunctionMethod<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7> function) => function;
        public Func<T1, T2, T3, T4, T5, T6, T7, T8> FunctionMethod<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8> function) => function;
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> FunctionMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> function) => function;
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> FunctionMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> function) => function;
    }
}