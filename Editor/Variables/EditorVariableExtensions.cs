using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    public static class EditorVariableExtensions
    {
        public static void DrawVariableSelector(Rect position, SerializedProperty nameProperty, SerializedProperty valueProperty, VariableType type)
        {
            // Get the label
            var label = new GUIContent();

            // To be implemented: Draw a dropdown selector for existing variables of the same type in the project
            bool hasVariables = true;

            // Check if there are any variables of the specified type available for selection in the VariableData instance
            switch (type)
            {
                case VariableType.Float:
                    hasVariables = VariableData.Instance.floatVariables.Count > 0;
                    break;
                case VariableType.String:
                    hasVariables = VariableData.Instance.stringVariables.Count > 0;
                    break;
                case VariableType.Bool:
                    hasVariables = VariableData.Instance.boolVariables.Count > 0;
                    break;
            }

            // If there are no bool variables available for selection, set the label to "None" and the name property to "None"
            if (!hasVariables)
            {
                // Set the label to the null label to indicate that there are no variables available for selection
                label.text = VariableHandler.MissingVariableName.AppendYarnPrefix();

                // Set the name property to the missing variable name constant
                nameProperty.stringValue = VariableHandler.MissingVariableName;
            }
            else
            {
                // Set the label to the area name and the connection name at the current index
                label.text = nameProperty.stringValue;

                // Get the size for the label from the mini pull down style
                var size = EditorStyles.miniPullDown.CalcSize(label);

                // If the position width is less than the label size, set the tooltip to the full label text and shorten the label text to just the connection name for better display
                if (position.width < size.x)
                {
                    label.tooltip = label.text;
                    //label.text = nameProperty.stringValue;
                }
            }

            // Define a method to apply the selected variable from the popup to the name and value properties
            void ApplySelection(string selection)
            {
                // Set the name property to the selected variable name when a selection is made in the popup
                nameProperty.stringValue = selection;

                // Get the value of the selected variable from the VariableData instance and set the value property accordingly based on the variable type
                switch (type)
                {
                    case VariableType.Float:
                        VariableData.Instance.GetVariable(selection, out float floatValue);
                        valueProperty.floatValue = floatValue;
                        break;
                    case VariableType.String:
                        VariableData.Instance.GetVariable(selection, out string stringValue);
                        valueProperty.stringValue = stringValue;
                        break;
                    case VariableType.Bool:
                        VariableData.Instance.GetVariable(selection, out bool boolValue);
                        valueProperty.boolValue = boolValue;
                        break;
                }

                // Apply modified properties
                nameProperty.serializedObject.ApplyModifiedProperties();
            }

            // Draw the dropdown for the connection selection
            if (EditorGUI.DropdownButton(position, label, FocusType.Passive))
            {
                PopupWindow.Show
                (
                    position,
                    new DatabaseTreePopup(new DatabaseTreeView(null, ApplySelection, type))
                    {
                        Width = Mathf.Max(position.width, 300)
                    }
                );
            }
        }
    }
}
