namespace Flashcards.Model;

internal class Card
{
    private const int MaxStringLength = 255;
    
    internal int? Id { get; set; }
    internal required string Front { get; set; }
    internal required string Back { get; set; }

    internal static bool IsValidString(string proposed, string type, out string error)
    {
        error = string.Empty;

        if (string.IsNullOrEmpty(proposed))
        {
            error = $"{type} must not be empty";
            return false;
        }

        if (proposed.Length  > MaxStringLength)
        {
            error = $"{type} must be less than [orange3]{MaxStringLength}[/] characters!";
            return false;
        }
        
        return true;
    }
}