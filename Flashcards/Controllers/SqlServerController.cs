using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Flashcards.Controllers;

internal class SqlServerController
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString;
    
    private SqlConnection GetConnection() => 
        new(_connectionString);

    internal async Task RunExample()
    {
        try
        {
            await using var connection = GetConnection();
            Console.WriteLine("\nQuery data example:");
            Console.WriteLine("=========================================\n");

            await connection.OpenAsync();

            var sql = "SELECT name, collation_name FROM sys.databases";
            
            await using var command = new SqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
            }
        }
        //catch (SqlException e) when (e.Number == /* specific error number */)
        catch (SqlException e)
        {
            Console.WriteLine($"SQL Error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

}