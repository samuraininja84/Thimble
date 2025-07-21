using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;

namespace Thimble
{
    [AddComponentMenu("Scripts/Yarn Spinner/Variable Storage Referencer")]
    public class VariableStorageReferencer : MonoBehaviour
    {
        [Header("Yarn Spinner")]
        public VariableStorageBehaviour variableStorage;

        [Header("Variables")]
        public List<VariableData> variableData;

        private void Awake() => SetVariableStorage();

        private void OnDestroy() => RemoveVariableStorage();

        /// <summary>
        /// Configures the variable storage by initializing it, resolving variable declarations, and associating the storage with the defined variables.
        /// </summary>
        /// <remarks>This method attempts to retrieve the variable storage from the associated component 
        /// and ensures that all defined variables are properly linked to the storage. It also  retrieves the current
        /// values of the variables from the storage after the association.</remarks>
        private void SetVariableStorage()
        {
            // If the storage is null, try to get it from the component
            variableStorage = GetVariableStorage();

            // Workaround for an isssue with getting declarations from Yarn Project
            FindIntialValues(variableStorage);

            // If there variables defined, iterate through them and set their storage
            variableData.ForEach(data => data.SetStorage(variableStorage));

            // Get the variables from the storage
            variableData.ForEach(data => data.GetVariables());
        }

        /// <summary>
        /// Removes all variable storage references and clears the associated data.
        /// </summary>
        /// <remarks>This method iterates through all variables in the collection, if any exist,  and sets
        /// their storage to <see langword="null"/> before clearing their data.</remarks>
        private void RemoveVariableStorage()
        {
            // Clear the variable storage reference for each variable
            variableData.ForEach(data => data.SetStorage(null));

            // Get the initial values from the Yarn Project
            var initialValues = GetInitialValues();

            // Set all variables to their initial values
            variableData.ForEach(data => data.SetAllVariables(initialValues.floats, initialValues.strings, initialValues.bools));
        }

        /// <summary>
        /// Retrieves the variable storage instance used by the component.
        /// </summary>
        /// <remarks>If the <see cref="variableStorage"/> field is not initialized, this method attempts
        /// to retrieve an <see cref="VariableStorageBehaviour"/> component attached to the current GameObject.</remarks>
        /// <returns>An instance of <see cref="VariableStorageBehaviour"/> representing the variable storage.</returns>
        private VariableStorageBehaviour GetVariableStorage() => variableStorage ?? GetComponent<VariableStorageBehaviour>();

        /// <summary>
        /// Initializes variables in the game's variable storage using the initial values defined in the Yarn Project.
        /// </summary>
        /// <remarks>
        /// This method retrieves the initial values from the Yarn Project associated with the active <see cref="DialogueRunner"/> and sets them in the variable storage. 
        /// Supported variable types include <see langword="string"/>, <see langword="float"/>, and <see langword="bool"/>. 
        /// If an unsupported type is encountered, a warning is logged.
        /// </remarks>
        private void FindIntialValues(VariableStorageBehaviour storage)
        {
            var initialValues = GetInitialValues();
            storage.SetAllVariables(initialValues.floats, initialValues.strings, initialValues.bools);
        }

        /// <summary>
        /// Retrieves the initial set of variables categorized by type from the Yarn project.
        /// </summary>
        /// <remarks>This method extracts initial variable values from a Yarn project and organizes them
        /// into separate dictionaries based on their types: float, string, and bool. These dictionaries can be used to
        /// initialize or reset the state of a dialogue system.</remarks>
        /// <returns>A tuple containing three dictionaries:  <list type="bullet"> <item> <description>A dictionary of float
        /// variables, keyed by variable name.</description> </item> <item> <description>A dictionary of string
        /// variables, keyed by variable name.</description> </item> <item> <description>A dictionary of bool variables,
        /// keyed by variable name.</description> </item> </list></returns>
        private (Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools) GetInitialValues()
        {
            // Get the Yarn Project from the Dialogue Runner
            var project = FindFirstObjectByType<DialogueRunner>().YarnProject;

            // Get the initial values from the Yarn Project
            var values = project.InitialValues;

            // Initialize dictionaries to hold different variable types
            var floatVariables = new Dictionary<string, float>();
            var stringVariables = new Dictionary<string, string>();
            var boolVariables = new Dictionary<string, bool>();

            // Iterate through the variable data and populate the dictionaries based on the variable type
            foreach (var pair in values)
            {
                // Get the value from the pair
                var value = pair.Value;

                // Check the type of the value and add it to the appropriate dictionary
                switch (value)
                {
                    case string stringValue:
                        stringVariables.Add(pair.Key, stringValue);
                        break;
                    case float floatValue:
                        floatVariables.Add(pair.Key, floatValue);
                        break;
                    case bool boolValue:
                        boolVariables.Add(pair.Key, boolValue);
                        break;
                }
            }

            // Return the dictionaries containing the variables
            return (floatVariables, stringVariables, boolVariables);
        }
    }
}