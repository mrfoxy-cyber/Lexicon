using Garage.ConsoleApp.Logging;

namespace Garage.ConsoleApp.ConsoleFramework;

public sealed class ConsoleMenu
{
    private readonly MenuOption[] options;

    public string Title { get; }
    public string BackOptionText { get; }

    public ConsoleMenu(
        string title,
        MenuOption[] options,
        string backOptionText = "Back")
    {
        Title = title;
        this.options = options;
        BackOptionText = backOptionText;
    }

    public void Show()
    {

        while (true)
        {
            Display();

            int choice = ConsoleInput.ReadInt(
                "Choose: ",
                minimum: 0,
                maximum: options.Length
                );

            if (choice == 0)
            {
                return;
            }

            Console.Clear();
            MenuOption selectedOption = options[choice - 1];

            // Catch and log unexpected action errors so the menu can continue.
            try
            {
                selectedOption.Execute();

            }
            catch (Exception exception)
            {
                bool wasLogged = ErrorLogger.TryLog(
                    exception,
                    operation: $"{Title} -> {selectedOption.Text}",
                    relatedObject: selectedOption);

                Console.WriteLine(
                    "Something unexpected went wrong.");

                Console.WriteLine(wasLogged
                    ? "The error was written to the log."
                    : "The error could not be written to the log.");

                ConsoleInput.Pause();
            }
        }
    }

    private void Display()
    {
        Console.Clear();

        Console.WriteLine($"=== {Title} ===");
        Console.WriteLine();

        for (int index = 0; index < options.Length; index++)
        {
            Console.WriteLine(
                $"{index + 1}. {options[index].Text}");
        }

        Console.WriteLine($"0. {BackOptionText}");
        Console.WriteLine();
    }
}
