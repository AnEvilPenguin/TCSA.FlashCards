using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Flashcards.Controllers;

internal class SqlServerController
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;
    
    private SqlConnection GetConnection() => 
        new(_connectionString);

    internal async Task Initialize()
    {
        try
        {
            await CreateDatabaseAsync();
        }
        catch (SqlException e)
        {
            await Console.Error.WriteLineAsync($"SQL Error (Database): {e.Message} - Line: {e.Number}");
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.ToString());
        }
        
        try
        {
            await CreateTablesAsync();
        }
        catch (SqlException e)
        {
            await Console.Error.WriteLineAsync($"SQL Error (Tables): {e.Message} - Line: {e.Number}");
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.ToString());
        }
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

    internal async Task CreateStackAsync(string name)
    {
        await HandleError(async () =>
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            const string sql = """
                                   INSERT INTO [FlashCards].[dbo].[Stacks]
                                   (Name)
                                   VALUES (@name)
                                """;

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("name", name);
            
            await command.ExecuteNonQueryAsync();
        }, "CreateStackAsync");
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