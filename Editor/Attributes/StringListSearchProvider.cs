using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace Thimble.Editor
{
    public class StringListSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private string windowTitle = "Select an Option";
        private List<string> options = new List<string>();
        private Action<string> onSelect;
        private Action<int> onIndexChanged;

        // Dictionary to map search tree entries to their indices
        private static Dictionary<SearchTreeEntry, int> treeEntries = new();

        public StringListSearchProvider(string windowTitle, List<string> options, Action<string> callback)
        {
            // Set the instance variables
            this.windowTitle = windowTitle;
            this.options = options;
            this.onSelect = callback;
        }

        public StringListSearchProvider SetValues(string windowTitle, List<string> options, Action<string> callback, Action<int> indexChangedCallback = null)
        {
            // Set the instance variables
            this.windowTitle = windowTitle;
            this.options = options;
            this.onSelect = callback;
            this.onIndexChanged = indexChangedCallback;

            // Return the updated instance
            return this;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) => BuildSearchTreeEntries(windowTitle, GetSortedOptions(options));

        private List<SearchTreeEntry> BuildSearchTreeEntries(string windowTitle, List<string> options)
        {
            // Clear previous entries
            treeEntries.Clear();

            // Initialize the list of search tree entries
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
            {
                // Add the root group entry with the window title
                new SearchTreeGroupEntry(new GUIContent(windowTitle), 0)
            };

            // List to keep track of added groups to avoid duplicates
            List<string> groups = new List<string>();

            // Iterate through each option to build the search tree
            for (int i = 0; i < options.Count; i++)
            {
                string item = options[i];

                // Initialize the group name
                string groupName = string.Empty;

                // Split the item by '/' to determine its group and title
                string[] entryTitle = item.Split('/');

                // Iterate through the segments to create group entries
                for (int j = 0; j < entryTitle.Length - 1; j++)
                {
                    // Build the group name by concatenating segments
                    groupName += entryTitle[j];

                    // If this group hasn't been added yet, add it to the entries
                    if (!groups.Contains(groupName))
                    {
                        // Add the group entry to the search tree
                        entries.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[j]), j + 1));

                        // Add the group name to the list of added groups
                        groups.Add(groupName);
                    }

                    // Append '/' to the group name for the next segment
                    groupName += "/";
                }

                // Add the actual entry to the search tree
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));

                // Set the level of the entry based on its depth in the hierarchy
                entry.level = entryTitle.Length;

                // Set the user data to the entry title for identification
                entry.userData = entryTitle.Last();

                // Add the entry to the list
                entries.Add(entry);

                // Map the item to its index for quick lookup
                treeEntries.Add(entry, i);
            }

            // Return the populated list of search tree entries
            return entries;
        }

        private List<string> GetSortedOptions(List<string> options)
        {
            // Sort options to ensure group entries come before regular entries
            options.Sort((a, b) =>
            {
                // Split the first string by '/'
                string[] splits1 = a.Split('/');

                // Split the second string by '/'
                string[] splits2 = b.Split('/');

                // Compare each segment of the split strings
                for (int i = 0; i < splits1.Length; i++)
                {
                    // If we've reached the end of splits2, a is greater than b
                    if (i >= splits2.Length) return 1;

                    // Compare the current segments of both strings
                    int value = splits1[i].CompareTo(splits2[i]);

                    // If they are different, return the comparison result
                    if (value != 0)
                    {
                        // Make sure that group entries come before regular entries
                        if (splits1[i].Length != splits2[i].Length && (i == splits1.Length - 1 || i == splits2.Length - 1))
                        {
                            // If one is a group entry and the other is a regular entry, the group entry comes first
                            return splits1.Length < splits2.Length ? -1 : 1;
                        }
                        else
                        {
                            // If both are group entries or both are regular entries, sort alphabetically
                            return value;
                        }
                    }
                }

                // If all segments are the same, the shorter string comes first
                return 0;
            });

            // Return the sorted options
            return options;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            // Invoke the callback with the selected entry's user data if it's not null
            onSelect?.Invoke((string)SearchTreeEntry.userData);

            // Get the index of the selected entry
            int selectedIndex = GetSearchTreeIndex(SearchTreeEntry);

            // Invoke the index changed callback if it's set
            if (selectedIndex != -1) onIndexChanged?.Invoke(selectedIndex);

            // Return true to indicate the entry was successfully selected
            return true;
        }

        private int GetSearchTreeIndex(SearchTreeEntry entry)
        {
            // Try to get the index of the entry from the dictionary
            if (treeEntries.TryGetValue(entry, out int index)) return index;

            // If the entry is not found, return -1
            return -1;
        }
    }
}
