using Flashcards.Controllers;
using Flashcards.View;

var database = new SqlServerController();
await database.Initialize();

return await new MainMenu(database).DisplayMenu();