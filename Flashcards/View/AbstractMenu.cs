using Spectre.Console;

namespace Flashcards.View;

internal abstract class AbstractMenu
{
    protected void WriteError(string message)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[red]Error:[/] {message}");
        AnsiConsole.WriteLine();
    }
}