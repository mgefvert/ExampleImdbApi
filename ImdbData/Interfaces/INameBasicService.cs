using ImdbData.Entities;
using ImdbData.GenericServices;

namespace ImdbData.Interfaces;

public class NameBasicQuery
{
    public int? Year { get; set; }
    public int? Start { get; set; }
    public int? Max { get; set; }
}

public interface INameBasicService : ICrudService<int, DbNameBasic, NameBasicQuery>
{
}
