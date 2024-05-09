using ImdbData.Entities;
using ImdbData.GenericServices;
using ImdbData.Interfaces;

namespace ImdbData.Services;

public class NameBasicService : CrudService<int, DbNameBasic, ImdbContext, NameBasicQuery>, INameBasicService
{
    public NameBasicService(ImdbContext imdbContext) 
        : base(imdbContext)
    {
        AllowedOperations = CrudOperations.All;
    }

    protected override DbNameBasic AssignObject(DbNameBasic existing, DbNameBasic source)
    {
        existing.PrimaryName = source.PrimaryName;
        existing.BirthYear   = source.BirthYear;
        existing.DeathYear   = source.DeathYear;
        return existing;
    }

    protected override int GetObjectKey(DbNameBasic value) => value.Id;

    protected override IQueryable<DbNameBasic> LoadQuery(int[]? ids = null, NameBasicQuery? query = null)
    {
        IQueryable<DbNameBasic> result = Context.NameBasics;

        if (ids != null)
            result = result.Where(x => ids.Contains(x.Id));
        if (query?.Year != null)
            result = result.Where(x => x.BirthYear == query.Year);

        result = result.OrderBy(x => x.PrimaryName);
        
        return Helper.AddLimiter(result, query?.Start, query?.Max);
    }
}
