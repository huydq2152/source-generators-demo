using System.Collections.Generic;

namespace WiredBrainCoffee.Generators.Model;

public class ClassToGenerate
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
}