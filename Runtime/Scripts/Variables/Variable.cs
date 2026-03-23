using System;
using UnityEngine;

namespace Thimble
{
    [Serializable]
    public class Variable : IEquatable<Variable>
    {
        /// <summary>
        /// Represents the prefix used to denote special Yarn Variables.
        /// </summary>
        public static string Prefix = "$";

        /// <summary>
        /// Represents the type of the variable.
        /// </summary>
        /// <remarks>This field specifies the classification or category of the variable. 
        /// It can be used to determine the behavior or characteristics associated with the variable.
        /// </remarks>
        public VariableType Type;

        /// <summary>
        /// Represents the name of the variable.
        /// </summary>
        public string Name;

        // Storage for different types of values
        public string stringValue;
        public float floatValue;
        public bool boolValue;

        // Helper properties to check the type of the value
        public bool IsString => Type == VariableType.String;
        public bool IsFloat => Type == VariableType.Float;
        public bool IsBool => Type == VariableType.Bool;

        // Implicit conversion operators to convert Variable to different types
        public static implicit operator string(Variable value) => value.ConvertValue<string>();
        public static implicit operator float(Variable value) => value.ConvertValue<float>();
        public static implicit operator bool(Variable value) => value.ConvertValue<bool>();

        // Equality operator to compare Variable instances
        public static bool operator ==(Variable left, Variable right) => left.Equals(right);
        public static bool operator !=(Variable left, Variable right) => !left.Equals(right);

        // Equality operator to compare Variable with other types
        public static bool operator ==(Variable left, string right) => left.IsString && left.stringValue.Equals(right, StringComparison.Ordinal);
        public static bool operator !=(Variable left, string right) => !(left == right);
        public static bool operator ==(Variable left, float right) => left.IsFloat && Mathf.Approximately(left.floatValue, right);
        public static bool operator !=(Variable left, float right) => !(left == right);
        public static bool operator ==(Variable left, bool right) => left.IsBool && left.boolValue == right;
        public static bool operator !=(Variable left, bool right) => !(left == right);

        // Helper methods for safe type conversions of the value types without the cost of boxing
        T AsString<T>(string value) => typeof(T) == typeof(string) && value is T correctType ? correctType : default;
        T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;
        T AsBool<T>(bool value) => typeof(T) == typeof(bool) && value is T correctType ? correctType : default;

        public Variable(string name, string stringValue)
        {
            Name = name.Replace(Prefix, string.Empty);
            this.stringValue = stringValue;
            this.floatValue = 0f; 
            this.boolValue = false;
            Type = VariableType.String;
        }

        public Variable(string name, float floatValue)
        {
            Name = name.Replace(Prefix, string.Empty);
            this.stringValue = string.Empty;
            this.floatValue = floatValue;
            this.boolValue = false;
            Type = VariableType.Float;
        }

        public Variable(string name, bool boolValue)
        {
            Name = name.Replace(Prefix, string.Empty);
            this.stringValue = string.Empty;
            this.floatValue = 0f;
            this.boolValue = boolValue;
            Type = VariableType.Bool;
        }

        public void SetValue(string value)
        {
            // If the variable is already set to a float or bool, return, otherwise set the string value
            if (IsFloat || IsBool)
            {
                Debug.LogWarning($"Variable '{Name}' is already set to a different type. Cannot set to bool.");
                return;
            }

            // Set the string value
            stringValue = value;
        }

        public void SetValue(float value)
        {
            // If the variable is already set to a string or bool, return, otherwise set the float value
            if (IsString || IsBool)
            {
                Debug.LogWarning($"Variable '{Name}' is already set to a different type. Cannot set to bool.");
                return;
            }

            // Set the float value
            floatValue = value;
        }

        public void SetValue(bool value)
        {
            // If the variable is already set to a float or string, return, otherwise set the bool value
            if (IsFloat || IsString)
            {
                Debug.LogWarning($"Variable '{Name}' is already set to a different type. Cannot set to bool.");
                return;
            }

            // Set the bool value
            boolValue = value;
        }

        /// <summary>
        /// Retrieves the full name by combining the prefix and name.
        /// </summary>
        /// <returns>A string representing the concatenated prefix and name.  
        /// If either the prefix or name is empty, the result will reflect the non-empty value or be empty if both are empty.
        /// </returns>
        public string GetName() => Prefix + Name;

