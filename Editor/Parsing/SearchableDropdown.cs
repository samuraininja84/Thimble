using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Thimble.Editor
{
    public class SearchableDropdown : EditorWindow
    {
        private static Action<int> onSelect;
        private static List<string> options;
        private static List<string> tags;
        private static int selectedIndex;
        private bool nameFilterEnabled = true;

        private string search = "";
        private Vector2 scrollPosition;

        public static void Show(Rect activatorRect, List<string> list, int currentIndex, Action<int> callback)
        {
            options = list;
            selectedIndex = currentIndex;
            onSelect = callback;

            var window = CreateInstance<SearchableDropdown>();
            window.ShowAsDropDown(activatorRect, new Vector2(300, 300));
        }

        public static void Show(Rect activatorRect, List<string> optionList, List<string> tagList, int currentIndex, Action<int> callback)
        {
            options = optionList;
            tags = tagList;
            selectedIndex = currentIndex;
            onSelect = callback;

            var window = CreateInstance<SearchableDropdown>();
            window.ShowAsDropDown(activatorRect, new Vector2(300, 300));
        }

        private void OnGUI()
        {
            // Top row with search and close button
            GUILayout.BeginHorizontal();

            // Add a search symbol on the left side of the top row
            GUILayout.Label(EditorGUIUtility.IconContent("Search Icon"), GUILayout.Width(20), GUILayout.Height(20));

            // Add a label for the search field
            search = EditorGUILayout.TextField(search);

            // Add a toggle for filtering by name or tags if tags are provided
            if (tags == null || tags.Count == 0)
            {
                // Default to name filter if no tags are available
                nameFilterEnabled = true;
            }
            else if (tags.Count > 0)
            {
                // Define a style for the toggle
                GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButton);
                toggleStyle.padding = new RectOffset(1, 1, 1, 1);
                toggleStyle.alignment = TextAnchor.MiddleCenter;
                toggleStyle.fixedHeight = EditorGUIUtility.singleLineHeight;
                toggleStyle.fixedWidth = 45;

                // Determine the label and tooltip based on the filter type
                string filterLabel = nameFilterEnabled ? "Tag" : "Name";
                string fitlerTooltip = nameFilterEnabled ? "Filter by Tags" : "Filter by Name";
                GUIContent filterContent = new GUIContent(filterLabel, fitlerTooltip);

                // If tags are available, allow toggling between name and tag filters
                nameFilterEnabled = GUILayout.Toggle(nameFilterEnabled, filterContent, toggleStyle);
            }

            // Define a style for the close button as mini button
            GUIStyle closeButtonStyle = new GUIStyle(EditorStyles.miniButton);
            closeButtonStyle.padding = new RectOffset(1, 1, 1, 1);
            closeButtonStyle.fixedHeight = EditorGUIUtility.singleLineHeight;
            closeButtonStyle.fixedWidth = 25;

            // Add a close button on the right side of the top row
            if (GUILayout.Button(EditorGUIUtility.IconContent("CrossIcon"), closeButtonStyle))
            {
                Close();
                return;
            }

            // End the horizontal layout for the top row
            GUILayout.EndHorizontal();

            // Filter the options based on the search input
            List<string> filtered = nameFilterEnabled ? NameFilter(search) : TagFilter(search);

            // Define the style for the options
            GUIStyle optionStyle = new GUIStyle(EditorStyles.miniButton);
            optionStyle.padding = new RectOffset(5, 5, 5, 5);
            optionStyle.alignment = TextAnchor.MiddleCenter;
            optionStyle.fixedHeight = EditorGUIUtility.singleLineHeight + 5;
            optionStyle.fixedWidth = 295;

            // Start a scroll view to handle long lists
            if (filtered.Count > 10)
            {
                // Adjust the width of the option style for scroll view
                optionStyle.fixedWidth = 283;

                // If there are more than 10 options, use a scroll view
                GUILayout.BeginVertical();
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(275));
            }

            // Display the options as buttons
            foreach (var option in filtered)
            {
                if (GUILayout.Button(option, optionStyle))
                {
                    int index = options.IndexOf(option);
                    onSelect?.Invoke(index);
                    Close();
                }
            }

            // End the scroll view if it was started
            if (filtered.Count > 10)
            {
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }
        }

        private List<string> NameFilter(string searchTerm) => string.IsNullOrEmpty(search) ? options : options.FindAll(o => o.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

        private List<string> TagFilter(string searchTerm)
        {
            // If no tags are provided, return an empty list
            if (tags == null || tags.Count == 0) return new List<string>();

            // If search is empty, return all options
            if (string.IsNullOrEmpty(search)) return options;

            // Get the indexes of tags that match the search term
            List<int> matchingTagIndexes = new List<int>();
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matchingTagIndexes.Add(i);
                }
            }

            // Show the options at the indexes that match the search term
            List<string> taggedOptions = new List<string>();
            for (int i = 0; i < options.Count + 1; i++)
            {
                if (matchingTagIndexes.Contains(i))
                {
                    taggedOptions.Add(options[i]);
                }
            }

            // Return the filtered options based on the tags that match the search term
            return taggedOptions;
        }
    }
}
