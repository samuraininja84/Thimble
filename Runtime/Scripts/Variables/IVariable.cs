using System;

namespace Thimble
{
    public interface IVariable<T> where T : IEquatable<T> 
    {
        string Name { get; set; }

        T Value { get; set; }

        void SetName(string name);

        void SetValue(T value);

        string GetName();

        T GetValue();

        bool Equals(IVariable<T> other);
    }
}