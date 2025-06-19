using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(YarnNodeDropdownAttribute))]
public class YarnNodeDropdownDrawer : PropertyDrawer
{
    private List<string> nodeNames = null;
    private List<string> nodeTags = null;
    private bool initialized = false;

    Regex TitleRegex => new Regex(@"title:\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    Regex TagRegex => new Regex(@"tags:\s*(.+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            var attr = (YarnNodeDropdownAttribute)attribute;
            nodeNames = LoadNodeTitles(attr.FolderPath);
            nodeTags = LoadNodeTags(attr.FolderPath);
            initialized = true;
        }

        EditorGUI.BeginProperty(position, label, property);

        string currentValue = property.stringValue;
        int index = Mathf.Max(0, nodeNames.IndexOf(currentValue));

        Rect buttonRect = EditorGUI.PrefixLabel(position, label);
        if (GUI.Button(buttonRect, string.IsNullOrEmpty(currentValue) ? "<None>" : currentValue))
        {
            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));
            SearchableDropdown.Show(new Rect(screenPos, position.size), nodeNames, nodeTags, index, (newIndex) =>
            {
                property.stringValue = nodeNames[newIndex] == "<None>" ? "" : ParseNodeName(nodeNames[newIndex]);
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        EditorGUI.EndProperty();
    }

    private List<string> LoadNodeTitles(string folderPath)
    {
        string[] files = Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories);

        HashSet<string> nodeSet = new HashSet<string>();

        foreach (var file in files)
        {
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                var match = TitleRegex.Match(line);
                if (match.Success)
                {
                    // Extract the file name without extension and the title
                    string fileName = Path.GetFileNameWithoutExtension(file);

                    // Parse the title from the match
                    string parsedTitle = match.Groups[1].Value.Trim();

                    // Format the title with the file name for the dropdown
                    string formattedTitle = $"{fileName}: {parsedTitle}";

                    // Add the formatted title to the set
                    nodeSet.Add(formattedTitle);
                }
            }
        }

        var sorted = nodeSet.ToList();
        sorted.Insert(0, "<None>");
        return sorted;
    }

    private List<string> LoadNodeTags(string folderPath)
    {
        string[] files = Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories);

        List<string> tagSet = new List<string>();

        foreach (var file in files)
        {
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                var match = TagRegex.Match(line);
                if (match.Success)
                {
                    tagSet.Add(match.Groups[1].Value.Trim());
                }
            }
        }

        var sorted = tagSet.ToList();
        sorted.Insert(0, "None");
        return sorted;
    }

    /// <summary>
    /// Extracts and trims the node name from the given title string.
    /// </summary>
    /// <param name="title">The input string containing the title. If the string contains a colon (:), the substring after the colon is
    /// extracted and trimmed; otherwise, the entire string is trimmed.</param>
    /// <returns>A string representing the extracted and trimmed node name. If the input string is empty or whitespace, the
    /// result will also be an empty or whitespace string.</returns>
    private string ParseNodeName(string title) => title.Contains(":") ? title.Substring(title.IndexOf(':') + 1).Trim() : title.Trim();
}
