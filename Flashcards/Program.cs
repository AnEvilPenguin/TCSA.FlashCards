using Flashcards.Controllers;
using Flashcards.View;
using Microsoft.Data.SqlClient;
using Spectre.Console;

var database = new SqlServerController();

try
{
    await database.Initialize();
}
catch (SqlException sqlEx)
{
    AnsiConsole.Markup($"[red]CRITICAL: {sqlEx.Message}[/]");
    return 1;
}

return await new MainMenu(database).DisplayMenu();