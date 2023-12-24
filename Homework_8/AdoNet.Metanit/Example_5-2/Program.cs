using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
string sqlExpression = "SELECT * FROM Clients";
using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
}