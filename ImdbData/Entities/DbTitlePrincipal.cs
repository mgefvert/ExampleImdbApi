using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("title_principals")]
public class DbTitlePrincipal
{
    public int TitleId { get; set; }
    public int Ordering { get; set; }
    public int NameId { get; set; }
    public int? Category { get; set; }
    public int? Job { get; set; }
    public int? Characters { get; set; }
}
