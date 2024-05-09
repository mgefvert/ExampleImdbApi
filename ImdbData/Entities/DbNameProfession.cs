using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("name_professions")]
public class DbNameProfession
{
    public int NameId { get; set; }
    public int ProfessionId { get; set; }
    
    public DbNameBasic? Name { get; set; }
    public DbLookupProfession? Profession { get; set; }
}