        /// <summary>
        /// Retrieves the value of the variable based on its type.
        /// </summary>
        /// <remarks>The returned value corresponds to the current <see cref="VariableType"/> of the
        /// variable. If the type is unsupported, an exception is thrown.</remarks>
        /// <returns>The value of the variable as an <see cref="object"/>. The type of the returned value depends on the  <see
        /// cref="VariableType"/>: <list type="bullet"> <item><description><see langword="string"/> if the type is <see
        /// cref="VariableType.String"/>.</description></item> <item><description><see langword="float"/> if the type is
        /// <see cref="VariableType.Float"/>.</description></item> <item><description><see langword="bool"/> if the type
        /// is <see cref="VariableType.Bool"/>.</description></item> </list></returns>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="VariableType"/> is unsupported.</exception>
        public object GetValue()
        {
            // Return the value based on the type of the variable
            switch (Type)
            {
                case VariableType.String:
                    return stringValue;
                case VariableType.Float:
                    return floatValue;
                case VariableType.Bool:
                    return boolValue;
                default:
                    throw new InvalidOperationException($"Unsupported VariableType: {Type}");
            }
        }

        /// <summary>
        /// Converts the current value to the specified type.
        /// </summary>
        /// <remarks>This method supports conversion to common types such as <see langword="string"/>,
        /// <see langword="float"/>,  and <see langword="bool"/> based on the underlying type of the value. If the type
        /// is <see langword="object"/>,  the value is cast directly.</remarks>
        /// <typeparam name="T">The target type to convert the value to.</typeparam>
        /// <returns>The value converted to the specified type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">Thrown if the current value cannot be converted to the specified type <typeparamref name="T"/>.</exception>
        public T ConvertValue<T>()
        {
            if (typeof(T) == typeof(object)) return CastToObject<T>();
            return Type switch
            {
                VariableType.String => AsString<T>(stringValue),
                VariableType.Float => AsFloat<T>(floatValue),
                VariableType.Bool => AsBool<T>(boolValue),
                _ => throw new InvalidCastException($"Cannot convert AnyValue of type {Type} to {typeof(T).Name}")
            };
        }

        /// <summary>
        /// Determines the corresponding <see cref="VariableType"/> for a given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to evaluate. Must represent a supported type.</param>
        /// <returns>A <see cref="VariableType"/> value that represents the type of the variable. For example, <see
        /// cref="VariableType.String"/> for <see cref="string"/>,  <see cref="VariableType.Float"/> for <see
        /// cref="float"/> or <see cref="int"/>,  and <see cref="VariableType.Bool"/> for <see cref="bool"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown if the provided <paramref name="type"/> is not supported.</exception>
        public static VariableType VariableTypeOf(Type type)
        {
            return type switch
            {
                // Note: int is treated as float for compatibility
                _ when type == typeof(string) => VariableType.String,
                _ when type == typeof(float) => VariableType.Float,
                _ when type == typeof(int) => VariableType.Float,
                _ when type == typeof(bool) => VariableType.Bool,
                _ => throw new NotSupportedException($"Unsupported type: {type}")
            };
        }

        /// <summary>
        /// Maps a <see cref="VariableType"/> enumeration value to its corresponding .NET <see cref="Type"/>.
        /// </summary>
        /// <remarks>This method provides a way to retrieve the .NET type associated with a given <see
        /// cref="VariableType"/>. For example, <see cref="VariableType.String"/> maps to <see cref="string"/>, <see
        /// cref="VariableType.Float"/>  maps to <see cref="float"/>, and <see cref="VariableType.Bool"/> maps to <see
        /// cref="bool"/>.</remarks>
        /// <param name="variableType">The <see cref="VariableType"/> to convert to a <see cref="Type"/>.</param>
        /// <returns>The .NET <see cref="Type"/> that corresponds to the specified <paramref name="variableType"/>.</returns>
        /// <exception cref="NotSupportedException">Thrown if the specified <paramref name="variableType"/> is not supported.</exception>
        public static Type TypeOf(VariableType variableType)
        {
            return variableType switch
            {
                VariableType.String => typeof(string),
                VariableType.Float => typeof(float),
                VariableType.Bool => typeof(bool),
                _ => throw new NotSupportedException($"Unsupported ValueType: {variableType}")
            };
        }

