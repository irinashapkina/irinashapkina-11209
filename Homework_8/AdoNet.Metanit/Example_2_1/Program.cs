using Npgsql;
using Dapper;
using Example_1_1.Models;

class Program
{
    static void Main()
    {
        using (var dbConnection = new NpgsqlConnection("Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany;"))
        {
            dbConnection.Open();

            var query = dbConnection.Query<Clients>("SELECT * FROM clients WHERE Age > @Age ORDER BY full_name", new { Age = 25 });

            foreach (var client in query)
            {
                Console.WriteLine($"{client.Id} \t{client.FullName} \t{client.Age}");
            }
        }
    }
}