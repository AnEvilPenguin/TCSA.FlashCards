using Flashcards.Controllers;
using Flashcards.Model;
using Spectre.Console;

namespace Flashcards.View;

internal static class StudySessionView
{
    internal static StudySession RunTest(StackCardTransferObject dto)
    {
        var studySession = new StudySession()
        {
            StackId = dto.StackId,
            CardsCount = dto.Cards.Count,
        };
        
        for (int i = 0; i < studySession.CardsCount; i++)
        {
            AnsiConsole.Clear();
                    
            var rule = new Rule($"[green]{dto.StackName} - ({i + 1}/{studySession.CardsCount})[/]")
            {
                Style = Style.Parse("red dim"),
                Justification = Justify.Left
            };

            AnsiConsole.Write(rule);
                    
            var card = dto.Cards[i];
            
            TestCard(studySession, card);
        }
        
        return studySession;
    }

    internal static void ViewSessions(IEnumerable<SessionTransferObject> sessions)
    {
        AnsiConsole.Clear();
        
        var table = new Table();
        
        table.AddColumn(new TableColumn("Date"));
        table.AddColumn(new TableColumn("Stack"));
        table.AddColumn(new TableColumn("Score"));
        table.AddColumn(new TableColumn("Card Count"));
        table.AddColumn(new TableColumn("Percentage"));

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Date.ToString("R"),
                session.Name,
                session.Score.ToString(),
                session.CardCount.ToString(),
                $"{session.Score / (float)session.CardCount * 100 :0}%");
        }
        
        AnsiConsole.Write(table);
        
        ViewHelpers.WaitForContinue();
    }
    
    private static void TestCard(StudySession session, CardTransferObject card)
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
    
    private static Panel ConfigureCardPanel(string header, string content)
    {
        var panel = new Panel(content)
            .Expand();

        panel.Padding = new Padding(2);

        panel.Header = new PanelHeader(header) { Justification = Justify.Center };
        panel.Border = BoxBorder.Double;
        
        return panel;
    }
}