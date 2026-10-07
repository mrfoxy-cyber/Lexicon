using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;

namespace DebuggingDemo;

internal class Program
{
    private static void Menu(string[] args )
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
            string choice = Console.ReadLine();
            switch (choice)
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
            TicketType = age < 20 ? "UngdomodmsPris" : age > 64 ? "Pensionärpris" : "Standardpris";
        }
    }

    private static void ticketPrice()
    {
        Console.WriteLine("Ticket Price Calculation");
        Console.Write("Enter your age: ");
        ReadResult input = RequiredInt();

        if (input.IsValid)
        {
            Ticket ticket = new Ticket(input.Value);
            Console.WriteLine($"Your ticket price is: {ticket.Price} SEK ({ticket.TicketType})");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid age.");
        }
    }

    private static void GroupTicket()
    {
        Console.WriteLine("Group Ticket Price Calculation");
        Console.WriteLine("How Many People?");

        ReadResult numberOfPeople = RequiredInt();
        List<Ticket> tickets = new List<Ticket>();

        if (numberOfPeople.IsValid)
        {
            for (int i = 1; i <= numberOfPeople.Value; i++)
            {
                Console.Write($"Enter age for person {i}: ");
                ReadResult ageInput = RequiredInt();

                if (ageInput.IsValid)
                {
                    Ticket ticket = new Ticket(ageInput.Value);
                    tickets.Add(ticket);
                }
                else
                {
                    Console.WriteLine($"Invalid input for person {i}. Please enter a valid age.");
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
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private static void PrintThirdWord()
    {
        Console.WriteLine("Enter a sentence:");

        ReadResult input = RequiredString();

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
            Console.WriteLine("Please enter a valid sentence.");
        }
    }

    struct ReadResult
    {
        public int Value;
        public string Input;
        public bool IsValid;
    }

    private static ReadResult RequiredInt()
    {

        ReadResult input = RequiredString();

        if (!input.IsValid)
        {
            Console.WriteLine("Please enter a valid number and try again.");

            return new ReadResult { IsValid = false };

        } else if (!int.TryParse(input.Input, out int groupSize))
        {
            Console.WriteLine("Please enter a valid number and try again.");
            return new ReadResult { IsValid = false };
        }
        else
        {
            return new ReadResult { Value = groupSize, IsValid = true };
        }
    }

    private static ReadResult RequiredString()
    {
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Please enter a valid number and try again.");

            return new ReadResult { IsValid = false };
        } else
        {
            return new ReadResult { Input = input, IsValid = true };
        }
    }



    private static void Main(string[] args)
    {
        Menu(args);
    }
}
