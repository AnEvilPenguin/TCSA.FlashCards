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

    private readonly string[] _hasStackOptions = ["List Stacks", "Rename Stack", "Delete Stack"];

    private bool _hasStack = false;

    protected override async Task<int> Run()
    {
        await ManageMenuOptions();
        
        var choice = PromptForChoice(_menuOptions);

        switch (choice)
        {
            case "Create Stack":
                await CreateStack();
                break;
            
            case "List Stacks":
                StackView.ListStacks(await GetStacks());
                break;
            
            case "Rename Stack":
                await RenameStack();
                break;
            
            case "Delete Stack":
                await DeleteStack();
                break;
            
            case "Back":
                return 0;
        }
        
        return 1;
    }
    
    private async Task CreateStack()
    {
        var name = StackView.GetStackName();
        
        AnsiConsole.Clear();
        
        await database.CreateStackAsync(name);
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

    private async Task DeleteStack()
    {
        var stacks = await GetStacks();
        
        var stack = StackView.SelectStack(stacks, " to delete");
        
        if (!await AnsiConsole.ConfirmAsync($"Are you sure you want to delete [green]{stack.Name}[/]? This action [red]cannot be undone[/]."))
            return;
        
        await database.DeleteStackAsync(stack);
    }

    private async Task RenameStack()
    {
        var stacks = await GetStacks();
        
        var stack = StackView.SelectStack(stacks, " to rename");
        
        var name = StackView.GetStackName();
        
        await database.RenameStackAsync(stack, name);
    }

    private async Task<IEnumerable<Stack>> GetStacks()
    {
        var stacks = await database.ListStacksAsync();

        if (stacks == null)
        {
            throw new Exception("No stacks found");
        }
        
        return stacks;
    }
}