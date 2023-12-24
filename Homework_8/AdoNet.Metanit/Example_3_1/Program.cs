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

            Console.WriteLine("До обновления");
            var clientsBeforeUpdate = dbConnection.Query<Clients>("SELECT * FROM clients LIMIT 5");
            foreach (var client in clientsBeforeUpdate)
            {
                Console.WriteLine("{0} \t{1} \t{2}", client.Id, client.FullName, client.Age);
            }

            Console.WriteLine();

            // возьмем первого пользователя
            var client1 = clientsBeforeUpdate.FirstOrDefault();
            if (client1 != null)
            {
                // и изменим у него возраст
                client1.Age = 28;

                // сохраним изменения
                dbConnection.Execute("UPDATE clients SET Age = @Age WHERE Id = @Id", client1);
            }

            Console.WriteLine();
            Console.WriteLine("После обновления");
            var clientsAfterUpdate = dbConnection.Query<Clients>("SELECT * FROM clients LIMIT 5");
            foreach (var client in clientsAfterUpdate)
            {
                Console.WriteLine("{0} \t{1} \t{2}", client.Id, client.FullName, client.Age);
            }
        }
    }
}