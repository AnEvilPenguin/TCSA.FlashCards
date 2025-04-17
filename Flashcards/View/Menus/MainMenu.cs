using Flashcards.Controllers;

namespace Flashcards.View.Menus;

internal class MainMenu(SqlServerController database) : AbstractMenu
{
    private readonly CardMenu _cardMenu = new(database);
    private readonly StackMenu _stackMenu = new(database);
    private readonly SessionMenu _sessionMenu = new(database);
    
    private readonly List<string> _menuOptions = ["Manage Stacks", "Exit"];

    protected override async Task<int> Run()
    {
        await ManageMenuOptions();

        var choice = PromptForChoice(_menuOptions);

        switch (choice)
        {
            case "Manage Stacks":
                await _stackMenu.DisplayMenu();
                break;
            
            case "Manage Cards":
                await _cardMenu.DisplayMenu();
                break;
            
            case "Study Session":
                await _sessionMenu.DisplayMenu();
                break;
            
            case "Exit":
                return 0;
        }

        return 1;
    }

    private async Task ManageMenuOptions()
    {
        var stacksExist = await database.HasStacksAsync();
        var menuContainsCards = _menuOptions.Contains("Manage Cards");
        
        if (!stacksExist && !menuContainsCards)
            return;
        
        if (!stacksExist && menuContainsCards)
        {
            _menuOptions.Remove("Manage Cards");
            return;
        }

        if (stacksExist && !menuContainsCards)
        {
            _menuOptions.Insert(1, "Manage Cards");
        }
        
        var stackWithCardsExist = await database.HasStackWithCardsAsync();
        var menuContainsStudy = _menuOptions.Contains("Study Session");

        if (stackWithCardsExist && !menuContainsStudy)
        {
            _menuOptions.Insert(2, "Study Session");
            return;
        }
        
        if (!stackWithCardsExist && _menuOptions.Contains("Study Session"))
            _menuOptions.Remove("Study Session");
    }
}