using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    public static class EditorVariableExtensions
    {
        public static void DrawVariableSelector(Rect position, SerializedProperty nameProperty, SerializedProperty valueProperty, VariableType type, bool allowMultiSelect = false)
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
                if (position.width < size.x) label.tooltip = label.text;
            }

            // Define a method to set the name property based on the selected variable name and the current name property value
            string SetName(string name, string selection)
            {
                // If the name is not null or empty, set the name property to the selected variable name, with a comma prefix if multi-select is allowed
                if (!string.IsNullOrEmpty(selection))
                {
                    // If the name is equal to the missing variable name constant, set the name to it directly; otherwise, set the name with a comma prefix if multi-select is allowed
                    if (selection == VariableHandler.MissingVariableName) return VariableHandler.MissingVariableName;

                    // If the name is equal to the missing variable name constant, set the name to an empty string; otherwise, keep the name as is
                    if (name == VariableHandler.MissingVariableName) name = string.Empty;

                    // If allowMultiSelect is true, create a hash set of the current variable names in the name property value, split by commas, to check for duplicates
                    if (allowMultiSelect)
                    {
                        // Create a hash set to store unique variable names, ignoring case sensitivity
                        var uniqueNames = new HashSet<string>(name.Split(", ", StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);

                        // Trim the selection to remove any leading or trailing whitespace
                        var trimmedSelection = selection.Trim();

                        // Add each name to the hash set after trimming whitespace to ensure uniqueness
                        if (!uniqueNames.Contains(trimmedSelection)) uniqueNames.Add(trimmedSelection);
                        else uniqueNames.Remove(trimmedSelection);

                        // Join the unique variable names back into a single string with a comma separator
                        var result = string.Join(", ", uniqueNames);

                        // If the name is null or empty, return the selection directly; otherwise, append the selection to the name with a comma separator
                        return !string.IsNullOrEmpty(result) ? result : VariableHandler.MissingVariableName;
                    }
                    else
                    {
                        // If the name is equal to the selection, ignoring case sensitivity, return the missing variable name constant; otherwise, return the selection directly   
                        return string.Equals(name, selection, StringComparison.OrdinalIgnoreCase) ? VariableHandler.MissingVariableName : selection;
                    }
                }

                // If the selection is null or empty, return the name as is
                return name;
            }

            // Define a method to apply the selected variable from the popup to the name and value properties
            void ApplySelection(string selection)
            {
                // Set the name property to the selected variable name when a selection is made in the popup
                nameProperty.stringValue = SetName(nameProperty.stringValue, selection);

                // If multi-select is allowed, handle the case where multiple variables are selected and set the value property accordingly
                if (allowMultiSelect)
                {
                    // Split the name property value into a list of variable names using comma as the delimiter and trim whitespace from each name
                    var names = new List<string>(nameProperty.stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

                    // If multi-select is not allowed, set the value property based on the selected variable's value from the VariableData instance
                    switch (type)
                    {
                        case VariableType.Float:
                            valueProperty.floatValue = VariableData.Instance.GetValueSum(names);
                            break;
                        case VariableType.String:
                            valueProperty.stringValue = VariableData.Instance.GetStringList(names);
                            break;
                        case VariableType.Bool:
                            valueProperty.boolValue = VariableData.Instance.GetConcatValue(names);
                            break;
                    }
                }
                else
                {
                    // If multi-select is not allowed, set the value property based on the selected variable's value from the VariableData instance
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
                }

                // Apply modified properties
                nameProperty.serializedObject.ApplyModifiedProperties();
            }

            // Draw the dropdown for the connection selection
            if (EditorGUI.DropdownButton(position, label, FocusType.Passive))
            {
                // Show the popup window with the variable database tree view for selection
                PopupWindow.Show(position, new VariableDatabaseTreePopup(new VariableDatabaseTreeView(ApplySelection, type))
                {
                    // Set the minimum width of the popup window to be at least 300 pixels or the width of the position, whichever is greater
                    Width = Mathf.Max(position.width, 300)
                });
            }
        }
    }
}
