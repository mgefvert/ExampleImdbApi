using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImdbData.Entities;

[Table("title_episodes")]
public class DbTitleEpisode
{
    [Key]
    public int Id { get; set; }
    public int ParentId { get; set; }
    public int? SeasonNumber { get; set; }
    public int? EpisodeNumber { get; set; }
}
