using System;
using UnityEngine;

namespace Thimble
{
    [Serializable]
    public struct FloatVariable : IVariable<float>
    {
        [SerializeField] private string name;
        [SerializeField] private float value;

        public string Name { get => name; set => name = value; }

        public float Value { get => value; set => this.value = value; }

        #region Operators

        #endregion

        public FloatVariable(string name, float value)
        {
            this.name = name;
            this.value = value;
        }

        public static FloatVariable Default => new FloatVariable(VariableHandler.MissingVariableName, 0f);

        public static FloatVariable Create(string name, float value) => new FloatVariable(name, value);

        public void SetName(string name) => Name = name;

        public void SetValue(float value)
        {
            // If the variable name is empty, return without setting the value to avoid creating an entry in VariableData with an empty name
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return;

            // Set the variable value in the VariableData using the variable's name
            VariableData.Instance.SetVariable(Name, value);

            // Update the value in case it was changed in the VariableData
            Value = value;
        }

        public void SetValue(IVariable<float> variable) => Value = variable.Value;

        public string GetName() => VariableHandler.Prefix + Name;

        public float GetValue()
        {
            // If the variable name is empty, return the current value without trying to get it from VariableData
            if (string.IsNullOrEmpty(Name) || string.Equals(Name, VariableHandler.MissingVariableName, StringComparison.OrdinalIgnoreCase)) return Value;

            // Try to get the variable value from the VariableData using the variable's name
            VariableData.Instance.GetVariable(Name, out float value);

            // Update the value in case it was changed in the VariableData
            return Value = value;
        }

        public bool Equals(IVariable<float> other)
        {
            // Compare names using case-insensitive comparison
            bool nameEquals = string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

            // If names are not equal, variables are not equal
            return nameEquals && other.Value.Approximately(Value);
        }

        public bool Equals(FloatVariable other) => Equals((IVariable<float>)other);

        public bool Equals(float other) => other.Approximately(Value);

        public override bool Equals(object obj)
        {
            if (obj is FloatVariable variable)
                return Equals(variable);
            if (obj is float f)
                return Equals(f);
            return false;
        }

        public override int GetHashCode() => (Name, Value).GetHashCode();
    }
}