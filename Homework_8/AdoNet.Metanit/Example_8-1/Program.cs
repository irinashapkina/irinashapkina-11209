using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

string sqlExpression = "SELECT COUNT(*) FROM clients";
using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var command = new NpgsqlCommand(sqlExpression, connection);
    object count = command.ExecuteScalar();
 
    command.CommandText = "SELECT MIN(Age) FROM clients";
    object minAge = command.ExecuteScalar();
 
    Console.WriteLine("В таблице {0} объектов", count);
    Console.WriteLine("Минимальный возраст: {0}", minAge);
}