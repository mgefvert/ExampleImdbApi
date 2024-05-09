using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("lookup_professions")]
public class DbLookupProfession
{
    [Key]
    public int Id { get; set; }
    public string? Profession { get; set; }
}
