using UnityEngine;
using Yarn.Unity;

namespace Thimble
{
    public class ConditionalFunction : MonoBehaviour
    {
        /// <summary>
        /// Based on the value of the condition it returns the correct string.
        /// </summary>
        /// <param name="condition">The boolean condition to compare against.</param>
        /// <param name="trueString">The string to return if the condition has been met</param>
        /// <param name="falseString">The string to return if the condition hasn't been met</param>
        /// <returns>A string value based on the value of the aforementioned condition.</returns>
        [YarnFunction("AsCondition")]
        public static string AsCondition(bool condition, string trueString, string falseString) => condition ? trueString : falseString;

        /// <summary>
        /// Searches for the first object with a matching name from the specified list.
        /// </summary>
        /// <remarks>
        /// The search is performed in the order of the names provided. 
        /// If multiple objects exist with the same name, only the first match is considered.
        /// Does not search for objects that are inactive in the scene. If no objects are found with any of the specified names, an empty string is returned.
        /// </remarks>
        /// <param name="concatNames">An array of object names to search for. The method checks each name in order.</param>
        /// <returns>The name of the first object found that matches one of the specified names; otherwise, an empty string if no objects are found.</returns>
        [YarnFunction("FindByOrder")]
        public static string FindByOrder(string concatNames)
        {
            // Split the input string into an array of names using a comma as the delimiter.
            var names = concatNames.Trim().Split(",");

            // Loop through the list of names.
            foreach (var name in names)
            {
                // Search for an object with this name
                var obj = GameObject.Find(name);

                // If we found an object with this name, return it.
                if (obj != null) return name;
            }

            // If we get here, then we didn't find any of the objects.
            return string.Empty;
        }
    }
}
