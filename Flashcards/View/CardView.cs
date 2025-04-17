using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal static class CardView
{
    internal static Card GetNewCard() =>
        new Card()
        {
            Front = GetCardString("What should be on the [green]front[/] of the card?", "Front"),
            Back = GetCardString("What should be on the [green]back[/] of the card?", "Back"),
        };

    private static string GetCardString(string prompt, string type)
    {
        var output = string.Empty;

        do
        {
            var proposed = AnsiConsole.Ask<string>(prompt);

            if (!Card.IsValidString(proposed, type, out string errorMessage))
            {
                ViewHelpers.WriteError(errorMessage);
                continue;
            }
                
            output = proposed;
            
        } while (string.IsNullOrWhiteSpace(output));
        
        return output;
    }
}