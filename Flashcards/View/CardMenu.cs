namespace Flashcards.View;

internal class CardMenu : AbstractMenu
{
    private readonly List<string> _menuOptions = 
    [
        "Back"
    ];
    
    protected override async Task<int> Run()
    {
        var choice = PromptForChoice(_menuOptions);
        
        switch (choice)
        {
            case "Back":
                return 0;
        }
        
        return 1;
    }
}