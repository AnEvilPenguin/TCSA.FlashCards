namespace Flashcards.Model;

internal class Stack
{
    private const int MaxNameLength = 100;
    
    internal int Id { get; set; }
    internal required string Name { get; set; }

    internal static bool IsValidName(string name, out string error)
    {
        error = string.Empty;

        if (string.IsNullOrEmpty(name))
        {
            error = "Name must not be empty";
            return false;
        }

        if (name.Length > MaxNameLength)
        {
            error = "Name must be less than 100 characters!";
            return false;
        }
        
        return true;
    }
}