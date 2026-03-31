using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

namespace Thimble.Editor
{
    public class DatabaseTreeView : TreeView
    {
        private readonly string _currentEntry;
        private readonly Action<string> _selectionHandler;
        private readonly VariableType variableType;
        private int _selectedId = -1;

        private TreeViewItem Root { get; set; }

        public static bool filterOn => DatabaseTreePopup.filterOn;

        public static string filterName => VariableHandler.InternalVariableDenotator;

        public DatabaseTreeView(string currentEntry, Action<string> selectionHandler, VariableType type) : base(new TreeViewState())
        {
            _currentEntry = currentEntry;
            _selectionHandler = selectionHandler;
            variableType = type;
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            // Create the root of the tree with a unique ID of -1.
            Root = new TreeViewItem(-1, -1);

            // This ID will be used to assign unique IDs to each tree item.
            var id = 1;

            // This list will hold the groups for each area handle, which will be added to the root at the end.
            var groups = new List<TreeViewItem>();

            // Add a default "None" option at the top of the list for users to select if they want to clear the variable selection
            Root.AddChild(CollectionTreeViewItem.Create(VariableHandler.MissingVariableName, id++, VariableHandler.MissingVariableName));

            // Create a group for each variable type to organize the variables in the tree view. For example, we can create a group for float variables, another for int variables, etc.
            var floatGroup = new TreeViewItem(id++) { displayName = "Float Variables" };
            var stringGroup = new TreeViewItem(id++) { displayName = "String Variables" };
            var boolGroup = new TreeViewItem(id++) { displayName = "Bool Variables" };

            // Iterate through all float variable in the VariableData instance
            foreach (var variable in VariableData.Instance.floatVariables)
            {
                // If filtering is on, skip variables that don't contain the filter name (e.g., "Yarn.Internal") in their key.
                if (filterOn && variable.Key.Contains(filterName, StringComparison.OrdinalIgnoreCase)) continue;

                // Add each variable as a child of the float group with its name formatted for display. Use the variable's name as the label, and assign an icon if desired.
                floatGroup.AddChild(CollectionTreeViewItem.Create(variable.Key, id++, variable.Key));
            }

            // Add the float group to the list of groups if it has any children
            if (floatGroup.hasChildren && (variableType == VariableType.Float || variableType == VariableType.None)) groups.Add(floatGroup);

            // Iterate through all string variable in the VariableData instance
            foreach (var variable in VariableData.Instance.stringVariables)
            {
                // If filtering is on, skip variables that don't contain the filter name (e.g., "Yarn.Internal") in their key.
                if (filterOn && variable.Key.Contains(filterName, StringComparison.OrdinalIgnoreCase)) continue;

                // Add each variable as a child of the string group with its name formatted for display. Use the variable's name as the label, and assign an icon if desired.
                stringGroup.AddChild(CollectionTreeViewItem.Create(variable.Key, id++, variable.Key));
            }

            // Add the string group to the list of groups if it has any children
            if (stringGroup.hasChildren && (variableType == VariableType.String || variableType == VariableType.None)) groups.Add(stringGroup);

            // Iterate through all bool variable in the VariableData instance
            foreach (var variable in VariableData.Instance.boolVariables)
            {
                // If filtering is on, skip variables that don't contain the filter name (e.g., "Yarn.Internal") in their key.
                if (filterOn && variable.Key.Contains(filterName, StringComparison.OrdinalIgnoreCase)) continue;

                // Add each variable as a child of the bool group with its name formatted for display. Use the variable's name as the label, and assign an icon if desired.
                boolGroup.AddChild(CollectionTreeViewItem.Create(variable.Key, id++, variable.Key));
            }

            // Add the bool group to the list of groups if it has any children
            if (boolGroup.hasChildren && (variableType == VariableType.Bool || variableType == VariableType.None)) groups.Add(boolGroup);

            // Parent the groups under the root based on how many there are.
            if (groups.Count == 1)
            {
                // If there is only one group, add its children directly to the root to avoid unnecessary nesting
                groups[0].children.ForEach(child => Root.AddChild(child));
            }
            else
            {
                // If there are multiple groups, add them as children of the root
                groups.ForEach(group => Root.AddChild(group));
            }

            // Set up the depths of the tree items based on their parent-child relationships
            SetupDepthsFromParentsAndChildren(Root);

            // Return the root of the tree, which contains all the groups and their variables as children
            return Root;
        }

        public override void OnGUI(Rect rect)
        {
            // If the selected ID is greater than -1, it means we have an item to frame
            if (_selectedId > -1)
            {
                // Frame the selected item in the tree view
                FrameItem(_selectedId);

                // Set the selected ID back to -1 to prevent continuous framing in subsequent OnGUI calls
                _selectedId = -1;
            }

            // Call the base OnGUI to render the tree view
            base.OnGUI(rect);
        }

        protected override bool CanMultiSelect(TreeViewItem item) => false;

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            // If no selection, do nothing
            if (FindItem(selectedIds[0], rootItem) is CollectionTreeViewItem item)
            {
                // Set the selected ID to frame the item in the next OnGUI call
                _selectionHandler(item.Entry);
            }
            else
            {
                // Toggle group expansion when clicking on a group
                SetExpanded(selectedIds[0], !IsExpanded(selectedIds[0]));

                // Clear selection when clicking on a group
                SetSelection(new int[] { });
            }
        }

        private class CollectionTreeViewItem : TreeViewItem
        {
            public readonly string Entry;

            public CollectionTreeViewItem(string entry, int id) : base(id, 0)
            {
                Entry = entry;
            }

            public static CollectionTreeViewItem Create(string entry, int id, string displayName) => new CollectionTreeViewItem(entry, id) { displayName = displayName };
        }
    }
}
