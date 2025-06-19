using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[CustomPropertyDrawer(typeof(YarnSpeakerDropdownAttribute))]
public class YarnSpeakerDropdownDrawer : PropertyDrawer
{
    private List<string> speakers;
    private bool initialized = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            var attr = (YarnSpeakerDropdownAttribute)attribute;
            speakers = LoadSpeakers(attr.FolderPath);
            initialized = true;
        }

        EditorGUI.BeginProperty(position, label, property);

        string currentValue = property.stringValue;
        int index = Mathf.Max(0, speakers.IndexOf(currentValue));

        Rect buttonRect = EditorGUI.PrefixLabel(position, label);
        if (GUI.Button(buttonRect, string.IsNullOrEmpty(currentValue) ? "<None>" : currentValue))
        {
            Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));
            SearchableDropdown.Show(new Rect(screenPos, position.size), speakers, index, (newIndex) =>
            {
                property.stringValue = speakers[newIndex] == "<None>" ? "" : speakers[newIndex];
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        EditorGUI.EndProperty();
    }

    private List<string> LoadSpeakers(string folderPath)
    {
        var speakerRegex = new Regex(@"^\s*(\w+):", RegexOptions.Compiled);
        var excluded = new HashSet<string> { "title", "position", "tags", "color", "set", "declare", "if", "endif", "stop" };

        HashSet<string> speakerSet = new HashSet<string>();

        foreach (var file in Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories))
        {
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                var match = speakerRegex.Match(line);
                if (match.Success)
                {
                    string candidate = match.Groups[1].Value.Trim();
                    if (!excluded.Contains(candidate.ToLower()))
                    {
                        speakerSet.Add(candidate);
                    }
                }
            }
        }

        var sorted = speakerSet.OrderBy(n => n).ToList();
        sorted.Insert(0, "<None>");
        return sorted;
    }

}
