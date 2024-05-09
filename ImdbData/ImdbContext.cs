using ImdbData.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImdbData;

public class ImdbContext : DbContext
{
    public ImdbContext(DbContextOptions<ImdbContext> options) : base(options)
    {
    }
    
    public DbSet<DbLookupGenre> LookupGenres { get; set; } = null!;
    public DbSet<DbLookupProfession> LookupProfessions { get; set; } = null!;
    public DbSet<DbLookupTitleType> LookupTitleTypes { get; set; } = null!;
    public DbSet<DbNameBasic> NameBasics { get; set; } = null!;
    public DbSet<DbNameKnownForTitle> NameKnownForTitles { get; set; } = null!;
    public DbSet<DbNameProfession> NameProfessions { get; set; } = null!;
    public DbSet<DbTitleAka> TitleAkas { get; set; } = null!;
    public DbSet<DbTitleBasic> TitleBasics { get; set; } = null!;
    public DbSet<DbTitleEpisode> TitleEpisodes { get; set; } = null!;
    public DbSet<DbTitleGenre> TitleGenres { get; set; } = null!;
    public DbSet<DbTitlePrincipal> TitlePrincipals { get; set; } = null!;
    public DbSet<DbTitleRating> TitleRating { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DbNameKnownForTitle>(e => e.HasKey(nameof(DbNameKnownForTitle.NameId), nameof(DbNameKnownForTitle.TitleId)));
        modelBuilder.Entity<DbNameProfession>(e => e.HasKey(nameof(DbNameProfession.NameId), nameof(DbNameProfession.ProfessionId)));
        modelBuilder.Entity<DbTitleAka>(e => e.HasKey(nameof(DbTitleAka.TitleId), nameof(DbTitleAka.Ordering)));
        modelBuilder.Entity<DbTitleGenre>(e => e.HasKey(nameof(DbTitleGenre.TitleId), nameof(DbTitleGenre.GenreId)));
        modelBuilder.Entity<DbTitlePrincipal>(e => e.HasKey(nameof(DbTitlePrincipal.TitleId), nameof(DbTitlePrincipal.Ordering)));
    }
}