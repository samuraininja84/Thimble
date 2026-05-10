using System;

namespace Thimble
{
    public interface ICachedYarnObject
    {
        /// <summary>
        /// The name of the object to be used in the Yarn script, this is not necessarily the same as the name of the object in Unity, and may be overridden by a condition.
        /// </summary>
        /// <remarks>Used to create the yarn variable name.</remarks>
        string YarnName { get; }

        /// <summary>
        /// The name of the object to be used in the Yarn script, this is the name of the object in Unity. 
        /// </summary>
        /// <remarks>The value of the Yarn Variable will be the name of the object in Unity, this is used to find the object <see cref="Type"/> in the scene or database.</remarks>
        string ObjectName { get; }

        /// <summary>
        /// The name of the condition that must be true for this object's name to be used.
        /// </summary>
        /// <remarks>If null or empty, this object will always be used.</remarks>
        string ConditionName { get; }

        /// <summary>
        /// The desired value for the condition to be for it to be the valid name for the object.
        /// </summary>
        bool DesiredValue { get; }

        /// <summary>
        /// The name of the scene that this object is local to, if it is local. 
        /// </summary>
        /// <remarks>This is used to create the yarn variable name for local objects, and is not necessarily the same as the name of the scene in Unity.</remarks>
        string SceneName { get; }

        /// <summary>
        /// Checks if the object is local to the scene or not. 
        /// </summary>
        /// <remarks>Local objects will have their scene name appended to their YarnName in the cache, allowing for multiple objects with the same name across different scenes.</remarks>
        bool IsLocal { get; }
    }
}
