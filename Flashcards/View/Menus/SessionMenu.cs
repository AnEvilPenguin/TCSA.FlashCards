using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.View.Menus;

internal class SessionMenu (SqlServerController database) : AbstractMenu 
{
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
            
            case "View Average Report":
                await AverageReport();
                break;
            
            case "View Session Report":
                await SessionReport();
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

        var completedSession = StudySessionView.RunTest(dto);
        
        await database.CreateSessionAsync(completedSession);
    }

    private async Task ManageMenuOptions()
    {
        var hasSession = await database.HasSessionAsync();
        var containsView = _menuOptions.Contains("View Sessions");

        if (hasSession && !containsView)
        {
            _menuOptions.Insert(1, "View Sessions");
            _menuOptions.Insert(2, "View Average Report");
            _menuOptions.Insert(3, "View Session Report");
        }


        if (!hasSession && containsView)
        {
            _menuOptions.Remove("View Sessions");
            _menuOptions.Remove("View Average Report");
            _menuOptions.Remove("View Session Report");
        }
            
    }

    private async Task ViewSessions()
    {
        var sessionDto = await database.ListSessionTransferAsync();
        
        if (sessionDto == null)
            throw new Exception("No valid session transfer");
        
        StudySessionView.ViewSessions(sessionDto);
    }
    
    private async Task AverageReport()
    {
        var year = PromptForReportYear(); 
        
        var report = await database.GetSessionAverageMonthsByYearReport(year);
        
        if (report == null)
            throw new Exception("No valid session avg report");
        
        StudySessionView.AverageReport(report, year);
    }

    private async Task SessionReport()
    {
        var year = PromptForReportYear(); 
        
        var report = await database.GetSessionsPerMonthByYearReport(year);
        
        if (report == null)
            throw new Exception("No valid session per month report");
        
        StudySessionView.SessionReport(report, year);
    }

    private static int PromptForReportYear()
    {
        var currentYear = DateTime.Now.Year;
        
        return AnsiConsole.Prompt(
            new TextPrompt<int>("What year would you like to report on?")
                .Validate((n) =>
                {
                    if (n > currentYear)
                        return ValidationResult.Error("Cannot report on the future");
                    
                    return n < 1990 
                        ? ValidationResult.Error("Too early a year to report on") 
                        : ValidationResult.Success();
                }));
    }
}