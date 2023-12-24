using Npgsql;

class Program
{
    static void Main(string[] args)
    {
        ConnectWithDB().GetAwaiter();
    }
 
    private static async Task ConnectWithDB()
    {
        string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
 
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();
            Console.WriteLine("Подключение открыто");
        }
        Console.WriteLine("Подключение закрыто...");
    }
}