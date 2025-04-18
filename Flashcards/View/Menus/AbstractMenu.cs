﻿using Spectre.Console;

namespace Flashcards.View.Menus;

internal abstract class AbstractMenu
{
    internal async Task<int> DisplayMenu()
    {
        Initialize();
        
        int output = -1;

        while (output != 0)
        {
            AnsiConsole.Clear();
            
            output = await Run();
        }
        
        return output;
    }
    
    protected void WriteError(string message) =>
        ViewHelpers.WriteError(message);
    
    protected void WaitForContinue() =>
        ViewHelpers.WaitForContinue();
    
    protected string PromptForChoice(IEnumerable<string> options) =>
        AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What would you like to do next?")
                .AddChoices(options));

    protected abstract Task<int> Run();
    
    protected virtual void Initialize()
    {}

    protected bool PromptDeleteConfirmation(string name) =>
        AnsiConsole.Confirm($"Are you sure you want to delete [green]{name}[/]? This action [red]cannot be undone[/].");
}