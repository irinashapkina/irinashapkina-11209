using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;

string connectionString =
    "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
int age = 23;
string fullName = "Kenny";
string gender = "Male";
string contactInfo = "kenny@example.com";
string status = "Gold";
bool isAnonymous = false;
bool isBlocked = false;

string sqlExpression = @"INSERT INTO Clients (gender, full_name, age, contact_info, status, is_anonymous, is_blocked)
                         VALUES (@gender, @fullName, @age, @contactInfo, @status, @isAnonymous, @isBlocked)
                         RETURNING id";

using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
{
    connection.Open();
    NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
    command.CommandType = CommandType.Text;
    // Create parameter for gender
    NpgsqlParameter genderParam = new NpgsqlParameter("@gender", NpgsqlDbType.Varchar);
    genderParam.Value = gender;
    command.Parameters.Add(genderParam);

    // Create parameter for full_name
    NpgsqlParameter fullNameParam = new NpgsqlParameter("@fullName", NpgsqlDbType.Varchar);
    fullNameParam.Value = fullName;
    command.Parameters.Add(fullNameParam);

    // Create parameter for age
    NpgsqlParameter ageParam = new NpgsqlParameter("@age", NpgsqlDbType.Integer);
    ageParam.Value = age;
    command.Parameters.Add(ageParam);

    // Create parameter for contact_info
    NpgsqlParameter contactInfoParam = new NpgsqlParameter("@contactInfo", NpgsqlDbType.Varchar);
    contactInfoParam.Value = contactInfo;
    command.Parameters.Add(contactInfoParam);

    // Create parameter for status
    NpgsqlParameter statusParam = new NpgsqlParameter("@status", NpgsqlDbType.Varchar);
    statusParam.Value = status;
    command.Parameters.Add(statusParam);

    // Create parameter for isAnonymous
    NpgsqlParameter isAnonymousParam = new NpgsqlParameter("@isAnonymous", NpgsqlDbType.Boolean);
    isAnonymousParam.Value = isAnonymous;
    command.Parameters.Add(isAnonymousParam);

    // Create parameter for isBlocked
    NpgsqlParameter isBlockedParam = new NpgsqlParameter("@isBlocked", NpgsqlDbType.Boolean);
    isBlockedParam.Value = isBlocked;
    command.Parameters.Add(isBlockedParam);

    // Execute the command and get the returned id
    int newClientId = (int)command.ExecuteScalar();

    // Display the new client id
    Console.WriteLine("Id нового объекта: {0}", newClientId);
}