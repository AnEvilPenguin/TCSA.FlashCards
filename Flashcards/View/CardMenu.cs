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
        await ManageMenuOptions();
        
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
            
            case "List Cards":
                if (_currentStack == null)
                    throw new Exception("No stack selected");
                CardView.ListCards(await database.GetCardsAsync(_currentStack));
                break;
            
            case "Delete Card":
                await DeleteCard();
                break;
            
            case "Back":
                return 0;
        }
        
        return 1;
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

    private async Task ManageMenuOptions()
    {
        if (_currentStack == null)
            return;

        if (_menuOptions[1] != "Create Card")
            _menuOptions.Insert(1, "Create Card");

        var hasCards = await database.HasCardsAsync(_currentStack);

        if (hasCards && _menuOptions.Count <= 3)
        {
            for (var i = 0; i < _stackHasCardOptions.Length; i++)
            {
                _menuOptions.Insert(i + 2, _stackHasCardOptions[i]);
            }
            
            return;
        }
        
        if (!hasCards && _menuOptions.Count > 3)
        {
            foreach (var option in _stackHasCardOptions)
                _menuOptions.Remove(option);
        }
    }

    private async Task DeleteCard()
    {
        if (_currentStack == null)
            throw new Exception("No stack selected");
                
        var card = CardView.SelectCard(await database.GetCardsAsync(_currentStack), " to delete");

        if (!PromptDeleteConfirmation(card.Front))
            return;
        
        await database.DeleteCardAsync(card);
    }
}