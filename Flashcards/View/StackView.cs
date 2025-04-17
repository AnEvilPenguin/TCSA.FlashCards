using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal static class StackView
{
    internal static void ListStacks(IEnumerable<Stack> stacks)
    {
        AnsiConsole.Clear();
        
        var table = new Table();

        table.AddColumn(new TableColumn("Name").Centered());
        
        foreach (var stack in stacks)
            table.AddRow(stack.Name);
        
        AnsiConsole.Write(table);

        ViewHelpers.WaitForContinue();
    }

    internal static Stack SelectStack(IEnumerable<Stack> stacks, string clarification) => 
        AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title($"Select a [green]stack[/]{clarification}:")
                .UseConverter(b => b.Name)
                .AddChoices(stacks));

    internal static string GetStackName()
    {
        var name = string.Empty;

        do
        {
            var proposed = AnsiConsole.Ask<string>("What is the name of the stack?");

            if (!Stack.IsValidName(proposed, out string errorMessage))
            {
                ViewHelpers.WriteError(errorMessage);
                continue;
            }
                
            name = proposed;
            
        } while (string.IsNullOrWhiteSpace(name));
        
        AnsiConsole.Clear();
        
        return name;
    }

    internal static void ViewStack(StackCardTransferObject transferObject)
    {
        AnsiConsole.Clear();
        
        var table = new Table();

        var titleStyle = new Style()
            .Decoration(Decoration.Bold)
            .Decoration(Decoration.Underline);
        
        table.Title = new TableTitle(transferObject.StackName, titleStyle);
        
        table.AddColumn(new TableColumn("Id").Centered());
        table.AddColumn(new TableColumn("Front").Centered());
        table.AddColumn(new TableColumn("Back").Centered());

        var cards = transferObject.Cards;

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            
            table.AddRow($"{i + 1}", card.Front, card.Back);
        }
        
        AnsiConsole.Write(table);

        ViewHelpers.WaitForContinue();
    }
}