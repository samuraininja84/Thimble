using UnityEngine;
using UnityEngine.SceneManagement;

namespace Thimble
{
    public class YarnObjectReference : MonoBehaviour, ICachedYarnObject
    {
        [Header("Yarn Object Information")]
        [Tooltip("The name of the Yarn object that this object is associated with. This is used for all objects, and should be unique across all objects in the project.")]
        [SerializeField] private string yarnName;
        [Tooltip("The name of the condition that this object is associated with. This is used for condition objects, and should be left blank for other objects.")]
        [SerializeField] private BoolVariable condition = BoolVariable.Default;
        [Tooltip("The desired value for the condition to be for it to be the valid name for the object. Defaults to true.")]
        [SerializeField] private bool desiredValue = true;
        [Tooltip("Whether this object is local to the scene or not. Local objects will get their scene name appended to their YarnName in the cache.")]
        [SerializeField] private bool isLocal = false;

        public string YarnName => string.IsNullOrEmpty(yarnName) ? gameObject.name : yarnName;

        public string ObjectName => gameObject.name;

        public string ConditionName => condition.GetName();

        public bool DesiredValue => desiredValue;

        public string SceneName => SceneManager.GetActiveScene().name;

        public bool IsLocal => isLocal;
    }
}
