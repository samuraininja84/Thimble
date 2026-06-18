using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Yarn.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Thimble.Editor.CommandGenerator;

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
                var commandInfo = CommandInfo.Create(method, attribute);

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
                var functionInfo = FunctionInfo.Create(method, attribute);

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
            return typeName switch
            {
                "Int32" => "number",
                "Single" => "number",
                "Boolean" => "bool",
                "String" => "string",
                _ => typeName,// Return the original type name if no conversion is defined
            };
        }

        #endregion

        public readonly struct GeneratorFormater
        {
            public readonly List<CommandInfo> commands;
            public readonly List<FunctionInfo> functions;

            public GeneratorFormater(List<CommandInfo> commands, List<FunctionInfo> functions = null)
            {
                this.commands = commands;
                this.functions = functions ?? new List<FunctionInfo>();
            }

            public static GeneratorFormater Create(List<CommandInfo> commands) => new GeneratorFormater(commands);

            public static GeneratorFormater Create(List<CommandInfo> commands, List<FunctionInfo> functions) => new GeneratorFormater(commands, functions);
        }

        [Serializable]
        public readonly struct CommandInfo : IEquatable<CommandInfo>
        {
            public readonly string yarnName;
            public readonly string definitionName;
            public readonly string documentation;
            public readonly List<YarnParameterInfo> parameters;

            private readonly int hashCode;

            public CommandInfo(string yarnName, string definitionName, string documentation, List<YarnParameterInfo> parameters)
            {
                // Set the properties of this command based on the provided information.
                this.yarnName = yarnName;
                this.definitionName = definitionName;
                this.documentation = documentation;
                this.parameters = parameters;

                // Calculate the hash code based on the command's properties
                hashCode = HashCode.Combine(yarnName) ^ parameters.GetHashCode();
            }

            public static CommandInfo Create(MethodInfo methodInfo, YarnCommandAttribute attribute)
            {
                return new CommandInfo
                (
                    yarnName: attribute?.Name ?? methodInfo.Name,
                    definitionName: methodInfo.Name,
                    documentation: "", // To-Do: Extend the YarnCommandAttribute to include method documentation, and extract it here
                    parameters: methodInfo.ToYarnParameters()
                );
            }

            public readonly bool Equals(CommandInfo other) => yarnName == other.yarnName && parameters.SequenceEqual(other.parameters);

            public override readonly bool Equals(object obj) => obj is CommandInfo other && Equals(other);

            public override readonly int GetHashCode() => hashCode;
        }

        [Serializable]
        public readonly struct FunctionInfo : IEquatable<FunctionInfo>
        {
            public readonly string yarnName;
            public readonly string definitionName;
            public readonly string documentation;
            public readonly bool async;
            public readonly bool containsErrors;
            public readonly List<YarnParameterInfo> parameters;
            public readonly ReturnInfo @return;

            [JsonIgnore] private readonly int hashCode;

            public FunctionInfo(string yarnName, string definitionName, string documentation, bool async, bool containsErrors, List<YarnParameterInfo> parameters, ReturnInfo @return)
            {
                this.yarnName = yarnName;
                this.definitionName = definitionName;
                this.documentation = documentation;
                this.async = async;
                this.containsErrors = containsErrors;
                this.parameters = parameters;
                this.@return = @return;

                // Calculate the hash code based on the function's properties
                hashCode = HashCode.Combine(yarnName, async, containsErrors) ^ parameters.GetHashCode() ^ @return.GetHashCode();
            }

            public static FunctionInfo Create(MethodInfo methodInfo, YarnFunctionAttribute attribute)
            {
                return new FunctionInfo
                (
                    attribute?.Name ?? methodInfo.Name,
                    methodInfo.Name,
                    "", // To-Do: Extend the YarnFunctionAttribute to include method documentation, and extract it here
                    methodInfo.ReturnType == typeof(System.Threading.Tasks.Task) || (methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(System.Threading.Tasks.Task<>)),
                    false, // To-Do: Implement a way to determine if the function can throw errors and include that information in the JSON
                    methodInfo.ToYarnParameters(),
                    ReturnInfo.Create(methodInfo.ReturnType.Name, string.Empty) // To-Do: Extend the YarnFunctionAttribute to include return value documentation, and extract it here
                );
            }

            public readonly bool Equals(FunctionInfo other) => yarnName == other.yarnName && async == other.async && containsErrors == other.containsErrors && parameters.SequenceEqual(other.parameters) && @return.Equals(other.@return);

            public override readonly bool Equals(object obj) => obj is FunctionInfo other && Equals(other);

            public override readonly int GetHashCode() => hashCode;

            [Serializable]
            public readonly struct ReturnInfo : IEquatable<ReturnInfo>
            {
                public readonly string type;
                public readonly string description;

                [JsonIgnore] private readonly int hashCode;

                public static ReturnInfo Create(string type, string description) => new ReturnInfo(type, description);

                public ReturnInfo(string type, string description)
                {
                    // Set the return type and description for this function.
                    this.type = ConvertParameterType(type);
                    this.description = description;

                    // Calculate the hash code based on the return info's properties
                    hashCode = HashCode.Combine(this.type, this.description);
                }

                public override readonly bool Equals(object obj) => obj is ReturnInfo other && Equals(other);

                public readonly bool Equals(ReturnInfo other) => type == other.type && description == other.description;

                public override readonly int GetHashCode() => hashCode;
            }
        }

        [Serializable]
        public readonly struct YarnParameterInfo : IEquatable<YarnParameterInfo>
        {
            public readonly string name;
            public readonly string type;
            public readonly string defaultValue;
            public readonly string documentation;
            public readonly bool isParamsArray;

            [JsonIgnore] private readonly int hashCode;

            public static YarnParameterInfo Create(string name, string type, string defaultValue, string documentation, bool isParamsArray) => new YarnParameterInfo(name, type, defaultValue, documentation, isParamsArray);

            public static YarnParameterInfo Create(ParameterInfo info) => new YarnParameterInfo(info.Name, info.ParameterType.Name, info.HasDefaultValue ? info.DefaultValue?.ToString() ?? "null" : "None", "", info.GetCustomAttribute<ParamArrayAttribute>() != null);

            public YarnParameterInfo(string name, string type, string defaultValue, string documentation, bool isParamsArray)
            {
                // Set the properties of this parameter based on the provided information.
                this.name = name;
                this.type = ConvertParameterType(type);
                this.defaultValue = defaultValue;
                this.documentation = documentation;
                this.isParamsArray = isParamsArray;

                // Calculate the hash code based on the parameter's properties
                hashCode = HashCode.Combine(name, this.type, defaultValue, documentation, isParamsArray);
            }

            public override readonly bool Equals(object obj) => obj is YarnParameterInfo other && Equals(other);

            public readonly bool Equals(YarnParameterInfo other) => name == other.name && type == other.type && defaultValue == other.defaultValue && isParamsArray == other.isParamsArray;

            public override readonly int GetHashCode() => hashCode;
        }
    }

    internal static class YarnParameterInfoExtensions
    {
        public static List<YarnParameterInfo> ToYarnParameters(this MethodInfo methodInfo)
        {
            // Convert the parameters of a MethodInfo into a list of YarnParameterInfo objects, which can then be serialized to JSON for use in our editor tools.
            var parameters = methodInfo.GetParameters();

            // Initialize a list to hold our YarnParameterInfo objects for this method
            var yarnParameters = new List<YarnParameterInfo>();

            // Loop through each parameter and create a YarnParameterInfo object for it
            foreach (var parameter in parameters)
            {
                // Create a YarnParameterInfo object for this parameter
                var yarnParameter = YarnParameterInfo.Create(parameter);

                // Add the YarnParameterInfo object to our list of parameters for this method
                yarnParameters.Add(yarnParameter);
            }

            // Return the list of YarnParameterInfo objects for this method
            return yarnParameters;
        }

        public static int GetHashCode(this List<YarnParameterInfo> parameters)
        {
            // Calculate a combined hash code for a list of YarnParameterInfo objects by XORing the hash codes of each individual parameter. This allows us to include the parameters in the hash code calculation for our FunctionInfo struct.
            int hashCode = 0;

            // Loop through each parameter and XOR its hash code with the combined hash code
            foreach (var parameter in parameters) hashCode ^= parameter.GetHashCode();

            // Return the combined hash code for the list of parameters
            return hashCode;
        }
    }
}
