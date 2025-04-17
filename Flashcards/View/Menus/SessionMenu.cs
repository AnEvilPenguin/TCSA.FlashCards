using Flashcards.Controllers;
using Flashcards.Model;

namespace Flashcards.View.Menus;

internal class SessionMenu (SqlServerController database) : AbstractMenu 
{
    private readonly StudySessionView _sessionView = new StudySessionView();
    
    private readonly List<string> _menuOptions = 
    [
        "Study Stack",
        "Back"
    ];

    protected override async Task<int> Run()
    {
        var choice = PromptForChoice(_menuOptions);
        
        switch (choice)
        {
            case "Study Stack":
                await StudyStack();
                break;
            
            case "Back":
                return 0;
        }
        
        return 1;
    }

    private async Task StudyStack()
    {
        var validStacks = await database.ListStacksWithCardsAsync();
        
        if (validStacks == null)
            throw new Exception("No valid stacks found");
        
        var selectedStack = StackView.SelectStack(validStacks, " to study from");
        
        var dto = await database.GetStackCardTransferObjectAsync(selectedStack);

        var completedSession = _sessionView.RunTest(dto);
        
        await database.CreateSessionAsync(completedSession);
    }
}