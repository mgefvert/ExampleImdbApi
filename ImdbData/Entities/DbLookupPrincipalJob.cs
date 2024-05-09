using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("lookup_principal_jobs")]
public class DbLookupPrincipalJob
{
    [Key]
    public int Id { get; set; }
    public string? Job { get; set; }
}
