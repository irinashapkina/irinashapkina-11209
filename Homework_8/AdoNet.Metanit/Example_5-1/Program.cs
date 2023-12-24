using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    NpgsqlCommand command = new NpgsqlCommand();
    command.CommandText = "SELECT * FROM Clients";
    command.Connection = connection;
}