using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Thimble.Editor
{
    public static class CachedNameGenerator
    {
        public static string CachedPath => EditorPrefs.GetString(CachedPathKey, "Assets");
        public const string CachedPathKey = "Thimble_CachedNameGenerator_ScriptCreationPath";
        public const string FileExtension = "yarn";
        public const string ConditionMethod = "AsCondition";
        public const string FindByOrderMethod = "FindByOrder";

        [MenuItem("Tools/Thimble/Generate Yarn Object Cache")]
        public static void CreateCache()
        {
            // Create a new dictionary to store the cached names, where the key is the Yarn name and the value is a list of CachedYarnObjectInfo structs
            Dictionary<string, List<CachedYarnObjectInfo>> cache = new();

            // Get the path of the currently active scene before processing all scenes
            string currentScenePath = EditorSceneManager.GetActiveScene().path;

            // Loop through all the scenes in the build settings and search for objects with the target script
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                // Get the scene at the current index
                Scene scene = EditorBuildSettings.scenes[i].path != null ? EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path) : default;

                // If the scene is valid, search for objects with the target script
                if (scene.IsValid())
                {
                    // Get the cache of objects that implement the ICachedYarnObject interface in the scene
                    var cacheInScene = GetCacheInScene(scene);

                    // Loop through the cache of objects in the scene and add their YarnName, ObjectName, and ConditionName to the main cache
                    foreach (var cachedObject in cacheInScene)
                    {
                        // If the main cache does not contain the YarnName of the cached object, add it to the main cache with an empty list as its value
                        if (!cache.ContainsKey(cachedObject.YarnName)) cache[cachedObject.YarnName] = new List<CachedYarnObjectInfo>();

                        // Add the ObjectName and ConditionName of the cached object to the list of CachedYarnObjectInfo structs associated with its YarnName in the main cache
                        cache[cachedObject.YarnName].Add(CachedYarnObjectInfo.FromReference(cachedObject));
                    }
                }
            }

            // Reopen the originally active scene after processing all scenes
            EditorSceneManager.OpenScene(currentScenePath);

            // Check if the cache is empty, and if it is, display a dialog to the user indicating that no objects were found in the scenes that implement the ICachedYarnObject interface and return early
            if (cache.Count == 0)
            {
                // Display a dialog to the user indicating that no objects were found in the scenes that implement the ICachedYarnObject interface
                EditorUtility.DisplayDialog("No Cached Yarn Objects Found", "No objects were found in the scenes that implement the ICachedYarnObject interface. Please make sure that there are objects in the scenes that implement the ICachedYarnObject interface and try again.", "OK");

                // Return early to prevent the user from being prompted to save an empty cache
                return;
            }

            // Move the local entries to the end of the cache, and add a comment for each scene name before the local entries for that scene
            cache = cache.OrderBy(entry => entry.Value.Count > 0 && entry.Value[0].IsLocal ? 1 : 0).ToDictionary(entry => entry.Key, entry => entry.Value);

            // Ask the user to select a path to save the generated script, using the scriptCreationPath variable as the default path
            string path = EditorUtility.SaveFilePanel("Save Yarn Object Cache", CachedPath, "YarnObjectCache", FileExtension);

            // If the user selected a valid path, save the cache to a script at that path
            if (!string.IsNullOrEmpty(path))
            {
                // Save the selected path to EditorPrefs for future use
                SaveCacheToPath(path, cache);

                // Save the selected path to EditorPrefs for future use
                EditorPrefs.SetString(CachedPathKey, path);
            }
        }
         
        public static void SaveCacheToPath(string path, Dictionary<string, List<CachedYarnObjectInfo>> cache)
        {
            // Create a new file at the specified path
            var file = System.IO.File.Create(path);

            // Create a new StreamWriter to write to the file
            var writer = new System.IO.StreamWriter(file);

            // Write the title of the node at the top of the file in the format "title: YarnObjectCache"
            writer.WriteLine("title: YarnObjectCache");

            // Add the "---" separator after the title
            writer.WriteLine("---");

            // Initialize a new string variable called lastScene to keep track of the last scene name that was processed, and set it to an empty string
            string lastScene = string.Empty;

            // Write each entry in the cache to the file, using the FormatYarnObjectInfo method to format the YarnName, ObjectName, and ConditionName of each entry
            foreach (var entry in cache)
            {
                // Get the referenced scenes for the current entry
                string referencedScenes = GetReferencedScenes(entry.Value);

                // Check if the referenced scenes for the current entry are different from the last scene that was processed
                if (lastScene != referencedScenes)
                {
                    // If this is not the first entry in the cache, add a blank line before the entry in the file to improve readability
                    if (entry.Key != cache.Keys.First()) writer.WriteLine();

                    // Add a comment before the entry depending on whether it's local or not
                    if (entry.Value[0].IsLocal)
                    {
                        // Write a comment for the local entry before the entry in the format "// Local To sceneName Scene:"
                        writer.WriteLine($"// Local To: {entry.Value[0].SceneName}");

                        // Update the lastScene variable to the current scene name
                        lastScene = entry.Value[0].SceneName;
                    }
                    else
                    {
                        // If there are multiple referenced scenes, write "Scenes: sceneName1, sceneName2, sceneName3", otherwise write ": sceneName Scene"
                        string multipleScenes = referencedScenes.Contains(",") ? $" Scenes: {referencedScenes}" : $": {referencedScenes}";

                        // Write a comment for the referenced scenes before the entry in the format "// Referenced In Scenes: sceneName1, sceneName2, sceneName3"
                        writer.WriteLine($"// Referenced In{multipleScenes}");
                    }
                }

                // Update the lastScene variable to the current scene name
                lastScene = referencedScenes;

                // Write the formatted YarnName, ObjectName, and ConditionName of the current entry to the file
                writer.WriteLine(FormatYarnObjectInfo(entry.Key, entry.Value));
            }

            // Add the "===" separator at the end of the file
            writer.WriteLine("===");

            // Close the StreamWriter and the file
            writer.Close();
            file.Close();
        }

        public static string FormatYarnObjectInfo(string yarnName, List<CachedYarnObjectInfo> objectInfoList)
        {
            // If there are multiple objects in the list, create a Dictionary to store the unique object names and add each object name from the list to the Dictionary
            Dictionary<string, List<string>> uniqueObjects = new();

            // Loop through the list of CachedYarnObjectInfo structs and add each ObjectName to the Dictionary of unique object names
            foreach (var objectInfo in objectInfoList)
            {
                // If the Dictionary of unique object names already contains the ObjectName of the current CachedYarnObjectInfo struct, skip it and continue to the next one
                if (uniqueObjects.ContainsKey(objectInfo.ObjectName)) continue;

                // Initialize a new list for the smart variable integration
                List<string> values = new();

                // If the scene name to the values list if isLocal is true
                if (objectInfo.IsLocal) values.Add(objectInfo.SceneName.ToLower() + "_" + yarnName);
                else values.Add(yarnName);

                // Add the ConditionName to the values list, if we have one
                if (objectInfo.ConditionName.HasCondition()) values.Add(objectInfo.ConditionName);

                // Add the ObjectName and ConditionName of the current CachedYarnObjectInfo struct to the Dictionary of unique object names, using the ObjectName as the key and the ConditionName as the value
                uniqueObjects.Add(objectInfo.ObjectName, values);
            }

            // If there is only one object in the list, return a string in the format "<<declare $memorySection = "objectName">>"
            if (uniqueObjects.Count == 1)
            {
                // Get the first (and only) key-value pair from the Dictionary of unique object names
                var uniqueObject = uniqueObjects.First();

                // Format the Yarn name by appending the Yarn prefix to it, and return a string in the format "<<declare {yarnName} = "objectName">>"
                return $"<<declare {uniqueObject.Value[0].AppendYarnPrefix()} = \"{uniqueObject.Key}\">>";
            }

            // If there is two items in the list, return a function that uses a boolean to get the correct value
            if (uniqueObjects.Count == 2)
            {
                // Cache the info list items
                var firstItem = uniqueObjects.ElementAt(0);
                var secondItem = uniqueObjects.ElementAt(1);

                // Get the condition for each item, if we have one
                var firstItemCondition = firstItem.Value.Count > 1 ? firstItem.Value[1] : string.Empty;
                var secondItemCondition = secondItem.Value.Count > 1 ? secondItem.Value[1] : string.Empty;

                // Check if both items have a condition, and if they do, and the conditions are the same, return a function that uses the condition to get the correct value
                if (firstItemCondition.HasCondition() && secondItemCondition.HasCondition() && string.Equals(firstItemCondition, secondItemCondition, System.StringComparison.OrdinalIgnoreCase))
                {
                    // Return a string in the format "<<declare {yarnName} = AsCondition({conditionName}, {objectName_1}, {objectName_2})>>
                    return $"<<declare {firstItem.Value[0].AppendYarnPrefix()} = {ConditionMethod}({firstItemCondition}, \"{firstItem.Key}\", \"{secondItem.Key}\")>>";
                }
            }

            // If there are multiple objects in the list, create a string in the format "<<declare {yarnName_condition} = {FindByOrderMethod}("objectName1, objectName2, ...")>>"
            return $"<<declare {uniqueObjects.ElementAt(0).Value[0].AppendYarnPrefix()} = {FindByOrderMethod}(\"{string.Join(", ", uniqueObjects.Select(kv => $"{kv.Key}"))}\")>>";
        }

        public static string GetReferencedScenes(List<CachedYarnObjectInfo> objectInfoList)
        {
            // Create a new HashSet to store the unique scene names, and add each SceneName from the list of CachedYarnObjectInfo structs to the HashSet
            HashSet<string> uniqueScenes = new();

            // Loop through the list of CachedYarnObjectInfo structs and add each SceneName to the HashSet of unique scene names
            foreach (var objectInfo in objectInfoList) uniqueScenes.Add(objectInfo.SceneName);

            // Return a string containing the unique scene names separated by commas, in the format "Scene1, Scene2, Scene3"
            return string.Join(", ", uniqueScenes);
        }

        public static List<ICachedYarnObject> GetCacheInScene(Scene scene)
        {
            // Create a new dictionary to store the cached names, where the key is the Yarn name and the value is a list of CachedYarnObjectInfo structs
            List<ICachedYarnObject> cache = new();

            // Get all the root game objects in the scene
            var rootObjects = scene.GetRootGameObjects();

            // Loop through the valid objects found in the scene and add their YarnName, ObjectName, and ConditionName to the cache
            for (int i = 0; i < rootObjects.Length; i++)
            {
                // Get all the components in the root game object and its children, including inactive ones, that are of type MonoBehaviour
                var components = rootObjects[i].GetComponentsInChildren<MonoBehaviour>(true);

                // Loop through the components and check if they implement the ICachedYarnObject interface. If they do, add their YarnName, ObjectName, and ConditionName to the cache
                foreach (var component in components)
                {
                    // Check if the component's type is exactly ICachedYarnObject or if it implements the ICachedYarnObject interface
                    if (component.GetType() == typeof(ICachedYarnObject) || component.GetType().GetInterface(nameof(ICachedYarnObject)) != null)
                    {
                        // If the component implements the ICachedYarnObject interface, add its YarnName, ObjectName, and ConditionName to the cache
                        var instance = component as ICachedYarnObject;

                        // If the instance is not null, add it to the cache
                        if (instance != null) cache.Add(instance);
                    }
                }
            }

            // Return the cache containing the YarnName, ObjectName, and ConditionName of all the objects in the scene that implement the ICachedYarnObject interface
            return cache;
        }
    }
}
