using System;
using System.Collections.Generic;
using System.Text;

namespace Encapsulation.Domain
{
    internal class Person
    {
        private string? firstName;
        private string? lastName;
        private int? age;

        private decimal? salary;

        public ArgumentException ArgumentException { get; private set; } = null;

        public decimal? IncreaseSalary(int percentage)
        {
            if (age < 30)
            {
                salary += salary * (decimal)percentage / 200;
            }
            else
            {
                salary += salary * (decimal)percentage / 100;
            }
            return salary;
        }

        public decimal? IncreaseSalary(decimal percentage)
        {
            if (age < 30)
            {
                salary += salary *  percentage / 200;
            }
            else
            {
                salary += salary * percentage / 100;
            }
            return salary;
        }

        bool atleast3Char(string s, string paramName)
        {
            if (s.Length < 3)
            {
                this.ArgumentException = new ArgumentException($"{paramName} must be at least 3 symbols");
            }
            return true;
        }

        public string? FirstName
        {
            get { return firstName; }
            set { atleast3Char(value, "FirstName"); firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { atleast3Char(value, "LastName"); lastName = value; }
        }

        public int? Age
        {
            get { return age; }
            set {
                if (value <= 0)
                {
                    this.ArgumentException = new ArgumentException("Age cannot be Zero or negative.");
                }
                age = value; 
            }
        }

        public decimal? Salary
        {
            get { return salary; }
            set
            {
                if (value < 460)
                {
                    this.ArgumentException = new ArgumentException("Salary cannot be less than 460 dollars.");
                }
                salary = value;
            }
        }

        public Person(string firstName, string lastName, int age)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age     = age;
        }

        public Person(string firstName, string lastName, int age, decimal salary)
            : this(firstName, lastName, age)
        {
            this.Salary = salary;
        }

        public string ToStringOld()
        {
            return $"{firstName} {lastName} is {age} years old.";
        }

        public override string ToString()
        {
            if (this.ArgumentException != null)
            {
                return this.ArgumentException.Message;
            }
            return $"{firstName} {lastName} received a {salary} dollars";
        }

    }
}
