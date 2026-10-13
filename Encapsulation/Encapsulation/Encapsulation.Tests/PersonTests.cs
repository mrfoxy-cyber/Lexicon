using System.Globalization;
using System.Reflection;
using Encapsulation.Domain;

namespace Encapsulation.Tests;

public class PersonTests
{
    [Fact]
    public void Constructor_StoresAllValues()
    {
        var person = new Person("Anna", "Andersson", 34, 666.66m);

        Assert.Equal("Anna", person.FirstName);
        Assert.Equal("Andersson", person.LastName);
        Assert.Equal(34, person.Age);
        Assert.Equal(666.66m, person.Salary);
    }

    [Fact]
    public void RequiredFields_ArePrivateAndHaveCorrectTypes()
    {
        var fields = typeof(Person).GetFields(
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        AssertField(fields, "firstName", typeof(string));
        AssertField(fields, "lastName", typeof(string));
        AssertField(fields, "age", typeof(int));
        AssertField(fields, "salary", typeof(decimal));
    }

    [Fact]
    public void Properties_HavePublicGettersAndNonPublicSetters()
    {
        var propertyNames = new[] { "FirstName", "LastName", "Age", "Salary" };

        foreach (var propertyName in propertyNames)
        {
            var property = typeof(Person).GetProperty(propertyName);

            Assert.NotNull(property);
            Assert.True(property.GetMethod?.IsPublic,
                $"{propertyName} must have a public getter.");
            Assert.False(property.SetMethod?.IsPublic,
                $"{propertyName} should not have a public setter.");
        }
    }

    [Fact]
    public void Persons_CanBeSortedByFirstNameAndThenAge()
    {
        var persons = new List<Person>
        {
            new("Anna", "Persson", 65, 2200m),
            new("Anna", "Andersson", 34, 2200m),
            new("Johanna", "Eriksson", 57, 2200m),
            new("Johanna", "Svensson", 27, 2200m),
            new("Erika", "Samuelsson", 35, 2200m)
        };

        var result = persons
            .OrderBy(person => person.FirstName)
            .ThenBy(person => person.Age)
            .Select(person => $"{person.FirstName} {person.Age}")
            .ToArray();

        Assert.Equal(
            ["Anna 34", "Anna 65", "Erika 35", "Johanna 27", "Johanna 57"],
            result);
    }

    [Fact]
    public void IncreaseSalary_GivesFullIncrease_WhenPersonIsAtLeastThirty()
    {
        var person = new Person("Anna", "Hamren", 44, 666.66m);

        person.IncreaseSalary(20m);

        Assert.Equal(799.992m, person.Salary);
    }

    [Fact]
    public void IncreaseSalary_GivesHalfIncrease_WhenPersonIsYoungerThanThirty()
    {
        var person = new Person("Bert", "Lewinsson", 27, 600m);

        person.IncreaseSalary(20m);

        Assert.Equal(660m, person.Salary);
    }

    [Fact]
    public void ToString_FormatsSalaryWithTwoDecimalPlaces()
    {
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var person = new Person("Anna", "Hamren", 44, 666.66m);
            person.IncreaseSalary(20m);

            Assert.Equal(
                "Anna Hamren receives 799.99 dollars.",
                person.ToString());
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-6)]
    public void Constructor_RejectsNonPositiveAge(int age)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Person("Anna", "Ivanov", age, 2200m));

        Assert.Equal(
            "Age cannot be zero or a negative integer!",
            exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("Bo")]
    public void Constructor_RejectsFirstNameShorterThanThreeSymbols(string firstName)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Person(firstName, "Dahl", 57, 3333m));

        Assert.Equal(
            "First name cannot contain fewer than 3 symbols!",
            exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("M")]
    [InlineData("Ma")]
    public void Constructor_RejectsLastNameShorterThanThreeSymbols(string lastName)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Person("Hanna", lastName, 27, 600m));

        Assert.Equal(
            "Last name cannot contain fewer than 3 symbols!",
            exception.Message);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("459.99")]
    public void Constructor_RejectsSalaryBelow460(string salaryText)
    {
        var salary = decimal.Parse(salaryText, CultureInfo.InvariantCulture);

        var exception = Assert.Throws<ArgumentException>(
            () => new Person("Asen", "Holm", 44, salary));

        Assert.Equal(
            "Salary cannot be less than 460 dollars!",
            exception.Message);
    }

    [Fact]
    public void Constructor_AcceptsBoundaryValues()
    {
        var person = new Person("Ann", "Lee", 1, 460m);

        Assert.Equal("Ann", person.FirstName);
        Assert.Equal("Lee", person.LastName);
        Assert.Equal(1, person.Age);
        Assert.Equal(460m, person.Salary);
    }

    private static void AssertField(
        IEnumerable<FieldInfo> fields,
        string name,
        Type expectedType)
    {
        var field = fields.SingleOrDefault(candidate =>
            candidate.Name.TrimStart('_').Equals(
                name,
                StringComparison.OrdinalIgnoreCase));

        Assert.NotNull(field);
        Assert.True(field.IsPrivate, $"The field '{name}' must be private.");
        Assert.Equal(expectedType, field.FieldType);
    }
}
