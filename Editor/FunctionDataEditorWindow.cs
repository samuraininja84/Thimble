using System.Linq;
using UnityEngine;
using UnityEditor;
using Yarn.Unity;

namespace Thimble.Editor
{
    public class FunctionDataEditorWindow : EditorWindow
    {
        public FunctionData[] functionDatas;

        private Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Tools/Thimble/Function Finder")]
        public static void ShowWindow()
        {
            GetWindow<FunctionDataEditorWindow>("Function Finder");
        }

        private void OnGUI()
        {
            // Get all function data from the folder path if the function data is null or if the function dats is not equal to the function data from the folder path
            if (!HasAllFunctionData()) functionDatas = GetFunctionData();

            // Get the height of the window
            float windowHeight = position.height;

            // Add a scroll view to the window to display all function datas when there are too many to fit on the screen
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(windowHeight));

            // Check that the function data is not null before drawing them
            if (FunctionDataExists())
            {
                // Display the function data label field
                DrawHeader("Function Data");

                // Display all function datas with their respective dialogue runner, functions, and function tools
                for (int i = 0; i < functionDatas.Length; i++)
                {
                    EditorGUILayout.LabelField("Function Data: " + FormatDataName(functionDatas[i].name), EditorStyles.boldLabel);
                    DrawFunctionData(functionDatas[i]);
                    EditorGUILayout.Space();

                    // Add a line to separate the function datas
                    if (i < functionDatas.Length - 1) EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    // Add a space between the function datas
                    EditorGUILayout.Space();
                }
            }
            else if (!FunctionDataExists())
            {
                // Display the function data label field
                DrawHeader("Function Data");

                // Display an error message if no function datas are found in the folder path
                EditorGUILayout.HelpBox("No Function Data is found in the folder path. Please create a Function Data in the folder path.", MessageType.Error);
            }

            // End the scroll view
            EditorGUILayout.EndScrollView();
        }

