using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyORM
;
[Table("Clients")]
public class Clients
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    [Column("gender", TypeName = "character varying(10)")]
    public string Gender { get; set; }

    [StringLength(255)]
    [Column("full_name", TypeName = "character varying(255)")]
    public string FullName { get; set; }

    [Column("age")]
    public int Age { get; set; }

    [StringLength(255)]
    [Column("contact_info", TypeName = "character varying(255)")]
    public string ContactInfo { get; set; }

    [Required]
    [StringLength(10)]
    [Column("status", TypeName = "character varying(10)")]
    public string Status { get; set; }

    [Column("is_anonymous")]
    public bool IsAnonymous { get; set; }

    [Column("is_blocked")]
    public bool IsBlocked { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string connectionString =
                @"Username=postgres;Password=root;Host=localhost;Port=5432;Database=EroticMassageCompany";

        var myDatabase = new Database(connectionString);
            
        var clientsList = myDatabase.Select<Clients>();
        foreach (var client in clientsList)
        {
                Console.WriteLine(client.FullName);
                Console.Read();
        }

        // Example of inserting a new client
        Clients newClient = new Clients
        {
            Gender = "Male",
            FullName = "John Doe",
            Age = 30,
            ContactInfo = "john.doe@example.com",
            Status = "Gold",
            IsAnonymous = false,
            IsBlocked = false
        };

        myDatabase.Insert(newClient);

        // Example of updating a client
        Clients clientToUpdate = clientsList.FirstOrDefault();
        if (clientToUpdate != null)
        {
            clientToUpdate.FullName = "Updated Name";
            myDatabase.Update(clientToUpdate);
        }

        // Example of deleting a client by ID
        int clientIdToDelete = 1;
        myDatabase.DeleteById<Clients>(clientIdToDelete);
    }
        
}
