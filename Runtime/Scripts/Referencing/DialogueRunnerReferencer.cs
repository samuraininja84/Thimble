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

        private void Awake()
        {
            SetDialogueRunner(GetDialogueRunner());
        }

        private void OnEnable()
        {
            SetDialogueRunner(GetDialogueRunner());
        }

        private void OnDisable()
        {
            SetDialogueRunner();
        }

        private void SetDialogueRunner(DialogueRunner runner = null)
        {
            if (commandData != null || commandData.Count > 0)
            {
                foreach (CommandData data in commandData)
                {
                    data.dialogueRunner = runner;
                }
            }

            if (functionData != null || functionData.Count > 0)
            {
                foreach (FunctionData data in functionData)
                {
                    data.dialogueRunner = runner;
                }
            }
        }

        private DialogueRunner GetDialogueRunner()
        {
            // If the dialogue runner is not set, try to get it from the game object, then return it
            if (dialogueRunner == null)
            {
                dialogueRunner = GetComponent<DialogueRunner>();
            }

            return dialogueRunner;
        }
    }
}