        /// <summary>
        /// Casts the current value to the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>This method supports casting to common types such as <see langword="int"/>, 
        /// <see langword="float"/>, <see langword="bool"/>, <see langword="string"/>, and <c>Vector3</c>. 
        /// Attempting to cast to an unsupported type or mismatched type will result in an <see cref="InvalidCastException"/>.
        /// </remarks>
        /// <typeparam name="T">The target type to cast the value to.
        /// </typeparam>
        /// <returns>The value cast to the specified type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">Thrown if the current value type cannot be cast to the specified type <typeparamref name="T"/>.</exception>
        private T CastToObject<T>()
        {
            return Type switch
            {
                VariableType.String => (T)(object)stringValue,
                VariableType.Float => (T)(object)floatValue,
                VariableType.Bool => (T)(object)boolValue,
                _ => throw new InvalidCastException($"Cannot convert AnyValue of type {Type} to {typeof(T).Name}")
            };
        }

        /// <summary>
        /// Determines whether the current <see cref="Variable"/> instance is equal to another <see cref="Variable"/>
        /// instance.
        /// </summary>
        /// <remarks>Equality is determined by comparing the <see cref="Variable.Type"/> property and the
        /// corresponding value based on the type. For <see cref="VariableType.String"/>, a case-sensitive ordinal
        /// comparison is performed. For <see cref="VariableType.Float"/>, values are compared using approximate
        /// equality. For <see cref="VariableType.Bool"/>, values are compared directly.</remarks>
        /// <param name="other">The <see cref="Variable"/> instance to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the current instance is equal to the specified <paramref name="other"/> instance;
        /// otherwise, <see langword="false"/>.</returns>
        public bool Equals(Variable other)
        {
            // Check if types are the same
            if (Type != other.Type) return false;

            // Compare values based on type
            switch (Type)
            {
                case VariableType.String:
                    return stringValue.Equals(other.stringValue, StringComparison.Ordinal);
                case VariableType.Float:
                    return Mathf.Approximately(this.floatValue, other.floatValue);
                case VariableType.Bool:
                    return boolValue == other.boolValue;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <remarks>This method supports type-specific comparisons: <list type="bullet">
        /// <item><description>If <paramref name="obj"/> is of type <see cref="Variable"/>, the comparison is performed
        /// using the <see cref="Equals(Variable)"/> method.</description></item> <item><description>If <paramref
        /// name="obj"/> is a <see cref="string"/>, the comparison is performed using the string representation of the
        /// current instance and <paramref name="obj"/>.</description></item> <item><description>If <paramref
        /// name="obj"/> is a <see cref="float"/>, the comparison uses a tolerance to account for floating-point
        /// precision.</description></item> <item><description>If <paramref name="obj"/> is a <see cref="bool"/>, the
        /// comparison is performed using the boolean value of the current instance.</description></item> </list> If
        /// <paramref name="obj"/> is not of a supported type, the method returns <see langword="false"/>.</remarks>
        /// <param name="obj">The object to compare with the current instance. Supported types include <see cref="Variable"/>, <see
        /// cref="string"/>, <see cref="float"/>, and <see cref="bool"/>.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current instance; otherwise, <see
        /// langword="false"/>. Equality is determined based on the type of the object and its value.</returns>
        public override bool Equals(object obj)
        {
            // If the object is of type Variable, compare values
            if (obj is Variable other) return this.Equals(other);

            // If the object is a string, compare string representations
            if (obj is string str) stringValue.Equals(str, StringComparison.Ordinal);

            // If the object is a float, compare float values with a tolerance
            if (obj is float floatValue) return Mathf.Approximately(this.floatValue, floatValue);

            // If the object is a bool, compare boolean values
            if (obj is bool boolValue) return this.boolValue == boolValue;

            // If the object is not of a supported type, return false
            return false;
        }

        /// <summary>
        /// Generates a hash code for the current instance based on its type and value components.
        /// </summary>
        /// <remarks>The hash code is computed using the combined hash codes of the <see cref="Type"/>, 
        /// <c>stringValue</c>, <c>floatValue</c>, and <c>boolValue</c> fields. This ensures that  instances with
        /// identical values produce the same hash code.</remarks>
        /// <returns>An integer representing the hash code for the current instance.</returns>
        public override int GetHashCode() => (Type, stringValue, floatValue, boolValue).GetHashCode();
    }

    public enum VariableType 
    { 
        String,
        Float, 
        Bool
    }
}
