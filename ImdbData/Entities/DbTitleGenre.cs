using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("title_genres")]
public class DbTitleGenre
{
    public int TitleId { get; set; }
    public int GenreId { get; set; }
    
    public DbTitleBasic? Title { get; set; }
    public DbLookupGenre? Genre { get; set; }
}
