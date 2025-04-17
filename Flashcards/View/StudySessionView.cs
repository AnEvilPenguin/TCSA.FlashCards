using Flashcards.Controllers;
using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal class StudySessionView()
{
    internal async Task<StudySession> RunTest(StackCardTransferObject dto)
    {
        var studySession = new StudySession()
        {
            StackId = dto.StackId,
            CardsCount = dto.Cards.Count,
        };
        
        for (int i = 0; i < studySession.CardsCount; i++)
        {
            AnsiConsole.Clear();
                    
            var rule = new Rule($"[green]{dto.StackName} - ({i + 1}/{studySession.CardsCount})[/]");
            rule.Style = Style.Parse("red dim");
            rule.Justification = Justify.Left;
        
            AnsiConsole.Write(rule);
                    
            var card = dto.Cards[i];
            
            TestCard(studySession, card);
        }
        
        return studySession;
    }

    private void TestCard(StudySession session, CardTransferObject card)
    {
        var frontPanel = ConfigureCardPanel("Front", card.Front);
        var backPanel = ConfigureCardPanel("Back", card.Back);

        AnsiConsole.Write(frontPanel);

        var answer = AnsiConsole.Ask<string>("What is the answer?");
        
        AnsiConsole.Write(backPanel);

        if (answer == card.Back)
        {
            session.Score++;
            
            AnsiConsole.MarkupLine("[green]Correct![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Incorrect[/]");
        }
        
        ViewHelpers.WaitForContinue();
    }
    
    private Panel ConfigureCardPanel(string header, string content)
    {
        var panel = new Panel(content)
            .Expand();

        panel.Padding = new Padding(2);

        panel.Header = new PanelHeader(header) { Justification = Justify.Center };
        panel.Border = BoxBorder.Double;
        
        return panel;
    }
}