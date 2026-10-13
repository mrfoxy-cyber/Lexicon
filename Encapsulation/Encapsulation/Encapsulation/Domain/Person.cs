using System.Globalization;

namespace Encapsulation.Domain
{
    internal class Person
    {
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private int age;
        private decimal salary;
        private string? errorMessage;

        public Person(string firstName, string lastName, int age)
        {
            try
            {
                FirstName = firstName;
                LastName = lastName;
                Age = age;
            }
            catch (ArgumentException ex)
            {
                errorMessage = ex.Message;
            }
        }

        public Person(string firstName, string lastName, int age, decimal salary)
            : this(firstName, lastName, age)
        {
            if (errorMessage is not null)
            {
                return;
            }

            try
            {
                Salary = salary;
            }
            catch (ArgumentException ex)
            {
                errorMessage = ex.Message;
            }
        }

        public string FirstName
        {
            get => firstName;
            private set
            {
                if (value is null || value.Length < 3)
                {
                    throw new ArgumentException(
                        "First name cannot contain fewer than 3 symbols!");
                }

                firstName = value;
            }
        }

        public string LastName
        {
            get => lastName;
            private set
            {
                if (value is null || value.Length < 3)
                {
                    throw new ArgumentException(
                        "Last name cannot contain fewer than 3 symbols!");
                }

                lastName = value;
            }
        }

        public int Age
        {
            get => age;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "Age cannot be zero or a negative integer!");
                }

                age = value;
            }
        }

        public decimal Salary
        {
            get => salary;
            private set
            {
                if (value < 460m)
                {
                    throw new ArgumentException(
                        "Salary cannot be less than 460 dollars!");
                }

                salary = value;
            }
        }

        public decimal IncreaseSalary(decimal percentage)
        {
            if (errorMessage is not null)
            {
                return salary;
            }

            var appliedPercentage = age < 30
                ? percentage / 2m
                : percentage;

            salary += salary * appliedPercentage / 100m;
            return salary;
        }

        public override string ToString()
        {
            if (errorMessage is not null)
            {
                return errorMessage;
            }

            if (salary == 0m)
            {
                return $"{firstName} {lastName} is {age} years old.";
            }

            var formattedSalary = salary.ToString(
                "F2",
                CultureInfo.InvariantCulture);

            return $"{firstName} {lastName} receives {formattedSalary} dollars.";
        }
    }
}
