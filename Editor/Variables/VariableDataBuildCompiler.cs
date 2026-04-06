using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Thimble.Editor
{
    internal class VariableDataBuildCompiler : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private VariableData variableData;
        private bool _removeFromPreloadedAssets;

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            // Get the variable data instance.
            variableData = VariableData.Instance;

            // Check if the variable data is null, if it is then we don't need to add it to the preloaded assets.
            if (variableData == null) return;

            // Initialize the variable data to ensure that all of the variables are loaded and ready to be used during the build process.
            variableData.Initialize();

            // Get the preloaded assets from the player settings.
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();

            // If the preloaded assets doesn't contain the variable data add it.
            if (!preloadedAssets.Contains(variableData))
            {
                // Add the variable data to the preloaded assets.
                ArrayUtility.Add(ref preloadedAssets, variableData);

                // Set the preloaded assets back to the player settings.
                PlayerSettings.SetPreloadedAssets(preloadedAssets);

                // Set the flag to remove the variable data from the preloaded assets after the build process is complete.
                _removeFromPreloadedAssets = true;
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            // Check if the variable data is null or if we don't need to remove it from the preloaded assets, if either of those is true then we don't need to do anything.
            if (variableData == null || !_removeFromPreloadedAssets) return;

            // Get the preloaded assets from the player settings.
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();

            // Remove the variable data from the preloaded assets.
            ArrayUtility.Remove(ref preloadedAssets, variableData);

            // Set the preloaded assets back to the player settings.
            PlayerSettings.SetPreloadedAssets(preloadedAssets);

            // Set the variable data to null to avoid any potential issues with it being used after the build process is complete.
            variableData = null;
        }
    }
}
