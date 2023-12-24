using System.Data;
using Npgsql;
using NpgsqlTypes;

class Program
    {
        static string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
        static void Main(string[] args) 
        {
            Console.Write("Введите имя пользователя:");
            string name = Console.ReadLine();
 
            GetAgeRange(name);
 
            Console.Read();
        }
 
        private static void GetAgeRange(string name)
        {
            string sqlExpression = "sp_getagerange";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                command.CommandType = CommandType.StoredProcedure;

                NpgsqlParameter nameParam = new NpgsqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                // определяем первый выходной параметр
                NpgsqlParameter minAgeParam = new NpgsqlParameter
                {
                    ParameterName = "@minage",
                    NpgsqlDbType = (NpgsqlDbType)SqlDbType.Int
                };
                minAgeParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(minAgeParam);

                // определяем второй выходной параметр
                NpgsqlParameter maxAgeParam = new NpgsqlParameter
                {
                    ParameterName = "@maxage",
                    NpgsqlDbType = (NpgsqlDbType)SqlDbType.Int
                };
                maxAgeParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(maxAgeParam);
 
                command.ExecuteNonQuery();
 
                Console.WriteLine("Минимальный возраст: {0}", command.Parameters["@minage"].Value);
                Console.WriteLine("Максимальный возраст: {0}", command.Parameters["@maxage"].Value);
            }

        }
    }