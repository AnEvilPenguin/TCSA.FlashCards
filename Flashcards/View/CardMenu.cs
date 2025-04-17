using Flashcards.Controllers;
using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal class CardMenu(SqlServerController database) : AbstractMenu
{
    private readonly List<string> _menuOptions = 
    [
        "Select Stack",
        "Back"
    ];
    
    private Stack? _currentStack;
    
    protected override async Task<int> Run()
    {
        if (_currentStack != null)
            AnsiConsole.MarkupLine($"Current stack: [green]{_currentStack.Name}[/]");
        
        var choice = PromptForChoice(_menuOptions);
        
        switch (choice)
        {
            case "Select Stack":
                await SelectStack();
                break;
            
            case "Back":
                return 0;
        }
        
        return 1;
    }

    private async Task SelectStack()
    {
        var stacks = await database.GetStacksAsync();
        
        _currentStack = StackView.SelectStack(stacks, " to manage cards for");
    }
}