using System.Linq;
using UnityEditor;
using UnityEngine;
using Yarn.Unity;

namespace Thimble.Editor
{
    public class VariableDataEditorWindow : EditorWindow
    {
        public VariableData[] variableData;

        public string stringInput = string.Empty;
        public float floatInput = 0f;
        public bool boolInput = false;

        private bool showVariableValues = false;
        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Tools/Thimble/Variable Verification")]
        public static void ShowWindow()
        {
            GetWindow<VariableDataEditorWindow>("Variable Verification");
        }

        private void OnGUI()
        {
            // Get all variable data from the folder path if the variable data is null or if the variable dats is not equal to the variable data from the folder path
            if (!HasAllVariableData()) variableData = GetVariableData();

            // Get the height of the window
            float windowHeight = position.height;

            // Add a scroll view to the window to display all variable datas when there are too many to fit on the screen
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(windowHeight));

            // Check that the variable data is not null before drawing them
            if (VariableDataExists())
            {
                // Display the variable data label field
                DrawHeader("Variable Data");

                // Display all variable datas with their respective variable storage, variables, and variable tools
                for (int i = 0; i < variableData.Length; i++)
                {
                    EditorGUILayout.LabelField("Variable Data: " + FormatDataName(variableData[i].name), EditorStyles.boldLabel);
                    DrawVariableData(variableData[i]);
                    EditorGUILayout.Space();

                    // Add a line to separate the variable datas
                    if (i < variableData.Length - 1) EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    // Add a space between the variable datas
                    EditorGUILayout.Space();
                }

                // Display the variable values label field
                DrawHeader("Variable Values");

                // Create a foldout to display the input fields for the variable values
                showVariableValues = EditorGUILayout.BeginFoldoutHeaderGroup(showVariableValues, "Input Fields: ");
                if (showVariableValues)
                {
                    // Display the input fields for the variable values
                    stringInput = EditorGUILayout.TextField("New String Value", stringInput);
                    floatInput = EditorGUILayout.FloatField("New Float Value", floatInput);
                    boolInput = EditorGUILayout.Toggle("New Bool Value", boolInput);
                    EditorGUILayout.Space();

                    // Display a help box to inform the user how to use the input fields
                    EditorGUILayout.HelpBox("Input the new values for the variables you want to change in these fields before pressing the set value next to a variable.", MessageType.Info);
                    EditorGUILayout.Space();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            else if (!VariableDataExists())
            {
                // Display the variable data label field
                DrawHeader("Variable Data");

                // Display an error message if no variable datas are found in the folder path
                EditorGUILayout.HelpBox("No Variable Data is found in the folder path. Please create a Variable Data in the folder path.", MessageType.Error);
            }

            // End the scroll view
            EditorGUILayout.EndScrollView();
        }

        private void ResetValues()
        {
            // Reset the input fields for the variable values
            stringInput = string.Empty;
            floatInput = 0f;
            boolInput = false;
        }

        private void DrawHeader(string headerText)
        {
            // Display the variable data label field in the middle of the window
            Rect labelPosition = GUILayoutUtility.GetRect(position.width, EditorGUIUtility.singleLineHeight);
            labelPosition.x = labelPosition.width / 2 - 50;
            EditorGUI.LabelField(labelPosition, headerText, EditorStyles.boldLabel);

            // Add a line to separate the variable data label field from the variable data
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void DrawVariableData(VariableData variableData, InMemoryVariableStorage variableStorage = null)
        {
            // Display the variable data field
            EditorGUILayout.LabelField("Variable Fields", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            variableData = (VariableData)EditorGUILayout.ObjectField("Variable Data", variableData, typeof(VariableData), false);
            EditorGUI.EndDisabledGroup();

            SerializedObject variableDataObject = new SerializedObject(variableData);
            variableDataObject.Update();

            // Check if the variable data has a variable storage before displaying the variable storage field, any existing variables, and the variable tools
            if (variableData.variableStorage != null)
            {
                // Get the variable storage from the variable data
                if (variableStorage == null) variableStorage = variableData.variableStorage;

                // Display the variable storage field if the variable data is not null
                EditorGUI.BeginDisabledGroup(true);
                variableStorage = (InMemoryVariableStorage)EditorGUILayout.ObjectField("Variable Storage", variableStorage, typeof(InMemoryVariableStorage), true);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();

                // Display all string variables with a bold label if there are any string variables
                EditorGUILayout.LabelField("String Variables", EditorStyles.boldLabel);
                if (!HasStringVariables(variableData))
                {
                    EditorGUILayout.LabelField("No string variables found", EditorStyles.miniLabel);
                }
                else if (HasStringVariables(variableData))
                {
                    for (int i = 0; i < variableData.stringVariables.Count; i++)
                    {
                        // Get the variable from the variable data and display it if it is active
                        Variable variable = variableData.stringVariables[i];

                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(variable.Name + ": " + variable.StringValue);

                        // Open a window to set the variable value
                        if (GUILayout.Button("Set Value"))
                        {
                            variableData.SetValue(variableStorage, variable.Name, stringInput);
                            ResetValues();
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Display all float variables with a bold label if there are any float variables
                EditorGUILayout.LabelField("Float Variables", EditorStyles.boldLabel);
                if (!HasFloatVariables(variableData))
                {
                    EditorGUILayout.LabelField("No float variables found", EditorStyles.miniLabel);
                }
                else if (HasFloatVariables(variableData))
                {
                    for (int i = 0; i < variableData.floatVariables.Count; i++)
                    {
                        // Get the variable from the variable data and display it if it is active
                        Variable variable = variableData.floatVariables[i];

                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(variable.Name + ": " + variable.FloatValue.ToString());

                        // Add a button to set the variable value
                        if (GUILayout.Button("Set Value"))
                        {
                            variableData.SetValue(variableStorage, variable.Name, floatInput);
                            ResetValues();
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Display all bool variables with a bold label if there are any bool variables
                EditorGUILayout.LabelField("Bool Variables", EditorStyles.boldLabel);
                if (!HasBoolVariables(variableData))
                {
                    EditorGUILayout.LabelField("No bool variables found", EditorStyles.miniLabel);
                }
                else if (HasBoolVariables(variableData))
                {
                    for (int i = 0; i < variableData.boolVariables.Count; i++)
                    {
                        // Get the variable from the variable data and display it if it is active
                        Variable variable = variableData.boolVariables[i];

                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(variable.Name + ": " + variable.BoolValue.ToString());

                        // Add a button to set the variable value
                        if (GUILayout.Button("Set Value"))
                        {
                            variableData.SetValue(variableStorage, variable.Name, boolInput);
                            ResetValues();
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                }

                // Display variable tools header with a bold label
                EditorGUILayout.LabelField("Variable Tools", EditorStyles.boldLabel);

                // Display the update variables and clear all variables buttons if there are any variables
                if (HasVariables(variableData))
                {
                    // Get all variables from the variable data if they are not equal to the variable storage
                    if (!HasAllVariables(variableStorage, variableData))
                    {
                        variableData.GetVariables(variableStorage);
                    }

                    // If the variables are not equal to the variable storage, display the update variables button
                    if (!Equal(variableStorage, variableData))
                    {
                        if (GUILayout.Button("Update Variables"))
                        {
                            variableData.UpdateVariables(variableStorage);
                        }
                    }

                    // Display the clear all variables button
                    if (GUILayout.Button("Clear All Variables"))
                    {
                        variableData.ClearAllVariables(variableStorage);
                    }
                }
                else if (!HasVariables(variableData))
                {
                    // Display an error message if no variables are found
                    EditorGUILayout.LabelField("No variables found", EditorStyles.miniLabel);

                    // Display the get variables button if there are no variables
                    if (GUILayout.Button("Get Variables"))
                    {
                        variableData.GetVariables(variableStorage);
                    }
                }
            }
            else if (variableData.variableStorage == null)
            {
                // Display an error message if the variable storage is null
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Variable Storage is null. Please assign a Variable Storage to the Variable Data.", MessageType.Error);
                EditorGUILayout.HelpBox("This can be assigned manually but using a Variable Storage Referencer is recommended to data the reference at Start.", MessageType.Error);
            }

            variableDataObject.ApplyModifiedProperties();
        }

        private VariableData[] GetVariableData()
        {
            // Get all variable data from the folder path
            return AssetDatabase.FindAssets("t:VariableData", null)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<VariableData>)
                .ToArray();
        }

        private bool VariableDataExists()
        {
            // Check if variable data is not null and has a length greater than 0
            return variableData != null && variableData.Length > 0;
        }

        private bool HasAllVariableData()
        {
            // Check if the variable data is not null and is equal to the variable data from the folder path
            return variableData != null && variableData == GetVariableData();
        }

        private bool HasStringVariables(VariableData variableData)
        {
            // Check if the variable data has active variables
            return variableData.stringVariables.Count > 0;
        }

        private bool HasFloatVariables(VariableData variableData)
        {
            // Check if the variable data has active variables
            return variableData.floatVariables.Count > 0;
        }

        private bool HasBoolVariables(VariableData variableData)
        {
            // Check if the variable data has active variables
            return variableData.boolVariables.Count > 0;
        }

        private bool HasVariables(VariableData variableData)
        {
            // Check if the variable data has variables
            return HasStringVariables(variableData) || HasFloatVariables(variableData) || HasBoolVariables(variableData);
        }

        private bool HasAllVariables(InMemoryVariableStorage storage, VariableData variableData)
        {
            // Check if the variables are equal to the variable storage
            return variableData.Equal(storage);
        }

        private bool Equal(InMemoryVariableStorage storage, VariableData variableData)
        {
            // Check if the variables are equal to the variable storage
            return variableData.Equal(storage);
        }

        private string FormatDataName(string variableDataName)
        {
            // Add spaces after each capital letter in the variable data name
            return string.Concat(variableDataName.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}
