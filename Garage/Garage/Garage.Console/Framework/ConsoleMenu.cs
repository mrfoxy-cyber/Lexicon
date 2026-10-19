namespace Garage.ConsoleApp.ConsoleFramework;

public sealed class ConsoleMenu
{
    private readonly IReadOnlyList<MenuOption> options;

    public string Title { get; }
    public string BackOptionText { get; }

    public ConsoleMenu(
        string title,
        IEnumerable<MenuOption> options,
        string backOptionText = "Back")
    {
        Title = title;
        this.options = options.ToList();
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
                maximum: options.Count);

            if (choice == 0)
            {
                return;
            }

            Console.Clear();
            options[choice - 1].Execute();
        }
    }

    private void Display()
    {
        Console.Clear();

        Console.WriteLine($"=== {Title} ===");
        Console.WriteLine();

        for (int index = 0; index < options.Count; index++)
        {
            Console.WriteLine(
                $"{index + 1}. {options[index].Text}");
        }

        Console.WriteLine($"0. {BackOptionText}");
        Console.WriteLine();
    }
}