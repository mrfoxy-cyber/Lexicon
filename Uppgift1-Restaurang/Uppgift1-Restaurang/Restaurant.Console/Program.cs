using Microsoft.AspNetCore.Identity;
using Restaurant.Core.Services;
using System;
using System.Collections.Generic;

var UserService = new Restaurant.Core.Services.UserRegistry();


string input = "John Doe, 50000, 1234567890, USA";

string[] parts = input.Split(", ");

string name = parts[0];

string salaryString = parts[1];

string personalNumber = parts[2];

string country = parts[3];

string salary = salaryString.Replace(",", ".");
salary = salary.Replace(" ", "");

var user = new Restaurant.Core.Models.User
{
    Name = name,
    Salary = (decimal)double.Parse(salary),
    PersonalNumber = personalNumber,
    Country = country
};

UserRegistry userRegistry = new Restaurant.Core.Services.UserRegistry();

userRegistry.RegisterUser(user);


Console.WriteLine(userRegistry.GetUserByPersonalNumber("1234567890").Name.ToString());