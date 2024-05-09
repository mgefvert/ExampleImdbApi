using ImdbData.Entities;
using ImdbData.GenericServices;

namespace ImdbData.Interfaces;

public class TitleBasicQuery
{
    public int? Year { get; set; }
    public int? Start { get; set; }
    public int? Max { get; set; }
}

public interface ITitleBasicService : ICrudService<int, DbTitleBasic, TitleBasicQuery>
{
}
