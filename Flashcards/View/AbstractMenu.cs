﻿using Spectre.Console;

namespace Flashcards.View;

internal abstract class AbstractMenu
{
    protected void WriteError(string message) =>
        ViewHelpers.WriteError(message);
    
    protected void WaitForContinue() =>
        ViewHelpers.WaitForContinue();
    
    protected string PromptForChoice(IEnumerable<string> options) =>
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do next?")
                .AddChoices(options));

    internal async Task<int> DisplayMenu()
    {
        int output = -1;

        while (output != 0)
        {
            AnsiConsole.Clear();
            
            output = await Run();
        }
        
        return output;
    }

    protected abstract Task<int> Run();
}