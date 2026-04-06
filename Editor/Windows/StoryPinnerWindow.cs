using System.Linq;
using UnityEngine;
using UnityEditor;
using Yarn.Unity;

namespace Thimble.Editor
{
    public class StoryPinnerWindow : EditorWindow
    {
        public CommandData[] commandDatas;
        public FunctionData[] functionDatas;
        public VariableData variableData => VariableData.Instance;

        public string stringInput = string.Empty;
        public float floatInput = 0f;
        public bool boolInput = false;
        private bool showVariableValues = true;

        private static string[] sectionNames = new string[] { "Variables", "Commands", "Functions" };
        private int selectedSectionIndex = 0;

        private Vector2 scrollPosition = Vector2.zero;

        private const string windowTitle = "Story Pinner";
        private const string iconPath = IconPathExtensions.IconPath + "StoryPinner.png";

        [MenuItem("Window/Thimble/" + windowTitle)]
        public static void Open()
        {
            // Get the window instance
            StoryPinnerWindow window = GetWindow<StoryPinnerWindow>(windowTitle);

            // Get the icon for the window
            Texture icon = EditorGUIUtility.FindTexture(iconPath);

            // Set the title icon for the window
            window.titleContent = new GUIContent(windowTitle, icon);
        }

        private void OnEnable()
        {
            commandDatas = GetCommandData();
            functionDatas = GetFunctionData();
        }

        private void OnDisable() => ResetValues();

        private void OnGUI()
        {
            // Get the height of the window
            float windowHeight = position.height;

            // Add a scroll view to the window to display all command data when there are too many to fit on the screen
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(windowHeight));

            // Add a space at the top of the window
            EditorGUILayout.Space();

            // Draw the selected section
            DrawSection(selectedSectionIndex);

            // Move the toolbar to the bottom of the window
            GUILayout.FlexibleSpace();

            // Add a header above the toolbar
            EditorGUILayout.LabelField("Select Section:", EditorStyles.boldLabel);

            // Draw the toolbar to select the section
            selectedSectionIndex = GUILayout.Toolbar(selectedSectionIndex, sectionNames);

            // Add space after the toolbar
            EditorGUILayout.Space();

            // End the scroll view
            EditorGUILayout.EndScrollView();
        }

        private void DrawSection(int sectionIndex)
        {
            // Draw the selected section based on the section index
            switch (sectionIndex)
            {
                // Draw the command data section
                case 0:
                    DrawVariableDataSection();
                    break;
                // Draw the function data section
                case 1:
                    DrawCommandDataSection();
                    break;
                // Draw the variable data section
                case 2:
                    DrawFunctionDataSection();
                    break;
                // Default case to handle invalid section index
                default:
                    Debug.LogError("Invalid section index: " + sectionIndex);
                    break;
            }
        }

        private void DrawHeader(string headerText, Color color) => EditorGUILayout.LabelField(headerText, HeaderStyle(color));

        #region Variable Data Section

