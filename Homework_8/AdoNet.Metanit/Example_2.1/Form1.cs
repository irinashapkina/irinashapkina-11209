using System.Data;
using Npgsql;

namespace Example_2._1;

public partial class Form1 : Form
{
    int pageSize = 5; // размер страницы
    int pageNumber = 0; // текущая страница
    string connectionString = "Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";
    NpgsqlDataAdapter adapter;
    DataSet ds;

    public Form1()
    {
        InitializeComponent();

        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.AllowUserToAddRows = false;

        using (var connection = new NpgsqlConnection(connectionString))
        {
            adapter = new NpgsqlDataAdapter(GetSql(), connection);

            ds = new DataSet();
            adapter.Fill(ds, "Clients");
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns["Id"].ReadOnly = true;
        }
    }

    private void nextButton_Click(object sender, EventArgs e)
    {
        if (ds.Tables["Clients"].Rows.Count < pageSize) return;

        pageNumber++;
        using (var connection = new NpgsqlConnection(connectionString))
        {
            adapter = new NpgsqlDataAdapter(GetSql(), connection);

            ds.Tables["Clients"].Rows.Clear();

            adapter.Fill(ds, "Clients");
        }
    }

    private void backButton_Click(object sender, EventArgs e)
    {
        if (pageNumber == 0) return;
        pageNumber--;

        using (var connection = new NpgsqlConnection(connectionString))
        {
            adapter = new NpgsqlDataAdapter(GetSql(), connection);

            ds.Tables["Clients"].Rows.Clear();

            adapter.Fill(ds, "Clients");
        }
    }
    
    private string GetSql()
    {
        return $"SELECT * FROM Clients ORDER BY Id OFFSET ({pageNumber} * {pageSize}) " +
               $"ROWS FETCH NEXT {pageSize} ROWS ONLY";
    }
}