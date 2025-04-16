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
            await CreateDatabase();
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
            await CreateTables();
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
        try
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT TOP (1) ID
                FROM [FlashCards].[dbo].[Stacks];
            ";

            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();
            return reader.HasRows;
        }
        catch (SqlException e)
        {
            await Console.Error.WriteLineAsync($"SQL Error (HasStacksAsync): {e.Message} - Line: {e.Number}");
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.ToString());
        }
        
        return false;
    }

    internal async Task CreateStack(string name)
    {
        try
        {
            await using var connection = GetConnection();
            await connection.OpenAsync();

            var sql = @"
                INSERT INTO [FlashCards].[dbo].[Stacks]
                (Name)
                VALUES (@name) 
            ";

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("name", name);
            
            await command.ExecuteNonQueryAsync();
        }
        catch (SqlException e)
        {
            await Console.Error.WriteLineAsync($"SQL Error (CreateStack): {e.Message} - Line: {e.Number}");
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.ToString());
        }
    }

    private async Task CreateDatabase()
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();

        var sql = @"
            IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'FlashCards')
            BEGIN
                CREATE DATABASE [FlashCards]
            END
        ";
        
        await using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    private async Task CreateTables()
    {
        await using var connection = GetConnection();
        await connection.OpenAsync();
        
        var sql = @"
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
        ";
            
        await using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}