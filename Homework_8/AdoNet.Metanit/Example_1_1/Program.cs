using Npgsql;
using Dapper;
using Example_1_1.Models;

class Program
{
    static string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
    
    static void Main(string[] args)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Получаем список клиентов
            var clients = connection.Query<Clients>("SELECT * FROM Clients").ToList();

            foreach (var client in clients)
            {
                Console.WriteLine("{0} \t{1} \t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
                    client.Id, client.Gender, client.FullName, client.Age, client.ContactInfo,
                    client.Status, client.IsAnonymous, client.IsBlocked);
            }
        }

        Console.Read();
    }
}