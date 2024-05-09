using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("lookup_principal_characters")]
public class DbLookupPrincipalCharacter
{
    [Key]
    public int Id { get; set; }
    public string? CharacterName { get; set; }
}
