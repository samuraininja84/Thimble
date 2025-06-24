using UnityEngine;

namespace Thimble
{
    /// <summary>
    /// Specifies that a property should display a dropdown menu populated with Yarn variables from a specified folder path in the Unity Inspector.
    /// </summary>
    /// <remarks>
    /// The dropdown is populated based on the Yarn variables found in the folder specified by <see cref="FolderPath"/>.
    /// </remarks>
    public class YarnVariableDropdownAttribute : PropertyAttribute
    {
        public string FolderPath;

        public YarnVariableDropdownAttribute(string folderPath)
        {
            FolderPath = folderPath;
        }
    }
}