using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [AddComponentMenu("Scripts/Yarn Spinner/Variable Storage Referencer")]
    public class VariableStorageReferencer : MonoBehaviour
    {
        [Header("Yarn Spinner")]
        public InMemoryVariableStorage variableStorage;

        [Header("Variables")]
        public List<VariableData> variables;

        private void Awake()
        {
            SetVariableStorage(GetVariableStorage());
        }

        private void OnEnable()
        {
            SetVariableStorage(GetVariableStorage());
            GetVariables();
        }

        private void OnDisable()
        {
            SetVariableStorage();
            ClearVariables();
        }

        private void SetVariableStorage(InMemoryVariableStorage storage = null)
        {
            if (variables != null || variables.Count > 0)
            {
                foreach (VariableData data in variables)
                {
                    data.variableStorage = storage;
                }
            }
        }

        private void GetVariables()
        {
            if (variables != null || variables.Count > 0)
            {
                foreach (VariableData data in variables)
                {
                    data.GetVariables(variableStorage);
                }
            }
        }

        private void ClearVariables()
        {
            if (variables != null || variables.Count > 0)
            {
                foreach (VariableData data in variables)
                {
                    data.Clear();
                }
            }
        }

        private InMemoryVariableStorage GetVariableStorage()
        {
            // If the variable storage is not set, try to get it from the game object, then return it
            if (variableStorage == null)
            {
                variableStorage = GetComponent<InMemoryVariableStorage>();
            }

            return variableStorage;
        }
    }
}