namespace Garage.ConsoleApp.ConsoleFramework;

public sealed class MenuOption
{
    private readonly Action action;

    public string Text { get; }

    public MenuOption(string text, Action action)
    {
        Text = text;
        this.action = action;
    }

    public MenuOption(string text, ConsoleMenu submenu)
        : this(text, submenu.Show)
    {
    }

    public void Execute()
    {
        action();
    }
}