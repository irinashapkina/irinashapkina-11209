using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

var status = ClientStatus.Silver;
var gender = ClientGender.Male;

string sqlExpression = "INSERT INTO Clients (gender, full_name, age, contact_info, status, is_anonymous, is_blocked) " +
                       $"VALUES ('{gender.ToString()}', 'Иванов Иван Иванович', 18, 'ivanushka@gmail.com', '{status.ToString()}', true, false)";

using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
    int number = command.ExecuteNonQuery();
    Console.WriteLine("Добавлено объектов: {0}", number);
}

Console.Read();

public enum ClientStatus
{
    Gold,
    Silver,
    Bronze,
    Wood
}

public enum ClientGender
{
    Male,
    Female
}