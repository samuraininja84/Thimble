using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [AddComponentMenu("Scripts/Thimble/Dialogue Runner Referencer")]
    public class DialogueRunnerReferencer : MonoBehaviour
    {
        [Header("References")]
        public DialogueRunner dialogueRunner;
        public CommandData commandData;
        public FunctionData functionData;

        private void Awake() => SetDialogueRunner();

        private void OnDestroy() => RemoveDialogueRunner();

        private void SetDialogueRunner()
        {
            // Get the dialogue runner component, if it is not set
            dialogueRunner = GetDialogueRunner();

            // If there are command data objects defined, iterate through them and set their dialogue runner
            commandData.Register(dialogueRunner);

            // If there are function data objects defined, iterate through them and set their dialogue runner
            functionData.Register(dialogueRunner);
        }

        private void RemoveDialogueRunner()
        {
            // If there are command data objects defined, iterate through them and remove their dialogue runner
            commandData.Unregister(dialogueRunner);

            // If there are function data objects defined, iterate through them and remove their dialogue runner
            functionData.Unregister(dialogueRunner);
        }

        private DialogueRunner GetDialogueRunner() => dialogueRunner ?? GetComponent<DialogueRunner>();

        private void Reset() => dialogueRunner = GetComponent<DialogueRunner>();
    }
}