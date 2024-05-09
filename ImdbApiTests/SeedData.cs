using ImdbData;
using ImdbData.Entities;

namespace ImdbApiTests;

public class SeedData
{
    private readonly ImdbContext _context;

    public SeedData(ImdbContext context)
    {
        _context = context;
    }

    public async Task Clear()
    {
        _context.RemoveRange(_context.LookupGenres);
        _context.RemoveRange(_context.LookupProfessions);
        _context.RemoveRange(_context.LookupTitleTypes);
        _context.RemoveRange(_context.NameBasics);
        _context.RemoveRange(_context.NameProfessions);
        _context.RemoveRange(_context.NameKnownForTitles);
        _context.RemoveRange(_context.TitleAkas);
        _context.RemoveRange(_context.TitleBasics);
        _context.RemoveRange(_context.TitleEpisodes);
        _context.RemoveRange(_context.TitleGenres);
        _context.RemoveRange(_context.TitlePrincipals);
        _context.RemoveRange(_context.TitleRating);
        
        await _context.SaveChangesAsync();
    }

    public async Task Seed()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        
        _context.AddRange(CreateLookupGenres());
        _context.AddRange(CreateProfessions());
        _context.AddRange(CreateTitleTypes());
        
        await _context.SaveChangesAsync();
    }

    private object[] CreateLookupGenres() =>
    [
        new DbLookupGenre { Id = 1, Genre = "Comedy" },
        new DbLookupGenre { Id = 2, Genre = "Science Fiction" },
        new DbLookupGenre { Id = 3, Genre = "Drama" },
        new DbLookupGenre { Id = 4, Genre = "Biography" }
    ];

    private object[] CreateProfessions() =>
    [
        new DbLookupProfession { Id = 1, Profession = "Actor" },
        new DbLookupProfession { Id = 2, Profession = "Actress" },
        new DbLookupProfession { Id = 3, Profession = "Director" },
        new DbLookupProfession { Id = 4, Profession = "Producer" }
    ];

    private object[] CreateTitleTypes() =>
    [
        new DbLookupTitleType { Id = 1, TitleType = "Movie" },
        new DbLookupTitleType { Id = 2, TitleType = "Short" },
        new DbLookupTitleType { Id = 3, TitleType = "TvSeries" }
    ];
}