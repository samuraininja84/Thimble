using UnityEngine;

public class YarnCommandDropdownAttribute : PropertyAttribute
{
    public string FolderPath;

    public YarnCommandDropdownAttribute(string folderPath)
    {
        FolderPath = folderPath;
    }
}