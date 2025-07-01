using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    [AddComponentMenu("Scripts/Yarn Spinner/Dialogue Runner Referencer")]
    public class DialogueRunnerReferencer : MonoBehaviour
    {
        [Header("Dialogue Runner")]
        public DialogueRunner dialogueRunner;

        [Header("Commands")]
        public List<CommandData> commandData;

        [Header("Functions")]
        public List<FunctionData> functionData;

        private void Awake() => SetDialogueRunner();

        private void OnDestroy() => RemoveDialogueRunner();

        private void SetDialogueRunner()
        {
            // Get the dialogue runner component, if it is not set
            dialogueRunner = GetDialogueRunner();

            // If there are command data objects defined, iterate through them and set their dialogue runner
            commandData.ForEach(data => data.SetRunner(dialogueRunner));

            // If there are function data objects defined, iterate through them and set their dialogue runner
            functionData.ForEach(data => data.SetRunner(dialogueRunner));
        }

        private void RemoveDialogueRunner()
        {
            // If there are command data objects defined, iterate through them and remove their dialogue runner
            commandData.ForEach(data => data.SetRunner(null));

            // If there are function data objects defined, iterate through them and remove their dialogue runner
            functionData.ForEach(data => data.SetRunner(null));
        }

        private DialogueRunner GetDialogueRunner() => dialogueRunner ?? GetComponent<DialogueRunner>();
    }
}