using WiredBrainCoffee.ConsoleApp.Model;

Console.WriteLine("---------------------------------------");
Console.WriteLine("  Wired Brain Coffee - Person Manager  ");
Console.WriteLine("---------------------------------------");
Console.WriteLine();

var person = new Person
{
    FirstName = "Dang",
    MiddleName = "Quang",
    LastName = "Huy",
    InternalProperty = "InternalProperty",
    PhoneNumber = "123456789"
};

var personAsString = person.ToString();

Console.WriteLine(personAsString);

Console.ReadLine();