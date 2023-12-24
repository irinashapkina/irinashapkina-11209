using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var transaction = connection.BeginTransaction();

    var command = connection.CreateCommand();
    command.Transaction = transaction;

    try
    {
        command.CommandText = $"insert into \"clients\" values(17,'{СlientGender.Male}', 'Жданов Ждан Жданович',50, 'zhdan@gmail.com', '{СlientStatus.Wood}', true,false);";
        command.ExecuteNonQuery();
        command.CommandText = $"insert into \"clients\" values(18,'{СlientGender.Female}', 'Баянова Баяна Баяновна',25, 'bayana@gmail.com', '{СlientStatus.Silver}', false,true);";
        command.ExecuteNonQuery();
 
        // подтверждаем транзакцию
        transaction.Commit();
        Console.WriteLine("Данные добавлены в базу данных");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        transaction.Rollback();
    }
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