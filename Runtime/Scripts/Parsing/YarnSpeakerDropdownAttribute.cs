using UnityEngine;

public class YarnSpeakerDropdownAttribute : PropertyAttribute
{
    public string FolderPath;

    public YarnSpeakerDropdownAttribute(string folderPath)
    {
        FolderPath = folderPath;
    }
}
