using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

string sqlExpression = "DELETE  FROM clients WHERE full_name='Иванов Иван Иванович'";
using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var command = new NpgsqlCommand(sqlExpression, connection);
    int number = command.ExecuteNonQuery();
    Console.WriteLine("Удалено объектов: {0}", number);
}