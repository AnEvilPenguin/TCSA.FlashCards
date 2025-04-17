namespace Flashcards.Model;

internal class StackCardTransferObject
{
    internal required string StackName { get; set; }
    internal required List<Card> Cards { get; set; }
}