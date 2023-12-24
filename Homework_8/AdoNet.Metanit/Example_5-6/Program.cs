using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

Console.WriteLine("Выберите пол (1 - Male, 2 - Female):");
var gender = (СlientGender)int.Parse(Console.ReadLine());

Console.WriteLine("Введите имя:");
var fullName = Console.ReadLine();

Console.WriteLine("Ведите возраст:");
var age = int.Parse(Console.ReadLine());

Console.WriteLine("Введите номер или почту:");
var contact = Console.ReadLine();

Console.WriteLine("Выберите статус (1 - Gold, 2 - Silver, 3 - Bronze, 4 - Wood):");
var status = (СlientStatus)int.Parse(Console.ReadLine());

var sqlExpression =
    $"insert into \"clients\" values(6,'{gender}', '{fullName}',{age}, '{contact}', '{status}', false,false);";

using (var sqlConnection = new NpgsqlConnection(connectionString))
{
    sqlConnection.Open();
    var command = new NpgsqlCommand(sqlExpression, sqlConnection);
    var number = command.ExecuteNonQuery();
    Console.WriteLine("Добавлено объектов: {0}", number);

    Console.WriteLine("Введите новое имя:");
    var name = Console.ReadLine();
    sqlExpression = $"UPDATE \"clients\" set full_name = '{name}' where age = {age}";
    command.CommandText = sqlExpression;
    number = command.ExecuteNonQuery();
    Console.WriteLine("Обновлено объектов: {0}", number);
}

public enum СlientGender
{
    Male = 1,
    Female = 2
}

public enum СlientStatus
{
    Gold = 1,
    Silver = 2,
    Bronze = 3,
    Wood = 4
}