using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
string connectionString2 = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany2";

using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine(connection.ProcessID);
}

using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
        Console.WriteLine(connection.ProcessID);
}

using (NpgsqlConnection connection = new NpgsqlConnection(connectionString2))
{
    connection.Open();
    Console.WriteLine(connection.ProcessID);
}