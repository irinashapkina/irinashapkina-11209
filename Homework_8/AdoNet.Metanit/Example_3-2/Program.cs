using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Подключение открыто");
}

Console.WriteLine("Подключение закрыто...");

Console.Read();