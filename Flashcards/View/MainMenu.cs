using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.View;

internal class MainMenu(SqlServerController database)
{
    internal int Run()
    {
        var optionList = new List<string> { "Exit" };
        
        while (true)
        {
            AnsiConsole.Clear();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do next?")
                    .AddChoices(optionList));

            switch (choice)
            {
                case "Exit":
                    return 0;
            }
        }
    }
}