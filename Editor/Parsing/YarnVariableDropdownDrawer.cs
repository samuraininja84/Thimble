using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(YarnVariableDropdownAttribute))]
    public class YarnVariableDropdownDrawer : PropertyDrawer
    {
        private List<string> options;
        private bool initialized = false;

        /// <summary>
        /// Represents a compiled regular expression used to match variable patterns in the format "<<declare $variable =".
        /// This regex captures variable names that start with a dollar sign ($) and consist of alphanumeric characters or underscores.
        /// </summary>
        private readonly Regex VariableRegex = new Regex(@"<<declare\s+\$([a-zA-Z_][a-zA-Z0-9_]*)\s*=", RegexOptions.Compiled);

        /// <summary>
        /// Represents a compiled regular expression used to match variable patterns in the format "<<declare $variable = value>>".
        /// This regex captures variable values after the = sign, allowing for extraction of variable values from Yarn files.
        /// </summary>
        private readonly Regex TypeRegex = new Regex(@"<<declare\s+\$[a-zA-Z_][a-zA-Z0-9_]*\s*=\s*(.*?)>>", RegexOptions.Compiled);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if the options have been initialized; if not, load them from the specified folder path
            if (!initialized)
            {
                var attr = (YarnVariableDropdownAttribute)attribute;
                options = LoadVariables(attr.FolderPath);
                initialized = true;
            }

            // Begin the property GUI with the given position, label, and property
            EditorGUI.BeginProperty(position, label, property);

            // Get the current value of the property and find its index in the options list
            string currentValue = property.stringValue;
            int index = Mathf.Max(0, options.IndexOf(currentValue));

            // Draw the dropdown button with the current value or "<None>" if empty
            Rect buttonRect = EditorGUI.PrefixLabel(position, label);
            if (GUI.Button(buttonRect, string.IsNullOrEmpty(currentValue) ? "<None>" : currentValue))
            {
                Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));
                SearchableDropdown.Show(new Rect(screenPos, position.size), options, index, (newIndex) =>
                {
                    property.stringValue = options[newIndex] == "<None>" ? "" : GetVariableName(options[newIndex]);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            // End the property GUI
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Loads and retrieves a sorted list of unique variables from Yarn files within the specified folder and its
        /// subdirectories.
        /// </summary>
        /// <remarks>This method scans all Yarn files in the specified folder and its subdirectories,
        /// extracts variables using a predefined pattern, and associates each variable with its type. Duplicate
        /// variables are removed, and the resulting list is sorted alphabetically.</remarks>
        /// <param name="folderPath">The path to the folder containing Yarn files to be processed. This folder and its subdirectories will be
        /// searched for files with a ".yarn" extension.</param>
        /// <returns>A sorted list of unique variables found in the Yarn files, including their types. The list begins with a
        /// "<None>" entry.</returns>
        private List<string> LoadVariables(string folderPath)
        {
            // Get all Yarn files in the specified folder and its subdirectories
            string[] files = Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories);

            // Create a HashSet to store unique variable names
            HashSet<string> variableSet = new HashSet<string>();

            // Iterate through each file and read its lines
            foreach (var file in files)
            {
                // Read all lines from the file
                var lines = File.ReadAllLines(file);

                // Match each line against the variable regex
                foreach (var line in lines)
                {
                    // Find all matches of the variable pattern in the line
                    foreach (Match match in VariableRegex.Matches(line))
                    {
                        // Extract the variable name from the match
                        string var = match.Groups[1].Value;

                        // If the variable name is empty, skip it
                        string type = GetVariableType(line);

                        // Get the variable with its type
                        string varWithType = $"{var} {type}";

                        // If the variable is in the included variables set, add it to the variable set
                        variableSet.Add(varWithType);
                    }
                }
            }

            // Convert the variable set to a sorted list
            var sorted = variableSet.OrderBy(c => c).ToList();

            // Insert a "<None>" option at the beginning of the list
            sorted.Insert(0, "<None>");

            // Return the sorted list of variables
            return sorted;
        }

        /// <summary>
        /// Extracts the variable name from a string that includes a type annotation and ensures it is prefixed with a dollar sign ($).
        /// </summary>
        /// <remarks>
        /// This method assumes that the type annotation, if present, is separated from the variable name by a space.  
        /// If no type annotation is found, the input string is treated as the variable name.
        /// </remarks>
        /// <param name="variable">A string containing a variable name followed by a type annotation in the format "variableName (type)".</param>
        /// <returns>
        /// A string representing the variable name, prefixed with a dollar sign ($). 
        /// </returns>
        private string GetVariableName(string variable)
        {
            // Remove the appended type from the line by removing in the format " (type)"
            int typeIndex = variable.LastIndexOf(' ');
            if (typeIndex > 0)
            {
                // Extract the variable name by taking the substring before the type
                variable = variable.Substring(0, typeIndex);
            }

            // Append the dollar sign ($) to the variable name if it is not already present
            if (!variable.StartsWith("$")) variable = "$" + variable;

            // If no type is found, return the whole line as the variable name
            return variable;
        }

        /// <summary>
        /// Determines the type of a variable based on the provided line of text.
        /// </summary>
        /// <remarks>The method uses regular expressions to extract the variable type from the input line.
        /// It performs additional checks to classify the type as a string, float, or boolean.</remarks>
        /// <param name="line">The line of text to analyze for a variable type. This should contain a variable declaration or value.</param>
        /// <returns>A string representing the variable type in a standardized format: <list type="bullet"> <item>
        /// <description><c>(string)</c> if the variable type is identified as a string.</description> </item> <item>
        /// <description><c>(float)</c> if the variable type is identified as a numeric value.</description> </item>
        /// <item> <description><c>(bool)</c> if the variable type is identified as a boolean value.</description>
        /// </item> <item> <description>An empty string if the type cannot be determined.</description> </item> </list></returns>
        private string GetVariableType(string line)
        {
            // Match the line against the type regex to extract the variable type
            var match = TypeRegex.Match(line);
            if (match.Success)
            {
                // Get the type from the match groups
                string type = match.Groups[1].Value;

                // If the variable type has "" characters, remove them and return (string)
                if (type.StartsWith("\"") && type.EndsWith("\"")) return "(string)";

                // If the variable type is a number, return it as (float)
                if (float.TryParse(type, out _)) return "(float)";

                // If the variable type is a boolean, return it as (bool)
                if (bool.TryParse(type, out _)) return "(bool)";
            }

            // If no match is found, return an empty string
            return string.Empty;
        }
    }
}