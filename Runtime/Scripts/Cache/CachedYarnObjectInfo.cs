using System;

namespace Thimble
{
    [Serializable]
    public readonly struct CachedYarnObjectInfo : IEquatable<CachedYarnObjectInfo>
    {
        public string ObjectName { get; }
        public string ConditionName { get; }
        public string SceneName { get; }
        public bool IsLocal { get; }

        private readonly int hashCode;

        public CachedYarnObjectInfo(string objectName, string conditionName, string sceneName, bool isLocal)
        {
            ObjectName = objectName;
            ConditionName = conditionName;
            SceneName = sceneName;
            IsLocal = isLocal;

            // Calculate the hash code once and store it, since the struct is immutable
            hashCode = HashCode.Combine(ObjectName, ConditionName, SceneName, IsLocal);
        }

        public static CachedYarnObjectInfo FromReference(ICachedYarnObject reference) => new CachedYarnObjectInfo(reference.ObjectName, reference.ConditionName, reference.SceneName, reference.IsLocal);

        public bool Equals(CachedYarnObjectInfo other) => ObjectName == other.ObjectName && ConditionName == other.ConditionName && SceneName == other.SceneName && IsLocal == other.IsLocal;

        public override bool Equals(object obj) => obj is CachedYarnObjectInfo other && Equals(other);

        public override int GetHashCode() => hashCode;
    }
}
