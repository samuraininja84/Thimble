using UnityEngine;

namespace Thimble
{
    public static class VariableExtensions
    {
        /// <summary>
        /// The prefix used to identify variable names in the system.
        /// </summary>
        public const string Prefix = "$";

        /// <summary>
        /// The default name used for variables that do not have a specified name. This can be used as a placeholder or to indicate that a variable is unnamed.
        /// </summary>
        public const string MissingVariableName = "None";

        /// <summary>
        /// Determines whether two floating-point values are approximately equal within a specified tolerance.
        /// </summary>
        /// <param name="a">The first floating-point value to compare.</param>
        /// <param name="b">The second floating-point value to compare.</param>
        /// <param name="epsilon">The maximum allowable difference between the two values for them to be considered approximately equal. The default value is 0.0001.</param>
        /// <returns><see langword="true"/> if the absolute difference between <paramref name="a"/> and <paramref name="b"/> is less than or equal to <paramref name="epsilon"/>; otherwise, <see langword="false"/>.</returns>
        public static bool Approximately(this float a, float b, float epsilon = 1E-06f) => Mathf.Abs(a - b) <= epsilon;
    }
}