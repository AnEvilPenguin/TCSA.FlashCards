using Flashcards.Controllers;
using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal class CardMenu(SqlServerController database) : AbstractMenu
{
    private List<string> _menuOptions = 
    [
        "Select Stack",
        "Back"
    ];
    
    private readonly string[] _stackHasCardOptions = ["List Cards", "Edit Card", "Delete Card"];
    
    private Stack? _currentStack;
    
    protected override async Task<int> Run()
    {
        ManageMenuOptions();
        
        if (_currentStack != null)
            AnsiConsole.MarkupLine($"Current stack: [green]{_currentStack.Name}[/]");
        
        var choice = PromptForChoice(_menuOptions);
        
        switch (choice)
        {
            case "Select Stack":
                await SelectStack();
                break;
            
            case "Create Card":
                await CreateCard();
                break;
            
            case "Back":
                return 0;
        }
        
        return 1;
    }

    private async Task CreateCard()
    {
        if (_currentStack == null)
            throw new Exception("No stack selected");
        
        var card = CardView.GetNewCard();
        
        await database.CreateCardAsync(_currentStack, card);
    }

    private async Task SelectStack()
    {
        var stacks = await database.GetStacksAsync();
        
        _currentStack = StackView.SelectStack(stacks, " to manage cards for");
    }

    private void ManageMenuOptions()
    {
        if (_currentStack == null)
            return;

        if (_menuOptions[1] != "Create Card")
            _menuOptions.Insert(1, "Create Card");
    }

    protected override void Initialize()
    {
        _currentStack = null;

        _menuOptions =
        [
            "Select Stack",
            "Back"
        ];
    }
}