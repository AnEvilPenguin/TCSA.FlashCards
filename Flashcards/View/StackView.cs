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
}