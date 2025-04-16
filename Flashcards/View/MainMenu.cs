using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.View;

internal class MainMenu(SqlServerController database) : AbstractMenu
{
    private readonly StackMenu _stackMenu = new(database);
    
    internal async Task<int> Run()
    {
        var optionList = new List<string> { "Exit", "Create Stack" };

        var stacks = false;
        
        while (true)
        {
            AnsiConsole.Clear();

            if (!stacks && await database.HasStacksAsync())
            {
                optionList.Remove("Create Stack");
                optionList.Add("Manage Stacks");
                
                stacks = true;
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do next?")
                    .AddChoices(optionList));

            switch (choice)
            {
                case "Create Stack":
                    await _stackMenu.CreateStack();
                    break;
                
                case "Exit":
                    return 0;
            }
        }
    }
}