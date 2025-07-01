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

            // Workaround for sssue with getting declarations from Yarn Project
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
        private void RemoveVariableStorage() => variableData.ForEach(data => data.SetStorage(null));

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
            var project = FindFirstObjectByType<DialogueRunner>().YarnProject;
            var values = project.InitialValues;
            foreach (var pair in values)
            {
                var value = pair.Value;
                switch (value)
                {
                    case string stringValue:
                        storage.SetValue(pair.Key, stringValue);
                        break;
                    case float floatValue:
                        storage.SetValue(pair.Key, floatValue);
                        break;
                    case bool boolValue:
                        storage.SetValue(pair.Key, boolValue);
                        break;
                    default:
                        Debug.LogWarning($"Unsupported variable type for key '{pair.Key}': {value.GetType()}");
                        break;
                }
            }
        }
    }
}