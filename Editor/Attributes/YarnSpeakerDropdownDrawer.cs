using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(YarnSpeakerDropdownAttribute))]
    public class YarnSpeakerDropdownDrawer : PropertyDrawer
    {
        private List<string> speakers;
        private bool initialized = false;
        private const string noneOption = "<None>";
        private const string windowTitle = "Select Yarn Speaker";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Initialize options only once
            if (!initialized)
            {
                // Get the attribute instance
                var attr = (YarnSpeakerDropdownAttribute)attribute;

                // Load speakers from the specified folder path
                speakers = LoadSpeakers(attr.FolderPath);

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
                mousePosition.OpenStringSearchWindow(windowTitle, speakers, selectedSpeaker =>
                {
                    // Update the property value with the selected speaker
                    property.stringValue = selectedSpeaker;

                    // Apply the modified properties to ensure the change is saved
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            // End property drawing
            EditorGUI.EndProperty();
        }

        private List<string> LoadSpeakers(string folderPath)
        {
            // Regex to match lines that define a speaker (e.g., "SpeakerName:")
            var speakerRegex = new Regex(@"^\s*(\w+):", RegexOptions.Compiled);

            // Common keywords to exclude from speaker names
            var excluded = new HashSet<string> { "title", "position", "tags", "color", "set", "declare", "if", "endif", "stop" };

            // Use a HashSet to avoid duplicate speaker names
            HashSet<string> speakerSet = new HashSet<string>();

            // Iterate through all .yarn files in the specified folder and its subdirectories
            foreach (var file in Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories))
            {
                // Read all lines from the file
                var lines = File.ReadAllLines(file);

                // Iterate through each line in the file
                foreach (var line in lines)
                {
                    // Match lines that define a speaker
                    var match = speakerRegex.Match(line);

                    // If a match is found
                    if (match.Success)
                    {
                        // Extract the speaker name
                        string candidate = match.Groups[1].Value.Trim();

                        // Exclude common keywords
                        if (!excluded.Contains(candidate.ToLower())) speakerSet.Add(candidate);
                    }
                }
            }

            // Sort the speakers alphabetically
            var sorted = speakerSet.OrderBy(n => n).ToList();

            // Add the "<None>" option at the beginning
            sorted.Insert(0, "<None>");

            // Return the sorted list of speakers
            return sorted;
        }

    }
}