using UnityEngine;

namespace Thimble
{
    /// <summary>
    /// Specifies that a property should display a dropdown menu populated with speaker names from a specified folder path in the Unity Editor.
    /// </summary>
    /// <remarks>
    /// This attribute is intended for use in Unity to enhance the editor experience by providing a dropdown menu for selecting speaker names. 
    /// The dropdown is populated based on the contents of the folder specified by <paramref name="folderPath"/>.
    /// </remarks>
    public class YarnSpeakerDropdownAttribute : PropertyAttribute
    {
        public string FolderPath;

        public YarnSpeakerDropdownAttribute(string folderPath)
        {
            FolderPath = folderPath;
        }
    }
}