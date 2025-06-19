using UnityEngine;

namespace Thimble
{
    /// <summary>
    /// Specifies that a property should display a dropdown menu populated with Yarn commands from a specified folder path in the Unity Inspector.
    /// </summary>
    /// <remarks>
    /// This attribute is intended for use in Unity to enhance the editor experience by allowing developers to select Yarn commands from a dropdown menu, rather than manually typing them. 
    /// The commands are retrieved from the folder specified by the <paramref name="folderPath"/> parameter.
    /// </remarks>
    public class YarnCommandDropdownAttribute : PropertyAttribute
    {
        public string FolderPath;

        public YarnCommandDropdownAttribute(string folderPath)
        {
            FolderPath = folderPath;
        }
    }
}