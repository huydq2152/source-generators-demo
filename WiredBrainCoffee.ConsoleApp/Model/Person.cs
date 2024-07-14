using WiredBrainCoffee.Generators;

namespace WiredBrainCoffee.ConsoleApp.Model;

[GenerateToString]
public partial class Person
{
    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    internal string? InternalProperty { get; set; }
}

public partial class Person
{
    public string? PhoneNumber { get; set; }
}