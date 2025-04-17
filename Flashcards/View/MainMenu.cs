using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.View;

internal class MainMenu(SqlServerController database) : AbstractMenu
{
    private readonly StackMenu _stackMenu = new(database);

    private readonly string[] _menuOptions = ["Manage Stacks", "Exit"];
    
    internal async Task<int> Run()
    {
        while (true)
        {
            AnsiConsole.Clear();

            var choice = PromptForChoice(_menuOptions);

            switch (choice)
            {
                case "Manage Stacks":
                    await _stackMenu.Run();
                    break;
                
                case "Exit":
                    return 0;
            }
        }
    }
}