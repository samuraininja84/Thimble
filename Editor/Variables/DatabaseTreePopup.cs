using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Thimble.Editor
{
    public class DatabaseTreePopup : PopupWindowContent
    {
        private readonly SearchField _searchField;
        private readonly DatabaseTreeView _treeView;
        private bool _shouldClose;

        public float Width { get; set; }

        public DatabaseTreePopup(DatabaseTreeView contents)
        {
            _searchField = new SearchField();
            _treeView = contents;
        }

        public override void OnGUI(Rect rect)
        {
            // Define constants for layout
            const int border = 4;
            const int topPadding = 12;
            const int searchHeight = 16;
            const int remainTop = topPadding + searchHeight + border;

            // Calculate the width of the toggle button (if any) and adjust the search field width accordingly
            var toggleWidth = 0;

            // Calculate the rectangles for the search field, toggle button, and remaining tree view area
            var searchRect = new Rect(border, topPadding, rect.width - toggleWidth - border * 2, searchHeight);
            var toggleRect = new Rect(searchRect.width + border * 2, topPadding, toggleWidth - border, searchHeight);
            var remainingRect = new Rect(border, topPadding + searchHeight + border, rect.width - border * 2, rect.height - remainTop - border);

            // Draw the search field at the top of the popup
            _treeView.searchString = _searchField.OnGUI(searchRect, _treeView.searchString);

            // Draw the tree view in the remaining space below the search field
            _treeView.OnGUI(remainingRect);

            // If close is flagged, close the popup
            if (_shouldClose)
            {
                // Clear the hot control to prevent any lingering interactions with the tree view
                GUIUtility.hotControl = 0;

                // Close the popup window
                editorWindow.Close();
            }

            // If the user clicks anywhere outside of the tree view, close the popup
            if (_treeView.HasSelection()) ForceClose();
        }

        public override void OnOpen()
        {
            // Set focus to the search field when the popup opens, allowing the user to start typing immediately
            _searchField.SetFocus();

            // Call the base method to ensure any necessary initialization in the parent class is performed
            base.OnOpen();
        }

        public override Vector2 GetWindowSize()
        {
            // Get the base window size from the parent class
            var result = base.GetWindowSize();

            // Set the width to the specified value, allowing for a wider popup if needed
            result.x = Width;

            // Set a default height if the base height is too small, ensuring the popup is large enough to display its contents
            return result;
        }

        private void ForceClose() => _shouldClose = true;
    }
}
