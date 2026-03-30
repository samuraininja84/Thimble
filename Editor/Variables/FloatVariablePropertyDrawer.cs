using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(FloatVariable))]
    public class FloatVariablePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Begin change check
            EditorGUI.BeginChangeCheck();

            // Get the name property and set it to lowercase to match the serialized field name
            var nameProperty = property.FindPropertyRelative(nameof(FloatVariable.Name).ToLower());

            // Get the value property and set it to lowercase to match the serialized field name
            var valueProperty = property.FindPropertyRelative(nameof(FloatVariable.Value).ToLower());

            // Begin property
            EditorGUI.BeginProperty(position, label, property);

            // Draw the label and adjust the position for the name and value fields
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Gap between name and value fields
            float gap = 3f;

            // Calculate the width for the name and value properties
            Rect nameRect = new Rect(position.x, position.y, position.width / 2 - gap, position.height);

            // Adjust the value rect to accommodate the smaller field for bool
            Rect valueRect = new Rect(position.x + position.width / 2 + gap, position.y, position.width / 2 - gap, position.height);

            // Draw name and value properties
            EditorVariableExtensions.DrawVariableSelector(nameRect, nameProperty, valueProperty, VariableType.Float);

            // Get the current name value to determine if the value field should be enabled
            string name = nameProperty.stringValue;

            // Disable the value field if the name isn't empty to prevent editing the value with a valid name
            GUI.enabled = string.IsNullOrEmpty(name) || string.Equals(name, VariableHandler.MissingVariableName, System.StringComparison.OrdinalIgnoreCase);
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
            GUI.enabled = true;

            // Check if changes were made
            if (EditorGUI.EndChangeCheck())
            {
                // Apply modified properties to save changes
                property.serializedObject.ApplyModifiedProperties();
            }

            // End property
            EditorGUI.EndProperty();
        }
    }
}
