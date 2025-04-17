﻿using Flashcards.Controllers;
using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal class StackMenu (SqlServerController database) : AbstractMenu
{
    private readonly List<string> _menuOptions =
    [
        "Create Stack",
        "Back"
    ];

    private readonly string[] _hasStackOptions = ["List Stacks", "Delete Stack"];

    private bool _hasStack = false;

    internal async Task Run()
    {
        while (true)
        {
            AnsiConsole.Clear();

            await ManageMenuOptions();
            
            var choice = PromptForChoice(_menuOptions);

            switch (choice)
            {
                case "Create Stack":
                    await CreateStack();
                    break;
                
                case "Back":
                    return;
            }
        }
    }
    
    private async Task CreateStack()
    {
        var name = string.Empty;

        do
        {
            var proposed = AnsiConsole.Ask<string>("What is the name of the stack?");

            if (!Stack.IsValidName(proposed, out string errorMessage))
            {
                WriteError(errorMessage);
                continue;
            }
                
            name = proposed;
            
        } while (string.IsNullOrWhiteSpace(name));
        
        AnsiConsole.Clear();
        
        await database.CreateStack(name);
    }

    private async Task ManageMenuOptions()
    {
        var stacksExist = await database.HasStacksAsync(); 
        
        switch (stacksExist)
        {
            case true when !_hasStack:
            {
                for (var i = 0; i < _hasStackOptions.Length; i++) 
                    _menuOptions.Insert(i + 1, _hasStackOptions[i]);
            
                _hasStack = true;
                return;
            }
            case true:
                return;
        }

        foreach (var stackOption in _hasStackOptions)
            _menuOptions.Remove(stackOption);
        
        _hasStack = false;

    }
}