        private void DrawHeader(string headerText)
        {
            // Display the function data label field in the middle of the window
            Rect labelPosition = GUILayoutUtility.GetRect(position.width, EditorGUIUtility.singleLineHeight);
            labelPosition.x = labelPosition.width / 2 - 50;
            EditorGUI.LabelField(labelPosition, headerText, EditorStyles.boldLabel);

            // Add a line to separate the function data label field from the function data
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void DrawFunctionData(FunctionData functionData = null, DialogueRunner dialogueRunner = null)
        {
            // Display the function data field
            EditorGUILayout.LabelField("Function Fields", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            functionData = (FunctionData)EditorGUILayout.ObjectField("Function Data", functionData, typeof(FunctionData), false);
            EditorGUI.EndDisabledGroup();

            // Check if the function data has a dialogue runner before displaying the dialogue runner field, any existing functions, and the function tools
            if (functionData.dialogueRunner != null)
            {
                // Get the dialogue runner from the function data
                if (dialogueRunner == null) dialogueRunner = functionData.dialogueRunner;

                // Display the dialogue runner field if the function data is not null
                EditorGUI.BeginDisabledGroup(true);
                dialogueRunner = (DialogueRunner)EditorGUILayout.ObjectField("Dialogue Runner", dialogueRunner, typeof(DialogueRunner), true);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.Space();

                // Display all active functions with a bold label if there are any active functions
                EditorGUILayout.LabelField("Active Functions", EditorStyles.boldLabel);
                if (!HasActiveFunctions(functionData))
                {
                    EditorGUILayout.LabelField("No active functions found", EditorStyles.miniLabel);
                }
                else if (HasActiveFunctions(functionData))
                {
                    for (int i = 0; i < functionData.functions.Count; i++)
                    {
                        // Get the function from the function data and display it if it is active
                        Function function = functionData.functions[i];
                        if (function.FunctionActive())
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(function.functionName);

                            // Add a button to display the function's information
                            if (GUILayout.Button("Info"))
                            {
                                EditorUtility.DisplayDialog(function.FunctionTierToString() + " Function: " + function.functionName, function.GetFunctionInfo(), "OK");
                            }

                            // Add a button to deactivate the function if it is active
                            if (GUILayout.Button("Deactivate"))
                            {
                                functionData.DeactivateFunction(dialogueRunner, function);
                            }

                            // Add a button to remove the function
                            if (GUILayout.Button("Remove"))
                            {
                                functionData.RemoveFunction(dialogueRunner, function);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display all inactive functions with a bold label if there are any inactive functions
                EditorGUILayout.LabelField("Inactive Functions", EditorStyles.boldLabel);
                if (!HasInactiveFunctions(functionData))
                {
                    EditorGUILayout.LabelField("No inactive functions found", EditorStyles.miniLabel);
                }
                else if (HasInactiveFunctions(functionData))
                {
                    for (int i = 0; i < functionData.functions.Count; i++)
                    {
                        // Get the function from the function data and display it if it is inactive
                        Function function = functionData.functions[i];
                        if (!function.FunctionActive())
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(function.functionName);

                            // Add a button to display the function's information
                            if (GUILayout.Button("Info"))
                            {
                                EditorUtility.DisplayDialog(function.FunctionTierToString() + " Function: " + function.functionName, function.GetFunctionInfo(), "OK");
                            }

                            // Add a button to activate the function if it is inactive
                            if (GUILayout.Button("Activate") && !function.FunctionActive())
                            {
                                functionData.ActivateFunction(dialogueRunner, function);
                            }

                            // Add a button to remove the function
                            if (GUILayout.Button("Remove"))
                            {
                                functionData.RemoveFunction(dialogueRunner, function);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }

                // Display the function tools if there are any functions
                if (HasFunctions(functionData))
                {
                    // Display the function tools with a bold label
                    EditorGUILayout.LabelField("Function Tools", EditorStyles.boldLabel);
                    if (GUILayout.Button("Activate All Functions"))
                    {
                        functionData.ActivateAllFunctions(dialogueRunner);
                    }
                    if (GUILayout.Button("Deactivate All Functions"))
                    {
                        functionData.DeactivateAllFunctions(dialogueRunner);
                    }
                    if (GUILayout.Button("Clear All Functions"))
                    {
                        functionData.ClearAllFunctions(dialogueRunner);
                    }
                }
            }
            else if (functionData.dialogueRunner == null)
            {
                // Display an error message if the dialogue runner is null
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Dialogue Runner is null. Please assign a Dialogue Runner to the Function Data.", MessageType.Error);
                EditorGUILayout.HelpBox("This can be assigned manually but using a Dialogue Runner Referencer is recommended to data the reference at Start.", MessageType.Error);
            }
        }

        private FunctionData[] GetFunctionData()
        {
            // Get all function data from the folder path
            return AssetDatabase.FindAssets("t:FunctionData", null)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<FunctionData>)
                .ToArray();
        }

        private bool FunctionDataExists()
        {
            // Check if function data is not null and has a length greater than 0
            return functionDatas != null && functionDatas.Length > 0;
        }

        private bool HasAllFunctionData()
        {
            // Check if the function data is not null and is equal to the function data from the folder path
            return functionDatas != null && functionDatas == GetFunctionData();
        }

        private bool HasActiveFunctions(FunctionData functionData)
        {
            // Check if all functions are active
            if (functionData.functions.Count > 0)
            {
                foreach (Function function in functionData.functions)
                {
                    if (function.FunctionActive()) return true;
                }
            }
            return false;
        }

        private bool HasInactiveFunctions(FunctionData functionData)
        {
            // Check if all functions are inactive
            if (functionData.functions.Count > 0)
            {
                foreach (Function function in functionData.functions)
                {
                    if (function.FunctionActive()) return true;
                }
            }
            return false;
        }

        private bool HasFunctions(FunctionData functionData)
        {
            // Check if the function data has functions
            return functionData.functions.Count > 0;
        }

        private string FormatDataName(string functionDataName)
        {
            // Add spaces after each capital letter in the function data name
            return string.Concat(functionDataName.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        }
    }
}