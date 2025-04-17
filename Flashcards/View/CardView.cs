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
    
    internal static void ListCards(IEnumerable<Card> cards)
    {
        AnsiConsole.Clear();
        
        var table = new Table();

        table.AddColumn(new TableColumn("Front").Centered());
        table.AddColumn(new TableColumn("Back").Centered());
        
        foreach (var card in cards)
            table.AddRow(card.Front, card.Back);
        
        AnsiConsole.Write(table);

        ViewHelpers.WaitForContinue();
    }
    
    internal static Card SelectCard(IEnumerable<Card> cards, string clarification) => 
        AnsiConsole.Prompt(
            new SelectionPrompt<Card>()
                .Title($"Select a [green]card[/]{clarification}:")
                .UseConverter(b => b.Front)
                .AddChoices(cards));

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