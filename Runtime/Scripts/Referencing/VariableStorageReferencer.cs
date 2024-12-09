using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [AddComponentMenu("Scripts/Yarn Spinner/Variable Storage Referencer")]
    public class VariableStorageReferencer : MonoBehaviour
    {
        [Header("Yarn Spinner")]
        public InMemoryVariableStorage variableStorage;

        [Header("Variable Storage")]
        public VariableData variableData;

        private void Awake()
        {
            SetVariableStorage(GetVariableStorage());
        }

        private void OnEnable()
        {
            SetVariableStorage(GetVariableStorage());
        }

        private void OnDisable()
        {
            SetVariableStorage();
        }

        private void SetVariableStorage(InMemoryVariableStorage storage = null)
        {
            variableStorage = storage;
            variableData.variableStorage = storage;
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