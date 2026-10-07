using System;
using System.Collections.Generic;

namespace DebuggingDemo;

internal class Program
{
    private static void Menu()
    {
        bool exit = false;

        while(!exit)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Single Ticket");
            Console.WriteLine("2. Group Ticket");
            Console.WriteLine("3. Print Third Word");

            Console.Write("Enter your choice: ");
            SelectionTypeResult choice = RequiredString();

            if (!choice.IsValid)
            {
                ShowError(choice.ErrorMessage);
                continue;
            }

            switch (choice.Input)
            {
                case "0":
                    Console.WriteLine("You will be exiting the program.");
                    exit = true;
                    break;
                //Youth and Pensioners
                case "1":
                    ticketPrice();
                    break;
                case "2":
                    GroupTicket();
                    break;
                case "3":
                    PrintThirdWord();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private struct Ticket
    {
        public int Age;
        public double Price;
        public string TicketType;
        public Ticket(int age)
        {
            Age = age;
            Price = age < 20 ? 80 : age > 64 ? 90 : 120;
            TicketType = age < 20 ? "Youth price" : age > 64 ? "Senior citizen price" : "Standard price";
        }
    }

    private static void ticketPrice()
    {
        Console.WriteLine("Ticket Price Calculation");
        Console.Write("Enter your age: ");
        SelectionTypeResult input = RequiredInt();

        if (input.IsValid)
        {
            Ticket ticket = new Ticket(input.Value);
            Console.WriteLine($"Your ticket price is: {ticket.Price} SEK ({ticket.TicketType})");
        }
        else
        {
            ShowError(input.ErrorMessage);
        }
    }

    private static void GroupTicket()
    {
        Console.WriteLine("Group Ticket Price Calculation");
        Console.WriteLine("How Many People?");

        SelectionTypeResult numberOfPeople = RequiredInt();
        List<Ticket> tickets = new List<Ticket>();

        if (numberOfPeople.IsValid)
        {
            for (int i = 1; i <= numberOfPeople.Value; i++)
            {
                Console.Write($"Enter age for person {i}: ");
                SelectionTypeResult ageInput = RequiredInt();

                if (ageInput.IsValid)
                {
                    Ticket ticket = new Ticket(ageInput.Value);
                    tickets.Add(ticket);
                }
                else
                {
                    ShowError($"Person {i}: {ageInput.ErrorMessage}");
                    i--; // Decrement to repeat the iteration for the same person
                }
            }

            double totalPrice = 0;
            foreach (var ticket in tickets)
            {
                totalPrice += ticket.Price;
            }
            Console.WriteLine($"There is {tickets.Count} people in the group. Total price for the group: {totalPrice} SEK");
        }
        else
        {
            ShowError(numberOfPeople.ErrorMessage);
        }
    }

    private static void PrintThirdWord()
    {
        Console.WriteLine("Enter a sentence:");

        SelectionTypeResult input = RequiredString();

        if (input.IsValid)
        {
            string[] words = input.Input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 3)
            {
                Console.WriteLine("The sentence does not contain a third word.");
            }
            else
            {
                Console.WriteLine($"The third word is: {words[2]}");
            }
        }
        else
        {
            ShowError(input.ErrorMessage);
        }
    }

    private struct SelectionTypeResult
    {
        public bool IsValid;
        public string Input;
        public int Value;
        public string ErrorMessage;
    }

    private static SelectionTypeResult RequiredInt()
    {
        SelectionTypeResult input = RequiredString();

        if (!input.IsValid)
        {
            return input;
        }

        if (!int.TryParse(input.Input, out int number))
        {
            return new SelectionTypeResult
            {
                IsValid = false,
                Input = input.Input,
                ErrorMessage = "Please enter a valid whole number."
            };
        }

        return new SelectionTypeResult
        {
            IsValid = true,
            Input = input.Input,
            Value = number,
            ErrorMessage = string.Empty
        };
    }

    private static SelectionTypeResult RequiredString()
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            return new SelectionTypeResult
            {
                IsValid = false,
                Input = string.Empty,
                ErrorMessage = "Input cannot be empty. Please try again."
            };
        }

        return new SelectionTypeResult
        {
            IsValid = true,
            Input = input.Trim(),
            ErrorMessage = string.Empty
        };
    }

    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }

    private static void Main(string[] args)
    {
        Menu();
    }
}

