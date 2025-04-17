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

    private void WaitForContinue()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}