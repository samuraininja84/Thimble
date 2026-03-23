using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace Thimble.Editor
{
    public static class SearchUtility
    {
        public static bool OpenStringSearchWindow(string windowTitle, List<string> options, Action<string> callback)
        {
            // Open the search window at the current mouse position with a new StringListSearchProvider
            return GUIUtility.GUIToScreenPoint(Event.current.mousePosition).OpenStringSearchWindow(windowTitle, options, callback);
        }

        public static bool OpenStringSearchWindow(this Vector2 position, string windowTitle, List<string> options, Action<string> callback, Action<int> indexChangedCallback = null)
        {
            // Open the search window at the specified position with a new StringListSearchProvider
            return SearchWindow.Open(new SearchWindowContext(position), ScriptableObject.CreateInstance<StringListSearchProvider>().SetValues(windowTitle, options, callback, indexChangedCallback));
        }
    }
}
