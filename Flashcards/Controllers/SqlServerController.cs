using System.Configuration;
using Flashcards.Model;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Flashcards.Controllers;

internal class SqlServerController
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;
    
    private SqlConnection GetConnection() => 
        new(_connectionString);

    internal async Task Initialize()
    {
        SqlMapper.AddTypeHandler(new DateTimeHandler());
        
        await HandleError(CreateDatabaseAsync, "CreateDatabaseAsync");
        await HandleError(CreateTablesAsync, "CreateTablesAsync");
    }

    internal async Task<bool> HasStacksAsync()
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT TOP (1) ID
                                   FROM [FlashCards].[dbo].[Stacks];
                               """;

            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }, "HasStacksAsync");
    }

    internal async Task<bool> HasStackWithCardsAsync()
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT TOP (1) Name
                                   FROM [FlashCards].[dbo].[Stacks] AS "Stacks" 
                                   JOIN [FlashCards].[dbo].[Cards] AS "Cards" ON [Stacks].[ID] = [Cards].StackID;
                               """;

            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }, "HasStacksAsync");
    }

    internal async Task CreateStackAsync(string name)
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   INSERT INTO [FlashCards].[dbo].[Stacks]
                                   (Name)
                                   VALUES (@name);
                                """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("name", name);
            
            await command.ExecuteNonQueryAsync();
        }, "CreateStackAsync");
    }

    internal async Task<StackCardTransferObject> GetStackCardTransferObjectAsync(Stack stack)
    {
        var cards = await GetCardsAsync(stack);
            
        var cardTransferObjects = cards
            .Select((Card card) => new CardTransferObject() { Front = card.Front, Back = card.Back})
            .ToList();
        
        return new StackCardTransferObject()
        {
            StackName = stack.Name,
            StackId = stack.Id,
            Cards = cardTransferObjects
        };
    }
    
    internal async Task<IEnumerable<Stack>?> ListStacksWithCardsAsync()
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT DISTINCT [Name], [Stacks].[ID]
                                   FROM [FlashCards].[dbo].[Stacks] AS "Stacks"
                                   JOIN [FlashCards].[dbo].[Cards] AS "Cards" ON [Stacks].[ID] = [Cards].StackID;
                               """;
            
            return await connection.QueryAsync<Stack>(sql);
        }, "ListStacksAsync");
    }

    private async Task<IEnumerable<Stack>?> ListStacksAsync()
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT * 
                                   FROM [FlashCards].[dbo].[Stacks];
                               """;
            
            return await connection.QueryAsync<Stack>(sql);
        }, "ListStacksAsync");
    }
    
    internal async Task<IEnumerable<Stack>> GetStacksAsync()
    {
        var stacks = await ListStacksAsync();

        if (stacks == null)
        {
            throw new Exception("No stacks found");
        }
        
        return stacks;
    }

    internal async Task DeleteStackAsync(Stack stack)
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                  DELETE
                                  FROM [FlashCards].[dbo].[Stacks]
                                  WHERE [ID] = @ID;
                               """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("ID", stack.Id);
            
            await command.ExecuteNonQueryAsync();
        }, "DeleteStackAsync");
    }

    internal async Task RenameStackAsync(Stack stack, string newName)
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                  UPDATE [FlashCards].[dbo].[Stacks]
                                  SET [Name] = @newName
                                  WHERE [ID] = @ID;
                               """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("ID", stack.Id);
            command.Parameters.AddWithValue("newName", newName);
            
            await command.ExecuteNonQueryAsync();
        }, "RenameStackAsync");
    }
    
    internal async Task<bool> HasCardsAsync(Stack stack)
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT TOP (1) ID
                                   FROM [FlashCards].[dbo].[Cards]
                                   WHERE [StackID] = @StackId;
                               """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("StackId", stack.Id);
            
            await using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }, "HasStacksAsync");
    }
    
    private async Task<IEnumerable<Card>?> ListCardsAsync(Stack stack)
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT * 
                                   FROM [FlashCards].[dbo].[Cards]
                                   WHERE [StackID] = @StackId;
                               """;

            var parameters = new { StackId = stack.Id };
            
            return await connection.QueryAsync<Card>(sql, parameters);
        }, "ListCardsAsync");
    }
    
    internal async Task<IEnumerable<Card>> GetCardsAsync(Stack stack)
    {
        var cards = await ListCardsAsync(stack);

        if (cards == null)
        {
            throw new Exception("No stacks found");
        }
        
        return cards;
    }

    internal async Task CreateCardAsync(Stack stack, Card card)
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   INSERT INTO [FlashCards].[dbo].[Cards]
                                   (Front, Back, StackID)
                                   VALUES (@Front, @Back, @StackId);
                               """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("Front", card.Front);
            command.Parameters.AddWithValue("Back", card.Back);
            command.Parameters.AddWithValue("StackId", stack.Id);
            
            await command.ExecuteNonQueryAsync();
        }, "CreateCardAsync");
    }
    
    internal async Task DeleteCardAsync(Card card)
    {
        if (card.Id == null)
            throw new Exception("Card id is null");
        
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                  DELETE
                                  FROM [FlashCards].[dbo].[Cards]
                                  WHERE [ID] = @ID;
                               """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("ID", card.Id);
            
            await command.ExecuteNonQueryAsync();
        }, "DeleteCardAsync");
    }
    
    internal async Task<bool> HasSessionAsync()
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT TOP (1) ID
                                   FROM [FlashCards].[dbo].[Sessions];
                               """;

            await using var command = new SqlCommand(sql, connection);
            
            await using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }, "HasStacksAsync");
    }

    internal async Task CreateSessionAsync(StudySession session)
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   INSERT INTO [FlashCards].[dbo].[Sessions]
                                   (CardCount, Score, StackID)
                                   VALUES (@CardCount, @Score, @StackId);
                               """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("CardCount", session.CardsCount);
            command.Parameters.AddWithValue("Score", session.Score);
            command.Parameters.AddWithValue("StackId", session.StackId);
            
            await command.ExecuteNonQueryAsync();
        }, "CreateCardAsync");
    }

    internal async Task<IEnumerable<SessionTransferObject>?> ListSessionTransferAsync()
    {
        return await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   SELECT 
                                       [Stacks].[Name] AS "Name",
                                       [Sessions].[CardCount] AS "CardCount",
                                       [Sessions].[Score] AS "Score",
                                       [Sessions].[Date] As "Date"
                                   FROM [FlashCards].[dbo].[Sessions] AS "Sessions"
                                   JOIN [FlashCards].[dbo].[Stacks] AS "Stacks" 
                                       ON [Sessions].[StackID] = [Stacks].[ID];
                               """;
            
            return await connection.QueryAsync<SessionTransferObject>(sql);
        }, "ListSessionTransferAsync");
    }

    private async Task CreateDatabaseAsync()
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'FlashCards')
                                   BEGIN
                                       CREATE DATABASE [FlashCards]
                                   END
                               """;

            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, "CreateDatabaseAsync");
    }

    private async Task CreateTablesAsync()
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();
            
            const string sql = """
                                   USE [FlashCards]

                                   IF OBJECT_ID(N'dbo.Stacks', N'U') IS NULL
                                   BEGIN
                                       CREATE TABLE Stacks (
                                           ID INT PRIMARY KEY IDENTITY (1, 1),
                                           Name VARCHAR(100)
                                       )
                                   END
                                   
                                   IF OBJECT_ID(N'dbo.Cards', N'U') IS NULL
                                   BEGIN
                                       CREATE TABLE Cards (
                                           ID INT PRIMARY KEY IDENTITY (1, 1),
                                           Front VARCHAR(255),
                                           Back VARCHAR(255),
                                           StackID INT NOT NULL,
                                           CONSTRAINT FK_Stacks_Cards FOREIGN KEY (StackID)
                                               REFERENCES Stacks (ID)
                                           ON DELETE CASCADE
                                       )
                                   END
                                   
                                   IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
                                   BEGIN
                                       CREATE TABLE Sessions (
                                           ID INT PRIMARY KEY IDENTITY (1, 1),
                                           Score INT NOT NULL,
                                           CardCount INT NOT NULL,
                                           Date DATETIME DEFAULT(GetUtcDate()),
                                           StackID INT NOT NULL,
                                           CONSTRAINT FK_Stacks_Sessions FOREIGN KEY (StackID)
                                               REFERENCES Stacks (ID)
                                           ON DELETE CASCADE
                                       )
                                   END
                               """;
                
            await using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }, "CreateTablesAsync");
    }

    private async Task<T?> HandleError<T>(Func<Task<T>> task, string command)
    {
        try
        {
            return await task();
        }
        catch (SqlException e)
        {
            await Console.Error.WriteLineAsync($"SQL Error ({command}): {e.Message} - Line: {e.Number}");
            throw;
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.ToString());
            throw;
        }
    }
    
    private async Task HandleError(Func<Task> task, string command)
    {
        try
        {
            await task();
        }
        catch (SqlException e)
        {
            await Console.Error.WriteLineAsync($"SQL Error ({command}): {e.Message} - Line: {e.Number}");
            throw;
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.ToString());
            throw;
        }
    }
}