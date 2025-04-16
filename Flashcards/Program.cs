using Flashcards.Controllers;
using Flashcards.View;

var database = new SqlServerController();
await database.Initialize();

return new MainMenu(database).Run();