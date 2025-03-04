using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(NodePointer))]
    public class NodePointerPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get value properties
            var linkNameProperty = property.FindPropertyRelative("linkName");
            var linkIndexProperty = property.FindPropertyRelative("linkIndex");
            var linkNamesProperty = property.FindPropertyRelative("linkNames");
            var storyFileProperty = property.FindPropertyRelative("storyFile");

            // Begin change check
            EditorGUI.BeginChangeCheck();

            // Begin property
            EditorGUI.BeginProperty(position, label, property);

            // Draw label and get position
            Rect labelPosition = position;
            position = EditorGUI.PrefixLabel(labelPosition, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Draw link name and index properties side by side
            Rect linkNameRect = new Rect(position.x, position.y, position.width, position.height);

            // Draw a dropdown for all the nodes in the story file
            int chosenIndexValue = linkIndexProperty.intValue;
            List<string> listNames = new List<string>();

            // Draw the story file property field, if it exists, otherwise draw a text field with the none value
            if (linkNamesProperty != null && listNames.Count >= 0)
            {
                for (int i = 0; i < linkNamesProperty.arraySize; i++)
                {
                    listNames.Add(linkNamesProperty.GetArrayElementAtIndex(i).stringValue);
                }

                // If the list is empty, set the index to 0
                if (chosenIndexValue >= listNames.Count) chosenIndexValue = listNames.Count;

                if (listNames.Count == 0)
                {
                    // Draw a text field for the link name
                    chosenIndexValue = 0;
                    listNames = new List<string>{ "None" };
                    chosenIndexValue = EditorGUI.Popup(linkNameRect, chosenIndexValue, listNames.ToArray());
                }
                else
                {
                    // Draw the dropdown and set the chosen index value
                    chosenIndexValue = EditorGUI.Popup(linkNameRect, chosenIndexValue, listNames.ToArray());
                    linkNameProperty.stringValue = listNames[chosenIndexValue];
                    linkIndexProperty.intValue = chosenIndexValue;
                }
            }
            else
            {
                // Draw a text field for the link name
                chosenIndexValue = 0;
                listNames = new List<string>{ "None" };
                chosenIndexValue = EditorGUI.Popup(linkNameRect, chosenIndexValue, listNames.ToArray());
            }

            // Check if changes were made
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            // Reset indent level
            EditorGUI.indentLevel = indent;

            // End property
            EditorGUI.EndProperty();
        }
    }
}