using System.Linq;
using UnityEngine;
using UnityEditor;
using Yarn.Unity;

namespace Thimble.Editor
{
    public class CommandDataEditorWindow : EditorWindow
    {
        public CommandData[] commandDatas;

        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Tools/Thimble/Command Center")]
        public static void ShowWindow() => GetWindow<CommandDataEditorWindow>("Command Center");

        private void OnEnable() => commandDatas = GetCommandData();

        private void OnGUI()
        {
            // Get the height of the window
            float windowHeight = position.height;

            // Add a scroll view to the window to display all command data when there are too many to fit on the screen
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(windowHeight));

            // Check that the command data is not null before drawing them
            if (CommandDataExists())
            {
                // Display the command data label field
                DrawHeader("Command Data");

                // Display all command datas with their respective dialogue runner, commands, and command tools
                for (int i = 0; i < commandDatas.Length; i++)
                {
                    EditorGUILayout.LabelField("Command Data: " + FormatDataName(commandDatas[i].name), EditorStyles.boldLabel);
                    DrawCommandData(commandDatas[i]);
                    EditorGUILayout.Space();

                    // Add a line to separate the command datas
                    if (i < commandDatas.Length - 1) EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    // Add a space between the command datas
                    EditorGUILayout.Space();
                }
            }
            else if (!CommandDataExists())
            {
                // Display the command data label field
                DrawHeader("Command Data");

                // Display an error message if no command datas are found in the folder path
                EditorGUILayout.HelpBox("No Command Data is found in the folder path. Please create a Command Data in the folder path.", MessageType.Error);
            }

            // Draw a button to refresh the command data
            if (GUILayout.Button("Refresh Command Data")) commandDatas = GetCommandData();

