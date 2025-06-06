using UnityEngine;

namespace Thimble
{
    public interface ICommandHandle
    {
        public virtual void ActivateCommands() { }

        public virtual void DeactivateCommands() { }
    }
}