        private void DrawVariableDataSection()
        {
            // Display the variable data label field
            DrawHeader("Variable Data", Color.cyan);

            // Check that the variable data is not null before drawing them
            if (VariableDataExists())
            {
                // Display the variable data with its respective variable storage, variables, and variable tools
                DrawVariableData(variableData);
            }
            else if (!VariableDataExists())
            {
                // Display an error message if no variable datas are found in the project
                EditorGUILayout.HelpBox("No Variable Data has been found in the project.", MessageType.Error);
            }

            // End the foldout for the variable values
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawVariableData(VariableData variableData, VariableStorageBehaviour storage = null)
        {
            // Create a label for the variable data name
            EditorGUILayout.LabelField("Variable Data: " + FormatDataName(variableData.name), EditorStyles.boldLabel);

            // Begin the disabled group for the variable data field
            EditorGUI.BeginDisabledGroup(true);

            // Display the variable data field
            variableData = (VariableData)EditorGUILayout.ObjectField("Variable Data", variableData, typeof(VariableData), false);

            // End the disabled group for the variable data field
            EditorGUI.EndDisabledGroup();

            // Create a serialized object for the variable data
            SerializedObject variableDataObject = new SerializedObject(variableData);

            // Update the serialized object
            variableDataObject.Update();

            // Add space after the variable storage field
            EditorGUILayout.Space();

            // Display the variable data based on if there are any variables in the variable data or not
            if (variableData.Empty())
            {
                // Display an error message if no variables are found
                EditorGUILayout.LabelField("No variables found", EditorStyles.miniLabel);
            }
            else
            {
                // Display all string variables with a bold label if there are any string variables
                EditorGUILayout.LabelField("String Variables", EditorStyles.boldLabel);

                // Check if there are any string variables
                if (!HasStringVariables(variableData))
                {
                    // Display a message if no string variables are found
                    EditorGUILayout.LabelField("No string variables found", EditorStyles.miniLabel);
                }
                else if (HasStringVariables(variableData))
                {
                    // Loop through all string variables in the variable data
                    for (int i = 0; i < variableData.stringVariables.Count; i++)
                    {
                        // Get the variable from the variable data and display it if it is active
                        var variable = variableData.stringVariables.ElementAt(i);

                        // Start the horizontal layout
                        EditorGUILayout.BeginHorizontal();

                        // Display the variable name and value
                        string currentValue = EditorGUILayout.TextField(FormatVariableName(variable.Key), variable.Value);

                        // Set the variable value if it has been changed in the input field
                        if (!currentValue.Equals(variable.Value, System.StringComparison.Ordinal)) VariableData.Instance.SetValue(variable.Key, currentValue);

                        // End the horizontal layouts
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Display all float variables with a bold label if there are any float variables
                EditorGUILayout.LabelField("Float Variables", EditorStyles.boldLabel);

                // Check if there are any float variables
                if (!HasFloatVariables(variableData))
                {
                    // Display a message if no float variables are found
                    EditorGUILayout.LabelField("No float variables found", EditorStyles.miniLabel);
                }
                else if (HasFloatVariables(variableData))
                {
                    // Loop through all float variables in the variable data
                    for (int i = 0; i < variableData.floatVariables.Count; i++)
                    {
                        // Get the variable from the variable data and display it if it is active
                        var variable = variableData.floatVariables.ElementAt(i);

                        // Start the horizontal layout
                        EditorGUILayout.BeginHorizontal();

                        // Display the variable name and value
                        float currentValue = EditorGUILayout.FloatField(FormatVariableName(variable.Key), variable.Value);

                        // Set the variable value if it has been changed in the input field
                        if (currentValue != variable.Value) VariableData.Instance.SetValue(variable.Key, currentValue);

                        // End the horizontal layouts
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Display all bool variables with a bold label if there are any bool variables
                EditorGUILayout.LabelField("Bool Variables", EditorStyles.boldLabel);

                // Check if there are any bool variables
                if (!HasBoolVariables(variableData))
                {
                    // Display a message if no bool variables are found
                    EditorGUILayout.LabelField("No bool variables found", EditorStyles.miniLabel);
                }
                else if (HasBoolVariables(variableData))
                {
                    // Loop through all bool variables in the variable data
                    for (int i = 0; i < variableData.boolVariables.Count; i++)
                    {
                        // Get the variable from the variable data and display it if it is active
                        var variable = variableData.boolVariables.ElementAt(i);

                        // Start the horizontal layout
                        EditorGUILayout.BeginHorizontal();

                        // Display the variable name and value
                        EditorGUILayout.LabelField(FormatVariableName(variable.Key));

                        // Push the value to the right of the variable name
                        GUILayout.FlexibleSpace();

                        // Display the variable name and value
                        bool currentValue = EditorGUILayout.Toggle(GUIContent.none, variable.Value);

                        // Set the variable value if it has been changed in the input field
                        if (currentValue != variable.Value) VariableData.Instance.SetValue(variable.Key, currentValue);

                        // End the horizontal layouts
                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Display a warning if the variable storage is null
                if (variableData.storage == null)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.HelpBox("Variable Storage is null. Variables will not be updated or refreshed in the Yarn Project.", MessageType.None);
                    EditorGUILayout.HelpBox("This can be assigned manually but using a Variable Storage Referencer is recommended to set the data in the reference at Start.", MessageType.None);
                    EditorGUILayout.Separator();
                }
            }

            // Display variable tools header with a bold label
            EditorGUILayout.LabelField("Variable Tools", EditorStyles.boldLabel);

            // Display the initialize variables button
            if (GUILayout.Button("Initialize Variables")) variableData.Initialize();

            // Display the get variables button if we are missing any variables in the variable data compared to the variable storage
            if (GUILayout.Button("Update From Storage")) variableData.RefreshVariables();

            // Display the update variables button
            if (GUILayout.Button("Update From Data")) variableData.UpdateVariables();

            // Display the clear all variables button
            if (GUILayout.Button("Clear Variables")) variableData.Clear();

            // Apply any changes to the serialized object
            variableDataObject.ApplyModifiedProperties();
        }

        private bool HasStringVariables(VariableData variableData) => variableData.stringVariables.Count > 0;

        private bool HasFloatVariables(VariableData variableData) => variableData.floatVariables.Count > 0;

        private bool HasBoolVariables(VariableData variableData) => variableData.boolVariables.Count > 0;

        private bool VariableDataExists() => VariableData.Instance != null;

        private string FormatVariableName(string variableName)
        {
            // If the variable name is null or empty, return string.Empty
            if (string.IsNullOrEmpty(variableName)) return string.Empty;

            // If the variable is a Yarn Internal variable, trim the "Yarn.Internal" prefix from the variable name and return the formatted variable name
            if (variableName.Contains(VariableHandler.InternalVariableDenotator, System.StringComparison.Ordinal))
            {
                // Remove the "Yarn.Internal" prefix from the variable name
                return variableName.Replace(VariableHandler.InternalVariableDenotator + ".", string.Empty);
            }

            // Return the variable name if it is not a Yarn Internal variable
            return variableName;
        }

        /// <summary>
        /// Formats a variable name by inserting spaces before uppercase letters.
        /// </summary>
        /// <param name="variableDataName">The name of the variable to format. Cannot be null.</param>
        /// <returns>A formatted string with spaces inserted before uppercase letters in <paramref name="variableDataName"/>.</returns>
        private string FormatDataName(string variableDataName) => string.Concat(variableDataName.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

        private void ResetValues()
        {
            // Reset the input fields for the variable values
            stringInput = string.Empty;
            floatInput = 0f;
            boolInput = false;
        }

        #endregion

        #region Command Data Section

        private void DrawCommandDataSection()
        {
            // Display the command data label field
            DrawHeader("Command Data", Color.orangeRed);

            // Check that the command data is not null before drawing them
            if (CommandDataExists())
            {
                // Display all command datas with their respective dialogue runner, commands, and command tools
                for (int i = 0; i < commandDatas.Length; i++)
                {
                    // Display the command data name as a bold label
                    EditorGUILayout.LabelField("Command Data: " + FormatCommandName(commandDatas[i].name), EditorStyles.boldLabel);

                    // Draw the command data
                    DrawCommandData(commandDatas[i]);

                    // Add a space between the command data and the next command data
                    EditorGUILayout.Space();

                    // Add a line to separate the command datas
                    if (i < commandDatas.Length - 1) EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    // Add a space between the command datas
                    EditorGUILayout.Space();
                }
            }
            else if (!CommandDataExists())
            {
                // Display an error message if no command datas are found in the folder path
                EditorGUILayout.HelpBox("No Command Data is found in the folder path. Please create a Command Data in the folder path.", MessageType.Error);
            }

            // Draw a button to refresh the command data
            if (GUILayout.Button("Refresh Command Data")) commandDatas = GetCommandData();
        }

        private void DrawCommandData(CommandData commandData = null)
        {
            // Begin the disabled group for the command data field
            EditorGUI.BeginDisabledGroup(true);

            // Display the command data field
            commandData = (CommandData)EditorGUILayout.ObjectField("Command Data", commandData, typeof(CommandData), false);

            // End the disabled group for the command data field
            EditorGUI.EndDisabledGroup();

            // Check if the command data has a dialogue runner before displaying the dialogue runner field, any existing commands, and the command tools
            if (commandData.HasRunners())
            {
                // Add space after the dialogue runner field
                EditorGUILayout.Space();

                // Display all active commands with a bold label if there are any active commands
                EditorGUILayout.LabelField("Active Commands", EditorStyles.boldLabel);

                // Check if there are any active commands
                if (!HasActiveCommands(commandData))
                {
                    // Display a message if no active commands are found
                    EditorGUILayout.LabelField("No active commands found", EditorStyles.miniLabel);
                }
                else if (HasActiveCommands(commandData))
                {
                    // Loop through all commands in the command data
                    for (int i = 0; i < commandData.commands.Count; i++)
                    {
                        // Get the command from the command data and display it if it is active
                        Command command = commandData.commands[i];

                        // Check if the command is active
                        if (command.CommandActive())
                        {
                            // Start the horizontal layout
                            EditorGUILayout.BeginHorizontal();

                            // Display the command name
                            EditorGUILayout.LabelField(command.commandName);

                            // Add a button to display the command's information
                            if (GUILayout.Button("Info")) EditorUtility.DisplayDialog(command.CommandTierToString() + " Command: " + command.commandName, command.GetCommandInfo(), "OK");

                            // Add a button to deactivate the command if it is active
                            if (GUILayout.Button("Deactivate")) commandData.DeactivateCommand(command);

                            // Add a button to remove the command
                            if (GUILayout.Button("Remove")) commandData.RemoveCommand(command);

                            // End the horizontal layout
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display all inactive commands with a bold label if there are any inactive commands
                EditorGUILayout.LabelField("Inactive Commands", EditorStyles.boldLabel);

                // Check if there are any inactive commands
                if (!HasInactiveCommands(commandData))
                {
                    // Display a message if no inactive commands are found
                    EditorGUILayout.LabelField("No inactive commands found", EditorStyles.miniLabel);
                }
                else if (HasInactiveCommands(commandData))
                {
                    // Loop through all commands in the command data
                    for (int i = 0; i < commandData.commands.Count; i++)
                    {
                        // Get the command from the command data and display it if it is inactive
                        Command command = commandData.commands[i];

                        // Check if the command is inactive
                        if (!command.CommandActive())
                        {
                            // Start the horizontal layout
                            EditorGUILayout.BeginHorizontal();

                            // Display the command name
                            EditorGUILayout.LabelField(command.commandName);

                            // Add a button to display the command's information
                            if (GUILayout.Button("Info")) EditorUtility.DisplayDialog(command.CommandTierToString() + " Command: " + command.commandName, command.GetCommandInfo(), "OK");

                            // Add a button to activate the command if it is inactive
                            if (GUILayout.Button("Activate")) commandData.ActivateCommand(command);

                            // Add a button to remove the command
                            if (GUILayout.Button("Remove")) commandData.RemoveCommand(command);

                            // End the horizontal layout
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display the command tools if there are any commands
                if (HasCommands(commandData))
                {
                    // Display the command tools with a bold label
                    EditorGUILayout.LabelField("Command Tools", EditorStyles.boldLabel);

                    // Display buttons to activate, deactivate, and clear all commands
                    if (GUILayout.Button("Activate All Commands")) commandData.ActivateAllCommands();

                    // Display a button to deactivate all commands
                    if (GUILayout.Button("Deactivate All Commands")) commandData.DeactivateAllCommands();

                    // Display a button to clear all commands
                    if (GUILayout.Button("Clear All Commands")) commandData.ClearAllCommands();
                }
            }
            else
            {
                // Display an error message if the dialogue runner is null
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Dialogue Runner is null. Please assign a Dialogue Runner to the Command Data.", MessageType.Error);
                EditorGUILayout.HelpBox("This can be assigned manually but using a Dialogue Runner Referencer is recommended to set the data in the reference at Start.", MessageType.Error);
            }
        }

        private bool HasActiveCommands(CommandData commandData) => commandData.commands.Exists(command => command.CommandActive());

        private bool HasInactiveCommands(CommandData commandData) => commandData.commands.Exists(command => !command.CommandActive());

        private bool CommandDataExists() => commandDatas != null && commandDatas.Length > 0;

        private bool HasCommands(CommandData commandData) => commandData.commands.Count > 0;

        /// <summary>
        /// Formats a command data name by inserting spaces before uppercase letters.
        /// </summary>
        /// <param name="commandDataName">The input string to format. Cannot be null.</param>
        /// <returns>A formatted string with spaces inserted before uppercase letters in the input string.</returns>
        private string FormatCommandName(string commandDataName) => string.Concat(commandDataName.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

        private CommandData[] GetCommandData()
        {
            // Get all command data from the folder path
            return AssetDatabase.FindAssets("t:CommandData", null)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<CommandData>)
                .ToArray();
        }

        #endregion

        #region Function Data Section

        private void DrawFunctionDataSection()
        {
            // Display the function data label field
            DrawHeader("Function Data", Color.green);

            // Check that the function data is not null before drawing them
            if (FunctionDataExists())
            {
                // Display all function datas with their respective dialogue runner, functions, and function tools
                for (int i = 0; i < functionDatas.Length; i++)
                {
                    // Display the function data name as a bold label
                    EditorGUILayout.LabelField("Function Data: " + FormatFunctionName(functionDatas[i].name), EditorStyles.boldLabel);

                    // Draw the function data
                    DrawFunctionData(functionDatas[i]);

                    // Add a space after each function data
                    EditorGUILayout.Space();

                    // Add a line to separate the function datas
                    if (i < functionDatas.Length - 1) EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    // Add a space between the function datas
                    EditorGUILayout.Space();
                }
            }
            else if (!FunctionDataExists())
            {
                // Display an error message if no function datas are found in the folder path
                EditorGUILayout.HelpBox("No Function Data is found in the folder path. Please create a Function Data in the folder path.", MessageType.Error);
            }

            // Display a button to refresh the function data
            if (GUILayout.Button("Refresh Function Data")) functionDatas = GetFunctionData();
        }

        private void DrawFunctionData(FunctionData functionData = null)
        {
            // Begin the disabled group for the function data field
            EditorGUI.BeginDisabledGroup(true);

            // Display the function data field
            functionData = (FunctionData)EditorGUILayout.ObjectField("Function Data", functionData, typeof(FunctionData), false);

            // End the disabled group for the function data field
            EditorGUI.EndDisabledGroup();

            // Check if the function data has a dialogue runner before displaying the dialogue runner field, any existing functions, and the function tools
            if (functionData.HasRunners())
            {
                // Display all active functions with a bold label if there are any active functions
                EditorGUILayout.LabelField("Active Functions", EditorStyles.boldLabel);

                // Check if there are any active functions
                if (!HasActiveFunctions(functionData))
                {
                    // Display a message if no active functions are found
                    EditorGUILayout.LabelField("No active functions found", EditorStyles.miniLabel);
                }
                else if (HasActiveFunctions(functionData))
                {
                    // Loop through all functions in the function data
                    for (int i = 0; i < functionData.functions.Count; i++)
                    {
                        // Get the function from the function data and display it if it is active
                        Function function = functionData.functions[i];

                        // Check if the function is active
                        if (function.FunctionActive())
                        {
                            // Start the horizontal layout
                            EditorGUILayout.BeginHorizontal();

                            // Display the function name
                            EditorGUILayout.LabelField(function.functionName);

                            // Add a button to display the function's information
                            if (GUILayout.Button("Info")) EditorUtility.DisplayDialog(function.FunctionTierToString() + " Function: " + function.functionName, function.GetFunctionInfo(), "OK");

                            // Add a button to deactivate the function if it is active
                            if (GUILayout.Button("Deactivate")) functionData.DeactivateFunction(function);

                            // Add a button to remove the function
                            if (GUILayout.Button("Remove")) functionData.RemoveFunction(function);

                            // End the horizontal layout
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display all inactive functions with a bold label if there are any inactive functions
                EditorGUILayout.LabelField("Inactive Functions", EditorStyles.boldLabel);

                // Check if there are any inactive functions
                if (!HasInactiveFunctions(functionData))
                {
                    // Display a message if no inactive functions are found
                    EditorGUILayout.LabelField("No inactive functions found", EditorStyles.miniLabel);
                }
                else if (HasInactiveFunctions(functionData))
                {
                    // Loop through all functions in the function data
                    for (int i = 0; i < functionData.functions.Count; i++)
                    {
                        // Get the function from the function data and display it if it is inactive
                        Function function = functionData.functions[i];

                        // Check if the function is inactive
                        if (!function.FunctionActive())
                        {
                            // Start the horizontal layout
                            EditorGUILayout.BeginHorizontal();

                            // Display the function name
                            EditorGUILayout.LabelField(function.functionName);

                            // Add a button to display the function's information
                            if (GUILayout.Button("Info")) EditorUtility.DisplayDialog(function.FunctionTierToString() + " Function: " + function.functionName, function.GetFunctionInfo(), "OK");

                            // Add a button to activate the function if it is inactive
                            if (GUILayout.Button("Activate")) functionData.ActivateFunction(function);

                            // Add a button to remove the function
                            if (GUILayout.Button("Remove")) functionData.RemoveFunction(function);

                            // End the horizontal layout
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display the function tools if there are any functions
                if (HasFunctions(functionData))
                {
                    // Display the function tools with a bold label
                    EditorGUILayout.LabelField("Function Tools", EditorStyles.boldLabel);

                    // Display a button to activate all functions
                    if (GUILayout.Button("Activate All Functions")) functionData.ActivateAllFunctions();

                    // Display a button to deactivate all functions
                    if (GUILayout.Button("Deactivate All Functions")) functionData.DeactivateAllFunctions();

                    // Display a button to clear all functions
                    if (GUILayout.Button("Clear All Functions")) functionData.ClearAllFunctions();
                }
            }
            else
            {
                // Display an error message if the dialogue runner is null
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Dialogue Runner is null. Please assign a Dialogue Runner to the Function Data.", MessageType.Error);
                EditorGUILayout.HelpBox("This can be assigned manually but using a Dialogue Runner Referencer is recommended to set the data in the reference at Start.", MessageType.Error);
            }
        }

        private bool HasActiveFunctions(FunctionData functionData) => functionData.functions.Exists(function => function.FunctionActive());

        private bool HasInactiveFunctions(FunctionData functionData) => functionData.functions.Exists(function => !function.FunctionActive());

        private bool FunctionDataExists() => functionDatas != null && functionDatas.Length > 0;

        private bool HasFunctions(FunctionData functionData) => functionData.functions.Count > 0;

        /// <summary>
        /// Formats a given data name by inserting spaces before uppercase letters.
        /// </summary>
        /// <param name="functionDataName">The input string to format. Typically represents a camel-cased or Pascal-cased name.</param>
        /// <returns>A formatted string where spaces are inserted before uppercase letters in the input.</returns>
        private string FormatFunctionName(string functionDataName) => string.Concat(functionDataName.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

        private FunctionData[] GetFunctionData()
        {
            // Get all function data from the folder path
            return AssetDatabase.FindAssets("t:FunctionData", null)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<FunctionData>)
                .ToArray();
        }

        #endregion

        private GUIStyle HeaderStyle(Color textColor)
        {
            // Create a new GUIStyle for the header
            return new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(-5, -5, -5, -5),
                normal = { textColor = textColor }
            };
        }
    }
}