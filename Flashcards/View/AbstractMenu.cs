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
    
    protected string PromptForChoice(IEnumerable<string> options) =>
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do next?")
                .AddChoices(options));
}