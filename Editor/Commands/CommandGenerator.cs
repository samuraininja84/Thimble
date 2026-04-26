using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Yarn.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thimble.Editor
{
    public class CommandGenerator
    {
        private const string cacheKey = "Thimble_CommandInfoCache";

        private static string CachedPath => EditorPrefs.GetString(cacheKey, "Assets");

        #region Command Format

        // This is the structure of the JSON we want to generate for each command. We can then serialize this to a file and use it in our editor tools.
        //{
        //  "commands": [
        //      {
        //          "yarnName": "test_commands",
        //          "definitionName": "test_commands",
        //          "documentation": "This command does a thing",
        //          "signature": "test",
        //          "parameters": [
        //            {
        //                "name": "first parameter",
        //                "type": "number",
        //                "defaultValue": "0",
        //                "documentation": "Integers Only",
        //                "isParamsArray": false                    
        //            }
        //        ]
        //     }
        //  ]
        //}

        #endregion

        #region Function Format

        // This is the structure of the JSON we want to generate for each function. We can then serialize this to a file and use it in our editor tools.
        //{
        //"functions": [
        // {
        //   "yarnName": "bucket_has_item",
        //   "definitionName": "BucketHasItem",
        //   "fileName": "/path/to/file",
        //   "documentation": "Returns true if a particular bucket contains a specific item",
        //   "language": "csharp",
        //   "async": false,
        //   "containsErrors": false,
        //   "parameters": [
        //       {
        //           "name": "bucketName",
        //           "documentation": "the name of the bucket being queried",
        //           "isParamsArray": false,
        //           "type": "string"
        //       },
        //       {
        //   "name": "itemName",
        //           "documentation": "the name of the item we are curious about",
        //           "isParamsArray": false,
        //           "type": "string"
        //       }
        //   ],
        //   "return": {
        //   "type": "bool",
        //       "description": "true if itemName is within bucketName"
        //   }
        // }
        //}

        #endregion

        #region Json Formatting

        public static string FormatValue(object value) => JToken.Parse(JsonConvert.SerializeObject(value)).ToString(Formatting.Indented);

        public static string FormatObject(object value) => JToken.Parse(JsonUtility.ToJson(value, true)).ToString(Formatting.Indented);

        #endregion

        #region YSLS Generation

        [MenuItem("Tools/Thimble/Generate YSLS File")]
        public static void GenerateYSLS()
        {
            // Ask the user where they want to save the generated JSON file
            string path = EditorUtility.SaveFilePanelInProject("Save Command Info", "commands.ysls", "json", "Select a location to save the generated command info JSON file.", CachedPath);

            // Check if the user provided a valid path
            if (string.IsNullOrEmpty(path))
            {
                // Log a warning if the user cancels the save dialog
                Debug.LogWarning("Command info generation cancelled by user.");

                // Return early if the user cancels the save dialog
                return;
            }

            // Get all the command info for our project.
            var formated = GeneratorFormater.Create(GetAllCommandInfo(), GetAllFunctionInfo());

            // Serialize the formatted command info to JSON with indentation for readability
            string json = FormatValue(formated);

            // Write the JSON to the specified file
            System.IO.File.WriteAllText(path, json);

            // Cache the path where we saved the file for next time
            EditorPrefs.SetString(cacheKey, path);

            // Refresh the AssetDatabase to make sure the new file is recognized by Unity
            AssetDatabase.Refresh();
        }

        public static List<CommandInfo> GetAllCommandInfo()
        {
            // Initialize a list to hold our command info objects
            var commandInfoCache = new List<CommandInfo>();

            // Loop through each command and extract the necessary information to create our CommandInfo objects
            foreach (var command in GetAllCommands())
            {
                // Get the MethodInfo for this command
                var method = command.Key;

                // Get the YarnCommandAttribute for this method, if it exists
                var attribute = command.Value;

                // Create a CommandInfo object for this command
                var commandInfo = new CommandInfo
                {
                    yarnName = attribute?.Name ?? method.Name,
                    definitionName = method.Name,
                    documentation = "", // To-Do: Extend the YarnCommandAttribute to include method documentation, and extract it here
                    parameters = method.GetParameters().Select(p => new CommandInfo.ParameterInfo
                    {
                        name = p.Name,
                        type = ConvertParameterType(p.ParameterType.Name),
                        defaultValue = p.HasDefaultValue ? p.DefaultValue?.ToString() ?? "null" : "None",
                        documentation = "",
                        isParamsArray = p.GetCustomAttribute<ParamArrayAttribute>() != null
                    }).ToList()
                };

                // Add the command info to our cache
                commandInfoCache.Add(commandInfo);
            }

            // Cache the command info for future use
            return commandInfoCache;
        }

        public static List<FunctionInfo> GetAllFunctionInfo()
        {
            // Initialize a list to hold our function info objects
            var functionInfoCache = new List<FunctionInfo>();

            // Loop through each function and extract the necessary information to create our FunctionInfo objects
            foreach (var function in GetAllFunctions())
            {
                // Get the MethodInfo for this function
                var method = function.Key;

                // Get the YarnFunctionAttribute for this method, if it exists
                var attribute = function.Value;

                // Create a FunctionInfo object for this function
                var functionInfo = new FunctionInfo
                {
                    yarnName = attribute?.Name ?? method.Name,
                    definitionName = method.Name,
                    documentation = "", // To-Do: Extend the YarnFunctionAttribute to include method documentation, and extract it here
                    async = method.ReturnType == typeof(System.Threading.Tasks.Task) || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(System.Threading.Tasks.Task<>)),
                    containsErrors = false, // To-Do: Implement a way to determine if the function can throw errors and include that information in the JSON
                    parameters = method.GetParameters().Select(p => new FunctionInfo.ParameterInfo
                    {
                        name = p.Name,
                        type = ConvertParameterType(p.ParameterType.Name),
                        defaultValue = p.HasDefaultValue ? p.DefaultValue?.ToString() ?? "null" : "None",
                        documentation = "",
                        isParamsArray = p.GetCustomAttribute<ParamArrayAttribute>() != null
                    }).ToList(),
                    @return = new FunctionInfo.ReturnInfo
                    {
                        type = ConvertParameterType(method.ReturnType.Name),
                        description = "" // To-Do: Extend the YarnFunctionAttribute to include return value documentation, and extract it here
                    }
                };

                // Add the function info to our cache
                functionInfoCache.Add(functionInfo);
            }

            // Cache the function info for future use
            return functionInfoCache;
        }

        public static Dictionary<MethodInfo, YarnCommandAttribute> GetAllCommands()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(method => method.GetCustomAttributes(typeof(YarnCommandAttribute), false).Length > 0)
                .ToDictionary(
                    method => method,
                    method => (YarnCommandAttribute)method.GetCustomAttributes(typeof(YarnCommandAttribute), false).First()
                );
        }

        public static Dictionary<MethodInfo, YarnFunctionAttribute> GetAllFunctions()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                .Where(method => method.GetCustomAttributes(typeof(YarnFunctionAttribute), false).Length > 0)
                .ToDictionary(
                    method => method,
                    method => (YarnFunctionAttribute)method.GetCustomAttributes(typeof(YarnFunctionAttribute), false).First()
                );
        }

        public static string ConvertParameterType(string typeName)
        {
            // Converts C# type names to more user-friendly names for display in the editor. This is a simple mapping, and can be extended as needed.
            switch (typeName)
            {
                case "Int32":
                    return "number";
                case "Single":
                    return "number";
                case "Boolean":
                    return "bool";
                case "String":
                    return "string";
                // Add more type conversions as needed
                default:
                    return typeName; // Return the original type name if no conversion is defined
            }
        }

        #endregion

        public struct GeneratorFormater
        {
            public List<CommandInfo> commands;
            public List<FunctionInfo> functions;

            public GeneratorFormater(List<CommandInfo> commands, List<FunctionInfo> functions = null)
            {
                this.commands = commands;
                this.functions = functions ?? new List<FunctionInfo>();
            }

            public static GeneratorFormater Create(List<CommandInfo> commands) => new GeneratorFormater(commands);

            public static GeneratorFormater Create(List<CommandInfo> commands, List<FunctionInfo> functions) => new GeneratorFormater(commands, functions);
        }

        [Serializable]
        public struct CommandInfo
        {
            public string yarnName;
            public string definitionName;
            public string documentation;
            public List<ParameterInfo> parameters;

            public struct ParameterInfo
            {
                public string name;
                public string type;
                public string defaultValue;
                public string documentation;
                public bool isParamsArray;
            }
        }

        [Serializable]
        public struct FunctionInfo
        {
            public string yarnName;
            public string definitionName;
            public string documentation;
            public bool async;
            public bool containsErrors;
            public List<ParameterInfo> parameters;
            public ReturnInfo @return;

            public struct ParameterInfo
            {
                public string name;
                public string type;
                public string defaultValue;
                public string documentation;
                public bool isParamsArray;  
            }

            public struct ReturnInfo
            {
                public string type;
                public string description;
            }
        }
    }
}
