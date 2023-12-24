using System.Data;
using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
var sql = "SELECT * FROM \"clients\";";
using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    // Создаем объект DataAdapter
    var adapter = new NpgsqlDataAdapter(sql, connection);
    // Создаем объект Dataset
    DataSet ds = new DataSet();
    // Заполняем Dataset
    adapter.Fill(ds);
    // Отображаем данные
    var table = ds.Tables[0];
    foreach (DataRow row in table.Rows)
    {
        foreach (var item in row.ItemArray)
        {
            Console.WriteLine(item);
        }
    }
}