using UnityEngine;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thimble
{
    /// <summary>
    /// A struct that displays a reference to a node in a yarn file.
    /// </summary>
    /// <remarks>
    /// This struct is used to create a link or a reference to a node in a yarn file as a selectable int or a string. It also can retrieve the content of the node.
    /// </remarks>
    /// 
    [CreateAssetMenu(fileName = "New Node Reference", menuName = "Thimble/New Node Reference")]
    public class NodeReference : ScriptableObject
    {
        public TextAsset storyFile; 
        public NodePointer nodePointer;

        private void OnValidate()
        {
            if (storyFile == null) return;
            nodePointer.SetStoryFile(storyFile);
            nodePointer.ParseForTitles();
        }

        public string GetActiveNodeName()
        {
            // Get the active node name from the node pointer.
            return nodePointer.activeNodeName;
        }

        public List<string> GetTitles()
        {
            // Get the titles from the node pointer.
            return nodePointer.nodeNames;
        }

        public List<string> GetContent()
        {
            // Get the content from the node pointer.
            return nodePointer.ParseForContent();
        }

        #if UNITY_EDITOR

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            // Set object name to the name of the node
            name = nodePointer.activeNodeName;

            // Set file name to the name of the node
            string path = AssetDatabase.GetAssetPath(this);
            path = path.Replace(name + ".asset", "");
            path = path + name + ".asset";
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(this), name);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endif
    }
}
