using Npgsql;
using NpgsqlTypes;

public class Image
{
    public Image(int id, string filename, string title, byte[] data)
    {
        Id = id;
        FileName = filename;
        Title = title;
        Data = data;
    }
    public int Id { get; private set; }
    public string FileName { get; private set; }
    public string Title { get; private set; }
    public byte[] Data { get; private set; }
}

public class ImageDatabase
{
    private static string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

    public static void SaveFileToDatabase()
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            var command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = @"INSERT INTO Images (FileName, Title, ImageData) VALUES (@FileName, @Title, @ImageData)";
            command.Parameters.Add("@FileName", NpgsqlDbType.Varchar, 50);
            command.Parameters.Add("@Title", NpgsqlDbType.Varchar, 50);
            command.Parameters.Add("@ImageData", NpgsqlDbType.Bytea, 1000000);

            string filename = @"C:\Users\irina\OneDrive\Рабочий стол\cat.jpeg";
            string title = "Коты";

            string shortFileName = filename.Substring(filename.LastIndexOf('\\')+1); // cats.jpg

            byte[] imageData;
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                imageData = new byte[fs.Length];
                fs.Read(imageData, 0, imageData.Length);
            }

            command.Parameters["@FileName"].Value = shortFileName;
            command.Parameters["@Title"].Value = title;
            command.Parameters["@ImageData"].Value = imageData;
 
            command.ExecuteNonQuery();
        }
    }

    public static List<Image> ReadFileFromDatabase()
    {
        List<Image> images = new List<Image>();

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            
            string sql = "SELECT * FROM Images";
            using (var command = new NpgsqlCommand(sql, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string filename = reader.GetString(1);
                    string title = reader.GetString(2);
                    byte[] data = (byte[])reader.GetValue(3);

                    Image image = new Image(id, filename, title, data);
                    images.Add(image);
                }
            }
        }

        return images;
    }

    public static void Main()
    {
        SaveFileToDatabase();

        List<Image> images = ReadFileFromDatabase();

        if (images.Count > 0)
        {
            using (FileStream fs = new FileStream(images[0].FileName, FileMode.OpenOrCreate))
            {
                fs.Write(images[0].Data, 0, images[0].Data.Length);
                Console.WriteLine("Изображение '{0}' сохранено", images[0].Title);
            }
        }
    }
}