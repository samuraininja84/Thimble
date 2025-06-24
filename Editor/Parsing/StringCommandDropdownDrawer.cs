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
        private List<string> options;
        private bool initialized = false;

        private static readonly HashSet<string> excludedCommands = new HashSet<string>
        {
            "set", "declare", "endif", "if", "stop", "command"
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!initialized)
            {
                var attr = (YarnCommandDropdownAttribute)attribute;
                options = LoadCommands(attr.FolderPath);
                initialized = true;
            }

            EditorGUI.BeginProperty(position, label, property);

            string currentValue = property.stringValue;
            int index = Mathf.Max(0, options.IndexOf(currentValue));

            Rect buttonRect = EditorGUI.PrefixLabel(position, label);
            if (GUI.Button(buttonRect, string.IsNullOrEmpty(currentValue) ? "<None>" : currentValue))
            {
                Vector2 screenPos = GUIUtility.GUIToScreenPoint(new Vector2(position.x, position.y));
                SearchableDropdown.Show(new Rect(screenPos, position.size), options, index, (newIndex) =>
                {
                    property.stringValue = options[newIndex] == "<None>" ? "" : options[newIndex];
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            EditorGUI.EndProperty();
        }

        private List<string> LoadCommands(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath, "*.yarn", SearchOption.AllDirectories);
            var commandRegex = new Regex(@"<<\s*(\w+)", RegexOptions.Compiled);

            HashSet<string> commandSet = new HashSet<string>();

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    foreach (Match match in commandRegex.Matches(line))
                    {
                        string cmd = match.Groups[1].Value;
                        if (!excludedCommands.Contains(cmd))
                            commandSet.Add(cmd);
                    }
                }
            }

            var sorted = commandSet.OrderBy(c => c).ToList();
            sorted.Insert(0, "<None>"); // Add null option
            return sorted;
        }
    }
}