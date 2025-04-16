using Flashcards.Controllers;
using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal class StackMenu (SqlServerController database) : AbstractMenu
{
    
    internal async Task CreateStack()
    {
        var name = string.Empty;

        do
        {
            var proposed = AnsiConsole.Ask<string>("What is the name of the stack?");

            if (!Stack.IsValidName(proposed, out string errorMessage))
            {
                WriteError(errorMessage);
                continue;
            }
                
            name = proposed;
            
        } while (string.IsNullOrWhiteSpace(name));
        
        AnsiConsole.Clear();
        
        await database.CreateStack(name);
    }
}