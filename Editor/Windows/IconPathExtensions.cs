namespace Thimble.Editor
{
    public static class IconPathExtensions
    {
        public const string IconPath = "Assets/Plugins/Thimble/Editor/EditorResources/";

        public static string GetIconPath(this string name, string extension = ".png") => IconPath + name + extension;
    }
}