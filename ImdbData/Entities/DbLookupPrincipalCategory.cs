using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("lookup_principal_categories")]
public class DbLookupPrincipalCategory
{
    [Key]
    public int Id { get; set; }
    public string? Category { get; set; }
}