            // End the scroll view
            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader(string headerText)
        {
            // Display the command data label field in the middle of the window
            Rect labelPosition = GUILayoutUtility.GetRect(position.width, EditorGUIUtility.singleLineHeight);
            labelPosition.x = labelPosition.width / 2 - 50;
            EditorGUI.LabelField(labelPosition, headerText, EditorStyles.boldLabel);

            // Add a line to separate the command data label field from the command data
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void DrawCommandData(CommandData commandData = null, DialogueRunner dialogueRunner = null)
        {
            // Display the command data field
            EditorGUILayout.LabelField("Command Fields", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            commandData = (CommandData)EditorGUILayout.ObjectField("Command Data", commandData, typeof(CommandData), false);
            EditorGUI.EndDisabledGroup();

            // Check if the command data has a dialogue runner before displaying the dialogue runner field, any existing commands, and the command tools
            if (commandData.dialogueRunner != null)
            {
                // Get the dialogue runner from the command data
                if (dialogueRunner == null) dialogueRunner = commandData.dialogueRunner;

                // Display the dialogue runner field if the command data is not null
                EditorGUI.BeginDisabledGroup(true);
                dialogueRunner = (DialogueRunner)EditorGUILayout.ObjectField("Dialogue Runner", dialogueRunner, typeof(DialogueRunner), true);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();

                // Display all active commands with a bold label if there are any active commands
                EditorGUILayout.LabelField("Active Commands", EditorStyles.boldLabel);
                if (!HasActiveCommands(commandData))
                {
                    EditorGUILayout.LabelField("No active commands found", EditorStyles.miniLabel);
                }
                else if (HasActiveCommands(commandData))
                {
                    for (int i = 0; i < commandData.commands.Count; i++)
                    {
                        // Get the command from the command data and display it if it is active
                        Command command = commandData.commands[i];
                        if (command.CommandActive())
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(command.commandName);

                            // Add a button to display the command's information
                            if (GUILayout.Button("Info"))
                            {
                                EditorUtility.DisplayDialog(command.CommandTierToString() + " Command: " + command.commandName, command.GetCommandInfo(), "OK");
                            }

                            // Add a button to deactivate the command if it is active
                            if (GUILayout.Button("Deactivate"))
                            {
                                commandData.DeactivateCommand(dialogueRunner, command);
                            }

                            // Add a button to remove the command
                            if (GUILayout.Button("Remove"))
                            {
                                commandData.RemoveCommand(dialogueRunner, command);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display all inactive commands with a bold label if there are any inactive commands
                EditorGUILayout.LabelField("Inactive Commands", EditorStyles.boldLabel);
                if (!HasInactiveCommands(commandData))
                {
                    EditorGUILayout.LabelField("No inactive commands found", EditorStyles.miniLabel);
                }
                else if (HasInactiveCommands(commandData))
                {
                    for (int i = 0; i < commandData.commands.Count; i++)
                    {
                        // Get the command from the command data and display it if it is inactive
                        Command command = commandData.commands[i];
                        if (!command.CommandActive())
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(command.commandName);

                            // Add a button to display the command's information
                            if (GUILayout.Button("Info"))
                            {
                                EditorUtility.DisplayDialog(command.CommandTierToString() + " Command: " + command.commandName, command.GetCommandInfo(), "OK");
                            }

                            // Add a button to activate the command if it is inactive
                            if (GUILayout.Button("Activate") && !command.CommandActive())
                            {
                                commandData.ActivateCommand(dialogueRunner, command);
                            }

                            // Add a button to remove the command
                            if (GUILayout.Button("Remove"))
                            {
                                commandData.RemoveCommand(dialogueRunner, command);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display the command tools if there are any commands
                if (HasCommands(commandData))
                {
                    // Display the command tools with a bold label
                    EditorGUILayout.LabelField("Command Tools", EditorStyles.boldLabel);
                    if (GUILayout.Button("Activate All Commands"))
                    {
                        commandData.ActivateAllCommands(dialogueRunner);
                    }
                    if (GUILayout.Button("Deactivate All Commands"))
                    {
                        commandData.DeactivateAllCommands(dialogueRunner);
                    }
                    if (GUILayout.Button("Clear All Commands"))
                    {
                        commandData.ClearAllCommands(dialogueRunner);
                    }
                }
            }
            else if (commandData.dialogueRunner == null)
            {
                // Display an error message if the dialogue runner is null
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Dialogue Runner is null. Please assign a Dialogue Runner to the Command Data.", MessageType.Error);
                EditorGUILayout.HelpBox("This can be assigned manually but using a Dialogue Runner Referencer is recommended to data the reference at Start.", MessageType.Error);
            }
        }

        private CommandData[] GetCommandData()
        {
            // Get all command data from the folder path
            return AssetDatabase.FindAssets("t:CommandData", null)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<CommandData>)
                .ToArray();
        }

        private bool CommandDataExists() => commandDatas != null && commandDatas.Length > 0;

        private bool HasAllCommandData() => commandDatas != null && commandDatas == GetCommandData();

        private bool HasActiveCommands(CommandData commandData)
        {
            // Check if all commands are active
            if (commandData.commands.Count > 0)
            {
                foreach (Command command in commandData.commands)
                {
                    if (command.CommandActive()) return true;
                }
            }

            // If no commands are active, return false
            return false;
        }

        private bool HasInactiveCommands(CommandData commandData)
        {
            // Check if all commands are inactive
            if (commandData.commands.Count > 0)
            {
                foreach (Command command in commandData.commands)
                {
                    if (!command.CommandActive()) return true;
                }
            }

            // If no commands are inactive, return false
            return false;
        }

        private bool HasCommands(CommandData commandData) => commandData.commands.Count > 0;

        /// <summary>
        /// Formats a command data name by inserting spaces before uppercase letters.
        /// </summary>
        /// <param name="commandDataName">The input string to format. Cannot be null.</param>
        /// <returns>A formatted string with spaces inserted before uppercase letters in the input string.</returns>
        private string FormatDataName(string commandDataName) => string.Concat(commandDataName.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
    }
}