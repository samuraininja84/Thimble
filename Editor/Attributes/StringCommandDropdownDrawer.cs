using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(YarnCommandDropdownAttribute))]
    public class StringCommandDropdownDrawer : PropertyDrawer
    {
        private List<string> commands;
        private bool initialized = false;
        private const string noneOption = "<None>";
        private const string windowTitle = "Select Yarn Command";

        /// <summary>
        /// Excluded commands that should not appear in the dropdown.
        /// </summary>
        private static readonly HashSet<string> excludedCommands = new HashSet<string>
        {
            "set", "declare", "endif", "if", "stop", "command"
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Initialize options only once
            if (!initialized)
            {
                // Get the attribute instance
                var attr = (YarnCommandDropdownAttribute)attribute;

                // Load commands from the specified folder path
                commands = LoadCommands(attr.FolderPath);

                // Mark as initialized to avoid reloading
                initialized = true;
            }

            // Begin property drawing
            EditorGUI.BeginProperty(position, label, property);

            // Get the current value of the property
            string currentValue = property.stringValue;

            // Draw the label and get the rect for the button
            Rect buttonRect = EditorGUI.PrefixLabel(position, label);

            // Draw the button with the current value or "<None>" if empty
            if (GUI.Button(buttonRect, string.IsNullOrEmpty(currentValue) ? noneOption : currentValue))
            {
                // Get the mouse position in screen coordinates
                Vector2 mousePosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

                // Open the search window at the button's screen position
                mousePosition.OpenStringSearchWindow(windowTitle, commands, selectedCommand =>
                {
                    // Update the property value with the selected command
                    property.stringValue = selectedCommand;

                    // Apply changes to the serialized object
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            // Handle the case where the current value is not in the options
            EditorGUI.EndProperty();
        }

        private List<string> LoadCommands(string folderPath)
        {
            // Get all .yarn files in the specified folder and its subdirectories
            string[] files = Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories);

            // Regex to match commands in the format << commandName
            var commandRegex = new Regex(@"<<\s*(\w+)", RegexOptions.Compiled);

            // Use a HashSet to store unique command names
            HashSet<string> commandSet = new HashSet<string>();

            // Iterate through each file found in the specified folder
            foreach (var file in files)
            {
                // Read all lines from the file
                var lines = File.ReadAllLines(file);

                // Iterate through each line in the file
                foreach (var line in lines)
                {
                    // Find all matches of the command pattern in the line
                    foreach (Match match in commandRegex.Matches(line))
                    {
                        // Extract the command name
                        string cmd = match.Groups[1].Value;

                        // Exclude certain commands
                        if (!excludedCommands.Contains(cmd)) commandSet.Add(cmd);
                    }
                }
            }

            // Sort the commands alphabetically
            var sorted = commandSet.OrderBy(c => c).ToList();

            // Add the "<None>" option at the top
            sorted.Insert(0, noneOption);

            // Return the sorted list of commands
            return sorted;
        }
    }
}