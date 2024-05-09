using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("lookup_title_types")]
public class DbLookupTitleType
{
    [Key]
    public int Id { get; set; }
    public string? TitleType { get; set; }
}
