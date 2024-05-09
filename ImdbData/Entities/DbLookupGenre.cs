using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("lookup_genres")]
public class DbLookupGenre
{
    [Key]
    public int Id { get; set; }
    public string? Genre { get; set; }
}
