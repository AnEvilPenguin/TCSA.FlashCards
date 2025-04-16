using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.View;

internal class StackMenu (SqlServerController database)
{
    internal async Task CreateStack()
    {
        var name = string.Empty;

        do
        {
            var proposed = AnsiConsole.Ask<string>("What is the name of the stack?");

            if (proposed.Length > 100)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[red]Error:[/] Must be less than 100 characters!");
                AnsiConsole.WriteLine();
                
                continue;
            }
                
            name = proposed;
            
        } while (string.IsNullOrWhiteSpace(name));
        
        AnsiConsole.Clear();
        
        await database.CreateStack(name);
    }
}