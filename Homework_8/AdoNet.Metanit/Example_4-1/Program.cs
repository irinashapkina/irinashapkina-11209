using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

NpgsqlConnection connection;
connection = new NpgsqlConnection(connectionString);
connection.Open();

Console.WriteLine(connection.ProcessID);
connection.Close();

connection.Open();
Console.WriteLine(connection.ProcessID);
connection.Close();