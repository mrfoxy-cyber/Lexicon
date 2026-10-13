using Encapsulation.Domain;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Encapsulation.Tests")]

class Program
{
    public static void Run()
    {
        const int lines = 5;

        var persons = new List<Person>();

        for (int i = 0; i < lines; i++)
        {
            var cmdArgs = Console.ReadLine()!.Split();

            var person = new Person(
                cmdArgs[0],
                cmdArgs[1],
                int.Parse(cmdArgs[2]),
                decimal.Parse(cmdArgs[3], CultureInfo.InvariantCulture));

            persons.Add(person);
        }

        var bonus = decimal.Parse(
            Console.ReadLine()!,
            CultureInfo.InvariantCulture);

        persons.ForEach(person => person.IncreaseSalary(bonus));

        persons.ForEach(person => Console.WriteLine(person));
    }

    public static void Main()
    {
        Run();
    }
}
