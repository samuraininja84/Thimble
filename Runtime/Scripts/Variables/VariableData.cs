using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [CreateAssetMenu(fileName = "New Variable Data", menuName = "Thimble/Variables/New Variable Data")]
    public class VariableData : ScriptableObject
    {
        [Header("Yarn Spinner")]
        public InMemoryVariableStorage variableStorage;
    }
}
