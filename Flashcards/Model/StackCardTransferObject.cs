namespace Flashcards.Model;

internal class StackCardTransferObject
{
    internal required string StackName { get; set; }
    internal required List<CardTransferObject> Cards { get; set; }
}