using Npgsql;

string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

var name = "T',10);INSERT INTO \"Clients\" (Name, Age) VALUES('H";
int age = 23;
string gender = "Male";
string contactInfo = "123-456-7890";
string status = "Gold";
bool isAnonymous = false;
bool isBlocked = false;

string sqlExpression = "INSERT INTO clients (id, gender, full_name, age, contact_info, status, is_anonymous, is_blocked) " +
                       "VALUES (9, @gender, @name, @age, @contactInfo, @status, @isAnonymous, @isBlocked)";

using (var connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    var command = new NpgsqlCommand(sqlExpression, connection);

    var genderParam = new NpgsqlParameter("@gender", gender);
    command.Parameters.Add(genderParam);
    var nameParam = new NpgsqlParameter("@name", name);
    command.Parameters.Add(nameParam);
    var ageParam = new NpgsqlParameter("@age", age);
    command.Parameters.Add(ageParam);
    var contactInfoParam = new NpgsqlParameter("@contactInfo", contactInfo);
    command.Parameters.Add(contactInfoParam);
    var statusParam = new NpgsqlParameter("@status", status);
    command.Parameters.Add(statusParam);
    var isAnonymousParam = new NpgsqlParameter("@isAnonymous", isAnonymous);
    command.Parameters.Add(isAnonymousParam);
    var isBlockedParam = new NpgsqlParameter("@isBlocked", isBlocked);
    command.Parameters.Add(isBlockedParam);

    int number = command.ExecuteNonQuery();
    Console.WriteLine("Добавлено объектов: {0}", number); // 1
}
