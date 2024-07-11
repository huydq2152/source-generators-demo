using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiredBrainCoffee.ConsoleApp.Model
{
    internal class PersonSourceGenerator
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public override string ToString()
        {
            return $"FirstName: {FirstName}; LastName: {LastName}";
        }
    }
}
