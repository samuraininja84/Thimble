using UnityEngine;

public class YarnNodeDropdownAttribute : PropertyAttribute
{
    public string FolderPath;

    public YarnNodeDropdownAttribute(string folderPath)
    {
        FolderPath = folderPath;
    }
}