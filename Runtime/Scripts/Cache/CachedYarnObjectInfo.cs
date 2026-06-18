using System;

namespace Thimble
{
    [System.Serializable]
    public struct CachedYarnObjectInfo : IEquatable<CachedYarnObjectInfo>
    {
        public string ObjectName;
        public string ConditionName;
        public string SceneName;
        public bool IsLocal;

        private int hashCode;

        public CachedYarnObjectInfo(string objectName, string conditionName, string sceneName, bool isLocal)
        {
            ObjectName = objectName;
            ConditionName = conditionName;
            SceneName = sceneName;
            IsLocal = isLocal;

            hashCode = HashCode.Combine(ObjectName, ConditionName, SceneName, IsLocal);
        }

        public static CachedYarnObjectInfo FromReference(ICachedYarnObject reference) => new CachedYarnObjectInfo(reference.ObjectName, reference.ConditionName, reference.SceneName, reference.IsLocal);

        public bool Equals(CachedYarnObjectInfo other) => ObjectName == other.ObjectName && ConditionName == other.ConditionName && SceneName == other.SceneName && IsLocal == other.IsLocal;

        public override bool Equals(object obj) => obj is CachedYarnObjectInfo other && Equals(other);

        public override int GetHashCode()
        {
            // If the hash code hasn't been calculated yet, calculate it now
            if (hashCode == 0) hashCode = HashCode.Combine(ObjectName, ConditionName, SceneName, IsLocal);

            // Return the cached hash code
            return hashCode;
        }
    }
}
