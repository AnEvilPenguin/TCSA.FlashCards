using Flashcards.Controllers;

namespace Flashcards.View;

internal class MainMenu(SqlServerController database) : AbstractMenu
{
    private readonly StackMenu _stackMenu = new(database);

    private readonly List<string> _menuOptions = ["Manage Stacks", "Exit"];
    
    private bool _hasStack;

    protected override async Task<int> Run()
    {
        await ManageMenuOptions();

        var choice = PromptForChoice(_menuOptions);

        switch (choice)
        {
            case "Manage Stacks":
                await _stackMenu.DisplayMenu();
                break;
            
            case "Exit":
                return 0;
        }

        return 1;
    }

    private async Task ManageMenuOptions()
    {
        var stacksExist = await database.HasStacksAsync(); 
        
        switch (stacksExist)
        {
            case true when !_hasStack:
            {
                _menuOptions.Insert(1, "Manage Cards");
            
                _hasStack = true;
                return;
            }
            case true:
                return;
        }

        _menuOptions.Remove("Manage Cards");
        
        _hasStack = false;
    }
}