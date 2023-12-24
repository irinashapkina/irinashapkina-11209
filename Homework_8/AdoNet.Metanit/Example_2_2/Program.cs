using Npgsql;
using Dapper;

using Example_1_1.Models;

class Program
{
    static void Main()
    {
        string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany;";

        using (var dbConnection = new NpgsqlConnection(connectionString))
        {
            dbConnection.Open();

            var query = dbConnection.Query<Clients>("SELECT * FROM clients ORDER BY age");

            var groupedQuery = query.GroupBy(u => u.Age);

            foreach (var group in groupedQuery)
            {
                Console.WriteLine($"Возраст: {group.Key}");
                foreach (var client in group)
                    Console.WriteLine(client.FullName);
                Console.WriteLine();
            }
        }
    }
}