using System.Data;
using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
string sql = "SELECT * FROM clients";
using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var adapter = new NpgsqlDataAdapter(sql, connection);

    var ds = new DataSet();
    adapter.Fill(ds);
    // перебор всех таблиц
    foreach (DataTable dt in ds.Tables)
    {
        Console.WriteLine(dt.TableName); // название таблицы
        // перебор всех столбцов
        foreach (DataColumn column in dt.Columns)
            Console.Write("\t{0}", column.ColumnName);
        Console.WriteLine();
        // перебор всех строк таблицы
        foreach (DataRow row in dt.Rows)
        {
            // получаем все ячейки строки
            var cells = row.ItemArray;
            foreach (object cell in cells)
                Console.Write("\t{0}", cell);
            Console.WriteLine();
        }
    }
}