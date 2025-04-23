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
                session.Date.ToLocalTime().ToString("g"),
                session.Name,
                session.Score.ToString(),
                session.CardCount.ToString(),
                $"{session.Score / (float)session.CardCount * 100 :0}%");
        }
        
        AnsiConsole.Write(table);
        
        ViewHelpers.WaitForContinue();
    }

    internal static void AverageReport(IEnumerable<SessionReport> reports, int year)
    {
        AnsiConsole.Clear();
        
        var rule = new Rule($"[green]Average Scores - {year}[/]")
        {
            Style = Style.Parse("red dim"),
            Justification = Justify.Center
        };

        AnsiConsole.Write(rule);
        
        var table = GetMonthlyTable();

        foreach (var stack in reports)
        {
            table.AddRow(
                stack.StackName,
                GetAverageFromMonth(stack.January),
                GetAverageFromMonth(stack.February),
                GetAverageFromMonth(stack.March),
                GetAverageFromMonth(stack.April),
                GetAverageFromMonth(stack.May),
                GetAverageFromMonth(stack.June),
                GetAverageFromMonth(stack.July),
                GetAverageFromMonth(stack.August),
                GetAverageFromMonth(stack.September),
                GetAverageFromMonth(stack.October),
                GetAverageFromMonth(stack.November),
                GetAverageFromMonth(stack.December));
        }
        
        AnsiConsole.Write(table);
        
        ViewHelpers.WaitForContinue();
    }
    
    internal static void SessionReport(IEnumerable<SessionReport> reports, int year)
    {
        AnsiConsole.Clear();
        
        var rule = new Rule($"[green]Session count - {year}[/]")
        {
            Style = Style.Parse("red dim"),
            Justification = Justify.Center
        };

        AnsiConsole.Write(rule);

        var table = GetMonthlyTable();

        foreach (var stack in reports)
        {
            table.AddRow(
                stack.StackName,
                $"{stack.January}",
                $"{stack.February}",
                $"{stack.March}",
                $"{stack.April}",
                $"{stack.May}",
                $"{stack.June}",
                $"{stack.July}",
                $"{stack.August}",
                $"{stack.September}",
                $"{stack.October}",
                $"{stack.November}",
                $"{stack.December}");
        }
        
        AnsiConsole.Write(table);
        
        ViewHelpers.WaitForContinue();
    }

    private static Table GetMonthlyTable()
    {
        var table = new Table();
        
        table.AddColumn(new TableColumn("Stack Name"));
        table.AddColumn(new TableColumn("January"));
        table.AddColumn(new TableColumn("February"));
        table.AddColumn(new TableColumn("March"));
        table.AddColumn(new TableColumn("April"));
        table.AddColumn(new TableColumn("May"));
        table.AddColumn(new TableColumn("June"));
        table.AddColumn(new TableColumn("July"));
        table.AddColumn(new TableColumn("August"));
        table.AddColumn(new TableColumn("September"));
        table.AddColumn(new TableColumn("October"));
        table.AddColumn(new TableColumn("November"));
        table.AddColumn(new TableColumn("December"));
        
        return table;
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

    private static string GetAverageFromMonth(float? month) =>
        month != null ? month + "%" : "";
}