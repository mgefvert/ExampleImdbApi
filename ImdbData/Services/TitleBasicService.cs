using ImdbData.Entities;
using ImdbData.GenericServices;
using ImdbData.Interfaces;

namespace ImdbData.Services;

public class TitleBasicService : CrudService<int, DbTitleBasic, ImdbContext, TitleBasicQuery>, ITitleBasicService
{
    public TitleBasicService(ImdbContext imdbContext) 
        : base(imdbContext)
    {
        AllowedOperations = CrudOperations.All;
    }

    protected override DbTitleBasic AssignObject(DbTitleBasic existing, DbTitleBasic source)
    {
        existing.TitleType      = source.TitleType;
        existing.OriginalTitle  = source.OriginalTitle;
        existing.IsAdult        = source.IsAdult;
        existing.StartYear      = source.StartYear;
        existing.EndYear        = source.EndYear;
        existing.RuntimeMinutes = source.RuntimeMinutes;
        return existing;
    }

    protected override int GetObjectKey(DbTitleBasic value) => value.Id;

    protected override IQueryable<DbTitleBasic> LoadQuery(int[]? ids = null, TitleBasicQuery? query = null)
    {
        IQueryable<DbTitleBasic> result = Context.TitleBasics;

        if (ids != null)
            result = result.Where(x => ids.Contains(x.Id));
        if (query?.Year != null)
            result = result.Where(x => x.StartYear == query.Year);

        result = result.OrderBy(x => x.PrimaryTitle);

        return Helper.AddLimiter(result, query?.Start, query?.Max);
    }
}
