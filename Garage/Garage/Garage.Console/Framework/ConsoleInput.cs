namespace Garage.ConsoleApp.ConsoleFramework;

public static class ConsoleInput
{
    public static int ReadInt(
        string prompt,
        int minimum,
        int maximum)
    {
        while (true)
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out int value) &&
                value >= minimum &&
                value <= maximum)
            {
                return value;
            }

            Console.WriteLine(
                $"Enter a number between {minimum} and {maximum}.");
        }
    }

    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("A value is required.");
        }
    }

    public static double ReadDouble(
        string prompt,
        double minimum,
        double maximum)
    {
        while (true)
        {
            Console.Write(prompt);

            if (double.TryParse(Console.ReadLine(), out double value) &&
                value >= minimum &&
                value <= maximum)
            {
                return value;
            }

            Console.WriteLine(
                $"Enter a number between {minimum} and {maximum}.");
        }
    }

    public static bool ReadBoolean(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (Y/N): ");
            string? input = Console.ReadLine()?.Trim();

            if (input?.Equals(
                    "Y",
                    StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (input?.Equals(
                    "N",
                    StringComparison.OrdinalIgnoreCase) == true)
            {
                return false;
            }

            Console.WriteLine("Enter Y or N.");
        }
    }

    public static TEnum ReadEnum<TEnum>(string prompt)
        where TEnum : struct, Enum
    {
        TEnum[] values = Enum.GetValues<TEnum>();

        Console.WriteLine(prompt);

        for (int index = 0; index < values.Length; index++)
        {
            Console.WriteLine($"{index + 1}. {values[index]}");
        }

        int choice = ReadInt(
            "Choose: ",
            minimum: 1,
            maximum: values.Length);

        return values[choice - 1];
    }

    public static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue.");
        Console.ReadLine();
    }
}
