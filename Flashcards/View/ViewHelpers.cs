using Spectre.Console;

internal static class ViewHelpers
{
    internal static void WriteError(string message)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[red]Error:[/] {message}");
        AnsiConsole.WriteLine();
    }
    
    internal static void WaitForContinue()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}