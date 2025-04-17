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
        await ManageMenuOptions();
        
        var choice = PromptForChoice(_menuOptions);
        
        switch (choice)
        {
            case "Study Stack":
                await StudyStack();
                break;
            
            case "View Sessions":
                await ViewSessions();
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

    private async Task ManageMenuOptions()
    {
        var hasSession = await database.HasSessionAsync();
        var containsView = _menuOptions.Contains("View Sessions");
        
        if (hasSession && !containsView)
            _menuOptions.Insert(1, "View Sessions");
        
        if (!hasSession && containsView)
            _menuOptions.Remove("View Sessions");
    }

    private async Task ViewSessions()
    {
        var sessionDto = await database.ListSessionTransferAsync();
        
        if (sessionDto == null)
            throw new Exception("No valid session transfer");
        
        _sessionView.ViewSessions(sessionDto);
    }
}