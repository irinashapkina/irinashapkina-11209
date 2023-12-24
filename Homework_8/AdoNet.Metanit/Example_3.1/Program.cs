using System.Data;
using Npgsql;

class Program
{
    static void Main()
    {
        string connectionString =
            "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
        string sql = "SELECT * FROM clients";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Используем using для автоматического освобождения ресурсов
            using (var adapter = new NpgsqlDataAdapter(sql, connection))
            {
                // Загружаем данные из базы в DataSet
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                // Получаем таблицу из DataSet
                DataTable dt = ds.Tables[0];
                dt.Columns["id"].AutoIncrement = true;

                // Добавляем новую строку
                DataRow newRow = dt.NewRow();
                newRow["gender"] = "Female";
                newRow["full_name"] = "Alice";
                newRow["age"] = 24;
                newRow["contact_info"] = "Phone: 123-456-7890";
                newRow["status"] = "Gold";
                newRow["is_anonymous"] = false;
                newRow["is_blocked"] = false;
                dt.Rows.Add(newRow);

                // Создаем объект NpgsqlCommandBuilder
                var commandBuilder = new NpgsqlCommandBuilder(adapter);

                // Обновляем базу данных
                adapter.Update(ds);

                // Очищаем DataSet и перезагружаем данные из базы
                ds.Clear();
                adapter.Fill(ds);

                // Выводим заголовки столбцов
                foreach (DataColumn column in dt.Columns)
                    Console.Write("\t{0}", column.ColumnName);
                Console.WriteLine();

                // Перебираем все строки таблицы
                foreach (DataRow row in dt.Rows)
                {
                    // Получаем все ячейки строки
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                }
            }
        }

        Console.Read();
    }
}