using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [CreateAssetMenu(fileName = "New Function Data", menuName = "Thimble/Functions/New Function Data")]
    public class FunctionData : ScriptableObject
    {
        [Header("Dialogue Runners")]
        public List<DialogueRunner> runners = new();

        [Header("Functions")]
        public List<Function> functions = new();

        public void Register(DialogueRunner runner)
        {
            // Check if the runner is null or already registered, if it is, return
            if (runner == null || runners.Contains(runner)) return;

            // Add the runner to the set of runners
            runners.Add(runner);

            // Set up all functions for the new runner
            functions.ForEach(function => function.AddFunction(runner));
        }

        public void Unregister(DialogueRunner runner)
        {
            // Check if the runner is null or not registered, if it is, return
            if (runner == null || !runners.Contains(runner)) return;

            // Remove all functions from the runner
            functions.ForEach(function => function.RemoveFunction(runner));

            // Remove the runner from the set of runners
            runners.Remove(runner);
        }
        
        public bool HasRunners() => runners != null && runners.Count > 0;

        #region Function Methods

        public void AddFunction(Function function)
        {
            // Add the function to the list of functions and set the function state to inactive
            Add(function);

            // Set the function state to inactive
            function.SetFunctionState(FunctionState.Inactive);
        }

        public void RemoveFunction(Function function)
        {
            // Check if the function is in the list of functions, if it is, remove it
            if (functions.Contains(function))
            {
                // Deactivate the function
                DeactivateFunction(function);

                // Remove the function from the list
                Remove(function);
            }
        }

        public void RemoveFunction(string functionName)
        {
            // Find the function with the matching name, if it exists, remove it
            Function functionToRemove = functions.Find(func => func.functionName == functionName) ?? null;

            // If the function exists, remove it
            if (functionToRemove != null)
            {
                // Deactivate the function
                DeactivateFunction(functionToRemove);

                // Remove the function from the list
                Remove(functionToRemove);
            }
        }

        public void ActivateFunction(Function function)
        {
            // Add the function to the list of functions if it is not already in the list
            Add(function);

            // Add the function all registered dialogue runners
            runners.ForEach(runner => function.AddFunction(runner));
        }

        public void DeactivateFunction(Function function)
        {
            // Check if the function is active, if it is not, return
            if (!function.FunctionActive()) return;

            // Remove the function from all registered dialogue runners
            runners.ForEach(runner => function.RemoveFunction(runner));
        }

        [ContextMenu("Activate All Functions")]
        public void ActivateAllFunctions() => functions.ForEach(function => ActivateFunction(function));

        [ContextMenu("Deactivate All Functions")]
        public void DeactivateAllFunctions() => functions.ForEach(function => DeactivateFunction(function));

        [ContextMenu("Clear All Functions")]
        public void ClearAllFunctions()
        {
            // Deactivate all functions
            DeactivateAllFunctions();

            // Clear the list of functions
            Clear();
        }

        #endregion

        #region List Management

        private void Add(Function function)
        {
            // Add the function to the list if it is not already in the list
            if (!functions.Contains(function)) functions.Add(function);
        }

        private void Remove(Function function)
        {
            // Remove the function from the list if it is in the list
            if (functions.Contains(function)) functions.Remove(function);
        }

        private void Clear() => functions.Clear();

        #endregion
    }
}
