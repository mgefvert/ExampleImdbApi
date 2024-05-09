using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("title_basics")]
public class DbTitleBasic
{
    [Key]
    public int Id { get; set; }
    public int TitleType { get; set; }
    public string? PrimaryTitle { get; set; }
    public string? OriginalTitle { get; set; }
    public bool IsAdult { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public int? RuntimeMinutes { get; set; }

    public ICollection<DbTitleGenre>? Genres { get; set; }

    public override string ToString()
    {
        return string.Join(", ", new [] {
            $"#{Id}, Type={TitleType}, Title={PrimaryTitle}",
            OriginalTitle != null ? $"OriginalTitle={OriginalTitle}" : null,
            $"Adult={IsAdult}, Year={StartYear}",
            EndYear != null ? $"EndYear={EndYear}" : null,
            $"Mins={RuntimeMinutes}"
        }.Where(x => x != null));
    }
}
