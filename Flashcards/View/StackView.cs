using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal class StackView
{
    internal void ListStacks(IEnumerable<Stack>? stacks)
    {
        AnsiConsole.Clear();
        
        var table = new Table();

        table.AddColumn(new TableColumn("Name").Centered());
        
        if (stacks != null)
            foreach (var stack in stacks)
                table.AddRow(stack.Name);
        
        AnsiConsole.Write(table);

        WaitForContinue();
    }

    internal Stack SelectStack(IEnumerable<Stack> stacks, string clarification) => 
        AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title($"Select a [green]stack[/]{clarification}:")
                .UseConverter(b => b.Name)
                .AddChoices(stacks));

    private void WaitForContinue()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}