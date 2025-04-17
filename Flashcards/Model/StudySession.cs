namespace Flashcards.Model;

internal class StudySession
{
    internal Stack Stack { get; set; }
    internal int CardsCount { get; set; }
    internal int Score { get; set; } = 0;
    internal DateTime Date { get; set; }
}