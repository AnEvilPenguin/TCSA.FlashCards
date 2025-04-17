using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal static class StackView
{
    internal static void ListStacks(IEnumerable<Stack>? stacks)
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

    internal static Stack SelectStack(IEnumerable<Stack> stacks, string clarification) => 
        AnsiConsole.Prompt(
            new SelectionPrompt<Stack>()
                .Title($"Select a [green]stack[/]{clarification}:")
                .UseConverter(b => b.Name)
                .AddChoices(stacks));

    private static void WaitForContinue()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}