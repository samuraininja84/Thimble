using UnityEditor;
using UnityEngine;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(Variable))]
    public class VariablePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Begin change check
            EditorGUI.BeginChangeCheck();

            // Get prefix property and name property
            var nameProperty = property.FindPropertyRelative("Name");

            // Get value properties
            var floatValueProperty = property.FindPropertyRelative("FloatValue");
            var stringValueProperty = property.FindPropertyRelative("StringValue");
            var boolValueProperty = property.FindPropertyRelative("BoolValue");

            // Get use value properties
            var useFloatProperty = property.FindPropertyRelative("UseFloat");
            var useStringProperty = property.FindPropertyRelative("UseString");
            var useBoolProperty = property.FindPropertyRelative("UseBool");

            // Get shown property
            var shownProperty = null as SerializedProperty;
            if (useFloatProperty.boolValue == true) shownProperty = floatValueProperty;
            if (useStringProperty.boolValue == true) shownProperty = stringValueProperty;
            if (useBoolProperty.boolValue == true) shownProperty = boolValueProperty;
            if (useFloatProperty.boolValue == false && useStringProperty.boolValue == false && useBoolProperty.boolValue == false)
            {
                shownProperty = stringValueProperty;
                useStringProperty.boolValue = true;
            }

            // Begin property
            EditorGUI.BeginProperty(position, label, property);
            position.width -= 24f;

            // Draw label and get position
            Rect labelPosition = position;
            position = EditorGUI.PrefixLabel(labelPosition, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate the divider based on the use value properties
            float divider = 2f;
            if (useBoolProperty.boolValue == true) divider = 1.065f;

            // Calculate the width and offset for the name and value properties
            float width = position.width / divider;
            float prefixWidth = 15f;
            float offset = 36f;
            float gap = 3f;

            // Draw name property and show value property side by side
            Rect prefixSymbolRect = new Rect(position.x, position.y, (position.width + prefixWidth) - position.width, position.height);
            Rect nameRect = new Rect(position.x + offset / 2f, position.y, width - offset / 2, position.height);
            Rect valueRect = new Rect(position.x + width + gap, position.y, width, position.height);

            // Get the prefix for variables
            string prefix = Variable.Prefix;

            // Draw the prefix symbol for the variable
            GUI.enabled = false;
            EditorGUI.TextField(prefixSymbolRect, prefix);
            GUI.enabled = true;

            // Draw name and value properties
            EditorGUI.PropertyField(nameRect, nameProperty, GUIContent.none);
            EditorGUI.PropertyField(valueRect, shownProperty, GUIContent.none);

            // Offset the position for the dropdown button
            position.x += position.width + 24;
            position.width = position.height = EditorGUI.GetPropertyHeight(shownProperty);
            position.x -= position.width;

            // Get the button style for the useConstantProperty
            GUIStyle buttonStyle = EditorStyles.miniButton;
            buttonStyle.padding = new RectOffset(1, 1, 1, 1);

            // Get the content for the buttons
            GUIContent settingsContent = EditorGUIUtility.IconContent("d_Settings");

            // Draw the dropdown button to select the variable type
            if (EditorGUI.DropdownButton(position, settingsContent, FocusType.Passive, buttonStyle))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Use String"), useStringProperty.boolValue, () =>
                {
                    useStringProperty.boolValue = true;
                    useFloatProperty.boolValue = false;
                    useBoolProperty.boolValue = false;
                    property.serializedObject.ApplyModifiedProperties();
                });
                menu.AddItem(new GUIContent("Use Float"), useFloatProperty.boolValue, () =>
                {
                    useStringProperty.boolValue = false;
                    useFloatProperty.boolValue = true;
                    useBoolProperty.boolValue = false;
                    property.serializedObject.ApplyModifiedProperties();
                });
                menu.AddItem(new GUIContent("Use Bool"), useBoolProperty.boolValue, () =>
                {
                    useStringProperty.boolValue = false;
                    useFloatProperty.boolValue = false;
                    useBoolProperty.boolValue = true;
                    property.serializedObject.ApplyModifiedProperties();
                });
                menu.ShowAsContext();
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
