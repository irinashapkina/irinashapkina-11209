using System.Data;
using Npgsql;
string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
string sql = "SELECT * FROM clients";
using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var adapter = new NpgsqlDataAdapter(sql, connection);
 
    DataSet ds = new DataSet("Clients");
    DataTable dt = new DataTable("Client");
    ds.Tables.Add(dt);
    adapter.Fill(ds.Tables["Client"]);
 
    ds.WriteXml("clients.xml");
    Console.WriteLine("Данные сохранены в файл");
}