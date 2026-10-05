# Restaurant employee registry

A .NET console application for registering restaurant employees and their
employment contracts. The project was created as part of Lexicon's .NET
Developer course.

## Features

- Register employees using name, personal number, and country code.
- Require an explicit start date for the employee's initial contract.
- Prevent duplicate employees with the same personal number and country code.
- Give one employee multiple simultaneous employment contracts.
- Store an initial hourly or monthly salary on each contract.
- Update a contract's salary from a specified date without deleting its history.
- Search by name or personal number and list the complete registry.
- Persist application data in an authenticated encrypted file.

## Data model

```text
Employee
└── EmploymentContract (one or more)
    └── SalaryAgreement (one or more, ordered by effective date)
```

Salary belongs to a contract rather than directly to an employee. When a
salary changes, the previous agreement receives an end date and a new agreement
is added. `EffectiveTo` is an exclusive end date.

New contracts are attached using the employee ID. Salary updates use the
contract ID. Personal numbers are therefore not used as mutable references for
either operation. Run `listemployees` to display both IDs.

## Requirements

- .NET 10 SDK

Check the installed version:

```powershell
dotnet --version
```

## Build and run

From the directory containing `Uppgift1-Restaurang.slnx`:

```powershell
dotnet build Uppgift1-Restaurang.slnx
dotnet run --project Restaurant.Console/Restaurant.Console.csproj
```

Available commands:

- `registeremployee`
- `addcontract`
- `updatesalary`
- `listemployees`
- `fetchemployeebyname`
- `fetchemployeebypersonalnumber`
- `help`
- `exit`

Application data is stored in `appdata.bin`. For local development the program
has a fallback password. Set `RESTAURANT_STORAGE_PASSWORD` before starting the
application when a separate password is required.

## Tests

Run the test suite with:

```powershell
dotnet test tests/tests.csproj
```

The tests cover persistence, encrypted loading, validation boundaries,
duplicate prevention, concurrent registration, multiple contracts, and salary
history.

## Original business requirements

- Registret skall kunna ta emot och lagra anställda med namn och lön.
- Programmet skall kunna skriva ut registret i en konsol.
- Persistent lagring är inte ett krav för uppgiften; encrypted persistence is
  an additional feature in this implementation.

## Author

Marju Halmann
