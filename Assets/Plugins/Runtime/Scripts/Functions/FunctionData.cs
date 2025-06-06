using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [CreateAssetMenu(fileName = "New Function Data", menuName = "Thimble/Functions/New Function Data")]
    public class FunctionData : ScriptableObject
    {
        [Header("Dialogue Runner")]
        public DialogueRunner dialogueRunner;

        [Header("Functions")]
        public List<Function> functions = new List<Function>();

        #region Function Methods

        public void AddFunction(DialogueRunner runner, Function function)
        {
            // Add the function to the list of functions and set the function state to inactive
            Add(function);
            function.SetFunctionState(FunctionState.Inactive);
        }

        public void RemoveFunction(DialogueRunner runner, Function function)
        {
            // Check if the function is in the list of functions, if it is, remove it
            if (functions.Contains(function))
            {
                DeactivateFunction(runner, function);
                Remove(function);
            }
        }

        public void RemoveFunction(DialogueRunner runner, string functionName)
        {
            // Find the function with the matching name, if it exists, remove it
            Function functionToRemove = null;
            foreach (Function function in functions)
            {
                if (function.functionName == functionName)
                {
                    functionToRemove = function;
                    break;
                }
            }

            if (functionToRemove != null)
            {
                DeactivateFunction(runner, functionToRemove);
                Remove(functionToRemove);
            }
        }

        public void ActivateAllFunctions(DialogueRunner runner)
        {
            foreach (Function function in functions)
            {
                ActivateFunction(runner, function);
            }
        }

        public void ActivateFunction(DialogueRunner runner, Function function)
        {
            // Add the function to the list of functions if it is not already in the list
            Add(function);

            // Add the function to the dialogue runner
            function.AddFunction(runner);
        }

        public void DeactivateAllFunctions(DialogueRunner runner)
        {
            foreach (Function function in functions)
            {
                DeactivateFunction(runner, function);
            }
        }

        public void DeactivateFunction(DialogueRunner runner, Function function)
        {
            if (!function.FunctionActive()) return;
            else function.RemoveFunction(runner);
        }

        public void ClearAllFunctions(DialogueRunner runner)
        {
            DeactivateAllFunctions(runner);
            functions.Clear();
        }

        #endregion

        #region List Management

        public void Add(Function function)
        {
            if (!functions.Contains(function)) functions.Add(function);
        }

        public void Remove(Function function)
        {
            if (functions.Contains(function)) functions.Remove(function);
        }

        #endregion
    }
}
