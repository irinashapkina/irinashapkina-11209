using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;

class Program
{
    static void Main()
    {
        string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
        string sql = "SELECT * FROM clients";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Create data adapter
            var adapter = new NpgsqlDataAdapter(sql, connection);
            var commandBuilder = new NpgsqlCommandBuilder(adapter);

            // Set the command for insertion
            adapter.InsertCommand = new NpgsqlCommand("sp_CreateClient", connection);
            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;

            // Set parameters for the stored procedure
            // ...
            adapter.InsertCommand.Parameters.Add(new NpgsqlParameter("@gender", NpgsqlDbType.Varchar, 10, "gender"));
            adapter.InsertCommand.Parameters.Add(new NpgsqlParameter("@full_name", NpgsqlDbType.Varchar, 255, "full_name"));
            adapter.InsertCommand.Parameters.Add(new NpgsqlParameter("@age", NpgsqlDbType.Integer, 0, "age"));
            adapter.InsertCommand.Parameters.Add(new NpgsqlParameter("@contact_info", NpgsqlDbType.Varchar, 255, "contact_info"));
            adapter.InsertCommand.Parameters.Add(new NpgsqlParameter("@status", NpgsqlDbType.Varchar, 10, "status"));
            adapter.InsertCommand.Parameters.Add(new NpgsqlParameter("@is_anonymous", NpgsqlDbType.Boolean, 0, "is_anonymous"));
            adapter.InsertCommand.Parameters.Add(new NpgsqlParameter("@is_blocked", NpgsqlDbType.Boolean, 0, "is_blocked"));
            NpgsqlParameter parameter = new NpgsqlParameter("@id", NpgsqlDbType.Integer);
            parameter.Direction = ParameterDirection.Output;
            adapter.InsertCommand.Parameters.Add(parameter); 
            
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            // Add a new row
            DataRow newRow = dt.NewRow();
            newRow["gender"] = "Male";
            newRow["full_name"] = "Kris";
            newRow["age"] = 26;
            newRow["contact_info"] = "1234567890";
            newRow["status"] = "Gold";
            newRow["is_anonymous"] = false;
            newRow["is_blocked"] = false;
            dt.Rows.Add(newRow);

            // Update the dataset
            adapter.Update(ds);
            ds.AcceptChanges();

            // Display column names
            foreach (DataColumn column in dt.Columns)
                Console.Write("\t{0}", column.ColumnName);
            Console.WriteLine();

            // Display data
            foreach (DataRow row in dt.Rows)
            {
                var cells = row.ItemArray;
                foreach (object cell in cells)
                    Console.Write("\t{0}", cell);
                Console.WriteLine();
            }
        }

        Console.Read();
    }
}
