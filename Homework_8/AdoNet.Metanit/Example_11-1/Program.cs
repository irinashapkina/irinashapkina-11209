using Npgsql;


class Program
{
    static string connectionString =
        "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
    static void Main(string[] args) 
    {
        Console.WriteLine("Выберите пол (1 - Male, 2 - Female):");
        var gender = (СlientGender)int.Parse(Console.ReadLine());
        
        Console.WriteLine("Введите полное имя:");
        var name = Console.ReadLine();
 
        Console.WriteLine("Введите возраст:");
        var age = Int32.Parse(Console.ReadLine());
        
        
        Console.WriteLine("Введите номер или почту:");
        var contact = Console.ReadLine();
        
        Console.WriteLine("Выберите статус (1 - Gold, 2 - Silver, 3 - Bronze, 4 - Wood):");
        var status = (СlientStatus)int.Parse(Console.ReadLine());

        AddUser(gender, name, age, contact, status);
 
        Console.WriteLine();
        //GetUsers();
 
        Console.Read();
    }
    // добавление пользователя
    static void AddUser(СlientGender gender, string name, int age, string contact, СlientStatus status)
    {
        // название процедуры
        string sqlExpression = "insertclients"; 
 
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
            // указываем, что команда представляет хранимую процедуру
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            NpgsqlParameter genderParam = new NpgsqlParameter
            {
                ParameterName = "@gender",
                Value = gender.ToString()
            };
            command.Parameters.Add(genderParam);
            // параметр для ввода имени
            NpgsqlParameter nameParam = new NpgsqlParameter
            {
                ParameterName = "@name",
                Value = name
            };
            // добавляем параметр
            command.Parameters.Add(nameParam);
            
            // параметр для ввода возраста
            NpgsqlParameter ageParam = new NpgsqlParameter
            {
                ParameterName = "@age",
                Value = age
            };
            command.Parameters.Add(ageParam);
            NpgsqlParameter contactParam = new NpgsqlParameter
            {
                ParameterName = "@contact",
                Value = contact
            };
            command.Parameters.Add(contactParam);
            NpgsqlParameter statusParam = new NpgsqlParameter
            {
                ParameterName = "@status",
                Value = status.ToString() 
            };
            command.Parameters.Add(statusParam);
            command.Parameters.Add(new NpgsqlParameter("@inserted_id", NpgsqlTypes.NpgsqlDbType.Integer));
            command.Parameters["@inserted_id"].Direction = System.Data.ParameterDirection.Output;
            command.ExecuteNonQuery();
            // Получаем возвращаемый ID
            var insertedId = command.Parameters["@inserted_id"].Value;

            Console.WriteLine("Id добавленного объекта: {0}", insertedId);
        }
    }

    // void GetUsers()
    // {
    //     // название процедуры
    //     string sqlExpression = "sp_GetUsers";
    //
    //     using (SqlConnection connection = new SqlConnection(connectionString))
    //     {
    //         connection.Open();
    //         SqlCommand command = new SqlCommand(sqlExpression, connection);
    //         // указываем, что команда представляет хранимую процедуру
    //         command.CommandType = System.Data.CommandType.StoredProcedure;
    //         var reader = command.ExecuteReader();
    //
    //         if (reader.HasRows)
    //         {
    //             Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));
    //
    //             while (reader.Read())
    //             {
    //                 int id = reader.GetInt32(0);
    //                 string name = reader.GetString(1);
    //                 int age = reader.GetInt32(2);
    //                 Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
    //             }
    //         }
    //         reader.Close();
    //     }
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