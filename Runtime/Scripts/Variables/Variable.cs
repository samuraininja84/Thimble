using UnityEngine;

namespace Thimble
{
    [System.Serializable]
    public class Variable
    {
        public static string Prefix = "$";

        public string Name;
        public string StringValue;
        public float FloatValue;
        public bool BoolValue;

        public bool UseString = true;
        public bool UseFloat;
        public bool UseBool;

        public Variable() { }

        public Variable(string name, string stringValue)
        {
            Name = name;
            StringValue = stringValue;
            UseString = true;
            UseFloat = false;
            UseBool = false;
        }

        public Variable(string name, float floatValue)
        {
            Name = name;
            FloatValue = floatValue;
            UseString = false;
            UseFloat = true;
            UseBool = false;
        }

        public Variable(string name, bool boolValue)
        {
            Name = name;
            BoolValue = boolValue;
            UseString = false;
            UseFloat = false;
            UseBool = true;
        }

        public void SetValue(string value)
        {
            // If the variable is already set to a float or bool, return, otherwise set the string value
            if (UseFloat || UseBool) return;
            StringValue = value;
            UseString = true;
            UseFloat = false;
            UseBool = false;
        }

        public void SetValue(float value)
        {
            // If the variable is already set to a string or bool, return, otherwise set the float value
            if (UseString || UseBool) return;
            FloatValue = value;
            UseString = false;
            UseFloat = true;
            UseBool = false;
        }

        public void SetValue(bool value)
        {
            // If the variable is already set to a float or string, return, otherwise set the bool value
            if (UseFloat || UseString) return;
            BoolValue = value;
            UseString = false;
            UseFloat = false;
            UseBool = true;
        }

        public string GetName()
        {
            return Prefix + Name;
        }

        public object GetValue()
        {
            // Return the value based on the type of the variable
            if (UseFloat) return FloatValue;
            if (UseString) return StringValue;
            if (UseBool) return BoolValue;
            return null;
        }
    }
}
