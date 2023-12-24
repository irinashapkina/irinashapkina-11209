using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

string sqlExpression = "UPDATE \"clients\" SET Age=20 WHERE full_name='Иванов Иван Иванович'";
using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var command = new NpgsqlCommand(sqlExpression, connection);
    int number = command.ExecuteNonQuery();
    Console.WriteLine("Обновлено объектов: {0}", number);
}