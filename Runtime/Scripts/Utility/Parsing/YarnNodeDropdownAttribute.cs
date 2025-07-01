using UnityEngine;

namespace Thimble
{
    /// <summary>
    /// Specifies that a property should be displayed as a dropdown menu populated with Yarn node names from a specified folder path in the Unity Editor.
    /// </summary>
    /// <remarks>
    /// This attribute is intended for use in Unity's Inspector to provide a dropdown menu for selecting Yarn Node names. 
    /// The dropdown is populated based on the Yarn files located in the specified folder path.
    /// </remarks>
    public class YarnNodeDropdownAttribute : PropertyAttribute
    {
        public string FolderPath;

        public YarnNodeDropdownAttribute(string folderPath)
        {
            FolderPath = folderPath;
        }
    }
}