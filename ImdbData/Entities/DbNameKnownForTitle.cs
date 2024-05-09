using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("name_known_for_titles")]
public class DbNameKnownForTitle
{
    public int NameId { get; set; }
    public int TitleId { get; set; }
    
    public DbNameBasic? Name { get; set; }
    public DbTitleBasic? Title { get; set; }
}
