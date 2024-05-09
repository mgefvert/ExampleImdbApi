using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("title_ratings")]
public class DbTitleRating
{
    [Key]
    public int Id { get; set; }
    public decimal? AverageRating { get; set; }
    public int? NumVotes { get; set; }
}
