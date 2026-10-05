# Restaurant Application

A short description of the application and the assignment.

## Features

- [Feature or use case]
- [Feature or use case]
- [Feature or use case]

## Project structure

```text
Restaurant/
├── src/
│   ├── Restaurant.Core/       # Classes and business logic
│   └── Restaurant.Console/    # Console application
├── tests/
│   └── Restaurant.Tests/      # xUnit tests
└── Restaurant.sln
```

## Requirements

- [.NET SDK version]
- [Other requirements, if any]

Check your installed .NET version:

```powershell
dotnet --version
```

## Getting started

Clone the repository:

```powershell
git clone [repository-url]
cd [repository-folder]
```

Restore dependencies and build the solution:

```powershell
dotnet restore
dotnet build
```

Run the console application:

```powershell
dotnet run --project src/Restaurant.Console
```

## Running the tests

Run all tests:

```powershell
dotnet test
```

Run only the xUnit test project:

```powershell
dotnet test "..\tests\Restaurant.Tests\Restaurant.Tests.csproj"
```

## UseCase Template

## Key:[Use-case name]

**Actor:** [Actor]  
**Goal:** [What the actor wants to achieve]

**Acceptance criteria:**

- Given [initial situation], when [action], then [expected result].
- Given [initial situation], when [invalid action], then [expected error].


## Business requirements

- Registret skall kunna ta emot och lagra anställda med namn och lön. (via inmatning i konsolen, inget krav på persistent lagring)
- Programmet skall kunna skriva ut registret i en konsol.

## Use cases

### UC1: [Use-case name]

**Actor:** Any employee
**Goal:** Takes care of the employee registry by adding new employees and printing the registry.

**Acceptance criteria:**

-Given the application is running, when the user selects the option to add a new employee, then the application prompts for the employee's name, salary and personal number and country, and adds the employee to the registry.
-Given the application is running, when the user selects the option to print the employee registry, then the application displays a list of all employees with their names, salaries, personal numbers and countries.
-Given the application is running, when the user selects an invalid option, then the application displays an error message and prompts the user to select a valid option.







## Technologies

- C#
- .NET
- xUnit
- Git and GitHub

## Author

Marju Halmann

## License

This project was created as part of Lexicon's .NET Developer course. It is intended for educational purposes and does not have a specific license.