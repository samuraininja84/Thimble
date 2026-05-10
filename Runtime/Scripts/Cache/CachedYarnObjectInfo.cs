namespace Thimble
{
    [System.Serializable]
    public struct CachedYarnObjectInfo
    {
        public string ObjectName;
        public string ConditionName;
        public string SceneName;
        public bool IsLocal;

        public CachedYarnObjectInfo(string objectName, string conditionName, string sceneName, bool isLocal)
        {
            ObjectName = objectName;
            ConditionName = conditionName;
            SceneName = sceneName;
            IsLocal = isLocal;
        }

        public static CachedYarnObjectInfo FromReference(ICachedYarnObject reference) => new CachedYarnObjectInfo(reference.ObjectName, reference.ConditionName, reference.SceneName, reference.IsLocal);
    }
}
