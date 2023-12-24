using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

string sqlExpression = "SELECT * FROM Clients";
using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var command = new NpgsqlCommand(sqlExpression, connection);
    var reader = command.ExecuteReader();

    if (reader.HasRows) // если есть данные
    {
        // выводим названия столбцов
        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
            reader.GetName(0),
            reader.GetName(1), reader.GetName(2), reader.GetName(3),
            reader.GetName(4), reader.GetName(5), reader.GetName(6), reader.GetName(7));

        while (reader.Read()) // построчно считываем данные
        {
            var id = reader.GetInt32(0);
            var gender = reader.GetString(1);
            var fullName = reader.GetString(2);
            var age = reader.GetInt32(3);
            var contactInfo = reader.GetString(4);
            var status = reader.GetString(5);
            var isAnonymous = reader.GetBoolean(6);
            var isBlocked = reader.GetBoolean(7);

            Console.WriteLine(
                "Id: {0}, Gender: {1}, Full name: {2}, Age: {3}, Contact: {4}, Status: {5}, isAnonymous: {6}, " +
                "isBlocked:{7}",
                id, gender, fullName, age, contactInfo, status, isAnonymous, isBlocked);
        }
    }

    reader.Close();
}

Console.Read();