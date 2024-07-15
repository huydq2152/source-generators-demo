using System;
using System.Collections.Generic;
using System.Linq;

namespace WiredBrainCoffee.Generators.Model;

public class ClassToGenerate : IEquatable<ClassToGenerate?>
{
    public string NamespaceName { get; }
    public string ClassName { get; }
    public IEnumerable<string> PropertyNames { get; }

    public ClassToGenerate(string namespaceName, string className, IEnumerable<string> propertyNames)
    {
        NamespaceName = namespaceName;
        ClassName = className;
        PropertyNames = propertyNames;
    }

    public bool Equals(ClassToGenerate? other)
    {
        return !ReferenceEquals(null, other) && 
               NamespaceName == other.NamespaceName && 
               ClassName == other.ClassName && 
               PropertyNames.SequenceEqual(other.PropertyNames);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClassToGenerate)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = NamespaceName.GetHashCode();
            hashCode = (hashCode * 397) ^ ClassName.GetHashCode();
            hashCode = (hashCode * 397) ^ PropertyNames.GetHashCode();
            return hashCode;
        }
    }
}