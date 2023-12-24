using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Example_1_1.Models;
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