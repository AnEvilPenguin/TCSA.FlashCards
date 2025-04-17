using Flashcards.Controllers;
using Spectre.Console;

namespace Flashcards.View;

internal class StudySessionView(SqlServerController database)
{
    internal async Task Run()
    {
        var validStacks = await database.ListStacksWithCardsAsync();
        
        if (validStacks == null)
            throw new Exception("No valid stacks found");
        
        var selectedStack = StackView.SelectStack(validStacks, " to study from");
        
        var dto = await database.GetStackCardTransferObjectAsync(selectedStack);
        
        AnsiConsole.MarkupLine($"Selected: {dto.StackName}, Count: {dto.Cards.Count}");
        
        ViewHelpers.WaitForContinue();
    }
}