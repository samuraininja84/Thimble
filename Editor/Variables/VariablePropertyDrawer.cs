using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    [CustomPropertyDrawer(typeof(Variable))]
    public class VariablePropertyDrawer : PropertyDrawer
    {
        private float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => height;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Begin change check
            EditorGUI.BeginChangeCheck();

            // Get type property and name property
            var typeProperty = property.FindPropertyRelative("Type");
            var nameProperty = property.FindPropertyRelative("Name");

            // Get value properties
            var floatValueProperty = property.FindPropertyRelative("floatValue");
            var stringValueProperty = property.FindPropertyRelative("stringValue");
            var boolValueProperty = property.FindPropertyRelative("boolValue");

            // Get shown property
            var shownProperty = null as SerializedProperty;
            switch ((VariableType)typeProperty.intValue)
            {
                case VariableType.Float:
                    shownProperty = floatValueProperty;
                    break;
                case VariableType.String:
                    shownProperty = stringValueProperty;
                    break;
                case VariableType.Bool:
                    shownProperty = boolValueProperty;
                    break;
            }

            // Begin property
            EditorGUI.BeginProperty(position, label, property);

            // Adjust position for label
            float gap = 3f;
            position.width -= gap;

            // Draw label and get position
            Rect labelPosition = position;
            position = EditorGUI.PrefixLabel(labelPosition, GUIUtility.GetControlID(FocusType.Passive), label);

            // Store the current indent level and set it to 0
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate the divider based on the use value properties
            float divider = 2f;
            if (boolValueProperty.boolValue) divider = 1.065f;

            // Calculate the width and offset for the name and value properties
            float width = position.width / divider;
            float prefixWidth = 15f;
            float offset = 36f;

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

            // Create a right-click context menu for the property
            bool hasClicked = Event.current.type == EventType.MouseUp  && Event.current.button == 1;
            if (hasClicked && position.Contains(Event.current.mousePosition))
            {
                // Create a context menu
                GenericMenu menu = new();

                // Add items to the context menu based on the current type of the variable
                menu.AddItem(new GUIContent("Use String"), IsType(typeProperty, VariableType.String), () =>
                {
                    typeProperty.enumValueIndex = (int)VariableType.String;
                    property.serializedObject.ApplyModifiedProperties();
                });
                menu.AddItem(new GUIContent("Use Float"), IsType(typeProperty, VariableType.Float), () =>
                {
                    typeProperty.enumValueIndex = (int)VariableType.Float;
                    property.serializedObject.ApplyModifiedProperties();
                });
                menu.AddItem(new GUIContent("Use Bool"), IsType(typeProperty, VariableType.Bool), () =>
                {
                    // Switch to bool type
                    typeProperty.enumValueIndex = (int)VariableType.Bool;
                    property.serializedObject.ApplyModifiedProperties();
                });

                // Show the context menu
                menu.ShowAsContext();

                // Consume the event to prevent it from propagating
                Event.current.Use();
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

        /// <summary>
        /// Determines whether the specified <see cref="SerializedProperty"/> matches the given <see cref="VariableType"/>.
        /// </summary>
        /// <param name="property">The serialized property to evaluate. Must not be <see langword="null"/>.</param>
        /// <param name="type">The variable type to compare against.</param>
        /// <returns><see langword="true"/> if the <paramref name="property"/> corresponds to the specified <paramref
        /// name="type"/>; otherwise, <see langword="false"/>.</returns>
        private bool IsType(SerializedProperty property, VariableType type) => property.enumValueIndex == (int)type;
    }
}
