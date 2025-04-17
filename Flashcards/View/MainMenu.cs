using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.View;

internal class MainMenu(SqlServerController database) : AbstractMenu
{
    private readonly StackMenu _stackMenu = new(database);

    private readonly string[] _menuOptions = ["Manage Stacks", "Exit"];

    protected override async Task<int> Run()
    {
        AnsiConsole.Clear();

        var choice = PromptForChoice(_menuOptions);

        switch (choice)
        {
            case "Manage Stacks":
                await _stackMenu.DisplayMenu();
                break;
            
            case "Exit":
                return 0;
        }

        return 1;
    }
}