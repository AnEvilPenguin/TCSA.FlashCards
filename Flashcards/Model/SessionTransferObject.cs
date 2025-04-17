namespace Flashcards.Model;

internal class SessionTransferObject
{
    internal required string Name { get; set; }
    internal required int CardCount { get; set; }
    internal required int Score { get; set; }
    internal required DateTime Date { get; set; }
}