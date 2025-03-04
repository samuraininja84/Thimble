using UnityEngine;

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

        #if UNITY_EDITOR

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            // Set object name to the name of the node
            name = nodePointer.linkName;

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
