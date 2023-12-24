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
            var id = reader.GetValue(0);
            var gender = reader.GetValue(1);
            var fullName = reader.GetValue(2);
            var age = reader.GetValue(3);
            var contactInfo = reader.GetValue(4);
            var status = reader.GetValue(5);
            var isAnonymous = reader.GetValue(6);
            var isBlocked = reader.GetValue(7);

            Console.WriteLine(
                "Id: {0}, Gender: {1}, Full name: {2}, Age: {3}, Contact: {4}, Status: {5}, isAnonymous: {6}, " +
                "isBlocked:{7}",
                id, gender, fullName, age, contactInfo, status, isAnonymous, isBlocked);
        }
    }

    reader.Close();
}

Console.Read();