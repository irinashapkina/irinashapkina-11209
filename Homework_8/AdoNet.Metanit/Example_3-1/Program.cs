using Npgsql;


string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

NpgsqlConnection connection = new NpgsqlConnection(connectionString);
try
{
    connection.Open();
    Console.WriteLine("Подключение открыто");
}
catch (NpgsqlException ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    connection.Close();
    Console.WriteLine("Подключение закрыто...");
}

Console.Read();