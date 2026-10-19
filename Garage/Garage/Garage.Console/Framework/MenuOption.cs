namespace Garage.ConsoleApp.ConsoleFramework;

public sealed class MenuOption
{
    private readonly Action action;
    private readonly Func<string> textProvider;

    public string Text => textProvider();

    public MenuOption(string text, Action action)
        : this(() => text, action)
    {
    }

    public MenuOption(Func<string> textProvider, Action action)
    {
        this.textProvider = textProvider;
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
