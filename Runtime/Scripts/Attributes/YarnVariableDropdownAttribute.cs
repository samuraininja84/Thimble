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
        /// <summary>
        /// Represents the path to a folder in the file system.
        /// </summary>
        public string FolderPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="YarnVariableDropdownAttribute"/> class with the specified folder path.
        /// </summary>
        /// <param name="folderPath">The path to the folder containing Yarn variables. This value is used to populate the dropdown with available variables.</param>
        public YarnVariableDropdownAttribute(string folderPath)
        {
            FolderPath = folderPath;
        }
    }
}