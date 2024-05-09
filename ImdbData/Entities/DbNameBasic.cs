using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("name_basics")]
public class DbNameBasic
{
    [Key]
    public int Id { get; set; }
    public string? PrimaryName { get; set; }
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    
    public ICollection<DbNameKnownForTitle>? KnownForTitles { get; set; }
    public ICollection<DbNameProfession>? Professions { get; set; }
}
