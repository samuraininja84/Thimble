using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(YarnNodeDropdownAttribute))]
    public class YarnNodeDropdownDrawer : PropertyDrawer
    {
        private List<string> nodeNames = null;
        private List<string> nodeTags = null;
        private bool initialized = false;
        private bool showTags = false;
        private const string noneOption = "<None>";
        private const string windowTitle = "Select Yarn Variable";

        Regex TitleRegex => new Regex(@"title:\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Regex TagRegex => new Regex(@"tags:\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Initialize options only once
            if (!initialized)
            {
                // Get the attribute instance
                var attr = (YarnNodeDropdownAttribute)attribute;

                // Load node names from the specified folder path
                nodeNames = LoadNodeTitles(attr.FolderPath);

                // Load tags to ensure alignment with node names
                nodeTags = LoadTaggedNodes(nodeNames, LoadNodeTags(attr.FolderPath));

                // Ensure both lists have the same length
                initialized = true;
            }

            // Begin property drawing
            EditorGUI.BeginProperty(position, label, property);

            // Get the current value of the property
            string currentValue = property.stringValue;

            // Draw the label and get the rect for the button
            Rect buttonRect = EditorGUI.PrefixLabel(position, label);

            // Add space for a toggle button
            buttonRect.width -= 45;

            // Draw the button with the current value or "<None>" if empty
            if (GUI.Button(buttonRect, string.IsNullOrEmpty(currentValue) ? noneOption : currentValue))
            {
                // Get the mouse position in screen coordinates
                Vector2 mousePosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);

                // Determine whether to show tags or node names based on current value
                if (!showTags)
                {
                    // Open the search window with node names
                    mousePosition.OpenStringSearchWindow(windowTitle, nodeNames, selectedNode =>
                    {
                        property.stringValue = selectedNode;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                else
                {
                    // Open the search window with tags
                    mousePosition.OpenStringSearchWindow(windowTitle, nodeTags, selectedTag =>
                    {
                        // Find the index of the selected tag
                        int index = nodeTags.IndexOf(selectedTag);

                        // Get the corresponding node name
                        string correspondingNode = index >= 0 && index < nodeNames.Count ? nodeNames[index] : noneOption;

                        // Update the property value with the corresponding node name
                        property.stringValue = correspondingNode;

                        // Apply changes to the serialized object
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
            }

            // Get the rect for the toggle button
            Rect toggleRect = new Rect(buttonRect.xMax, buttonRect.y, 45, buttonRect.height);

            // Determine the text for the toggle button
            GUIContent toggleContent = new GUIContent(showTags ? "Tags" : "Names", showTags ? "Showing Tags" : "Showing Node Names");

            // Define a custom style for the toggle button
            GUIStyle toggleStyle = new GUIStyle(GUI.skin.button)
            {
                // Set a smaller font size for the toggle button
                fontSize = 10,

                // Center the text within the button
                alignment = TextAnchor.MiddleCenter,

                // Remove padding for a more compact button
                padding = new RectOffset(0, 0, 0, 0)
            };

            // Draw the toggle button to switch between node names and tags
            showTags = GUI.Toggle(toggleRect, showTags, toggleContent, toggleStyle);

            // End the property drawing
            EditorGUI.EndProperty();
        }

        private List<string> LoadNodeTitles(string folderPath)
        {
            // Get all .yarn files in the specified folder and its subdirectories
            string[] files = Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories);

            // Use a HashSet to avoid duplicate titles
            HashSet<string> nodeSet = new HashSet<string>();

            // Iterate through each file to extract titles
            foreach (var file in files)
            {
                // Read all lines from the file
                var lines = File.ReadAllLines(file);

                // Iterate through each line to find titles
                foreach (var line in lines)
                {
                    // Match the title line
                    var match = TitleRegex.Match(line);

                    // If a title is found, process it
                    if (match.Success)
                    {
                        // Extract the file name without extension and the title
                        string fileName = Path.GetFileNameWithoutExtension(file);

                        // Parse the title from the match
                        string parsedTitle = match.Groups[1].Value.Trim();

                        // Format the title with the file name for the dropdown
                        string formattedTitle = $"{fileName}/{parsedTitle}";

                        // Add the formatted title to the set
                        nodeSet.Add(formattedTitle);
                    }
                }
            }

            // Convert the set to a sorted list
            var sorted = nodeSet.ToList();

            // Add the none option at the start
            sorted.Insert(0, noneOption);

            // Ensure the titles list matches the tags list in length
            return sorted;
        }

        /// <summary>
        /// Loads and returns a list of tags extracted from all Yarn files in the specified folder and its subdirectories.
        /// </summary>
        /// <remarks>This method reads all Yarn files (*.yarn) in the specified folder and its subdirectories, extracting tags based on a predefined pattern. 
        /// Tags are added in the order they are encountered, and duplicates are preserved. 
        /// The resulting list always starts with a "none" option.
        /// </remarks>
        /// <param name="folderPath">The path to the folder containing Yarn files to process. The method searches recursively in all subdirectories.</param>
        /// <returns>A list of tags extracted from the Yarn files. The list includes an initial "none" option and may contain duplicates. If no tags are found, the list will still include the "none" option.</returns>
        private List<string> LoadNodeTags(string folderPath)
        {
            // Get all .yarn files in the specified folder and its subdirectories
            string[] files = Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories);

            // Use a list to maintain order and allow duplicates if necessary
            List<string> tagSet = new List<string>();

            // Iterate through each file to extract tags
            foreach (var file in files)
            {
                // Read all lines from the file
                var lines = File.ReadAllLines(file);

                // Iterate through each line to find tags
                foreach (var line in lines)
                {
                    // Match the tags line
                    var match = TagRegex.Match(line);

                    // Add the tag if found, otherwise add an empty string
                    if (match.Success) tagSet.Add(match.Groups[1].Value.Trim());
                }
            }

            // Add the none option at the start
            tagSet.Insert(0, noneOption);

            // Ensure the tags list matches the node names list in length
            return tagSet;
        }

        private List<string> LoadTaggedNodes(List<string> nodes, List<string> tags)
        {
            // For each node add its corresponding tag
            List<string> taggedNodes = new List<string>();

            // Iterate through each node
            for (int i = 0; i < nodes.Count; i++)
            {
                // Get the corresponding tag or none option
                string tag = i < tags.Count ? tags[i] : noneOption;

                // Add the node title to the tagged node after removing any path prefixes
                string nodeTitle = ParseNodeName(nodes[i]);

                // Format the tagged node
                string formattedNode = $"{tag}/{nodeTitle}";

                // Add to the list
                taggedNodes.Add(formattedNode);
            }

            // Return the list of tagged nodes
            return taggedNodes;
        }

        /// <summary>
        /// Extracts and trims the node name from the given title string.
        /// </summary>
        /// <param name="title">The input string containing the title. If the string contains a colon (:), the substring after the colon is
        /// extracted and trimmed; otherwise, the entire string is trimmed.</param>
        /// <returns>A string representing the extracted and trimmed node name. If the input string is empty or whitespace, the
        /// result will also be an empty or whitespace string.</returns>
        private string ParseNodeName(string title) => title.Contains("/") ? title.Substring(title.IndexOf('/') + 1).Trim() : title.Trim();
    }
}