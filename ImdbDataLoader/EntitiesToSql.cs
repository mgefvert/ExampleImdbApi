using System.Globalization;
using ImdbData.Entities;
using MySqlConnector;

namespace ImdbDataLoader;

public static class EntitiesToSql
{
    public static string ToSql(params object?[] values)
    {
        return string.Join(",", values.Select(ToSql));
    }

    public static string ToSql(object? value)
    {
        return value switch
        {
            null                          => "null",
            int n                         => n.ToString(),
            bool b                        => b ? "1" : "0",
            string s                      => "'" + MySqlHelper.EscapeString(s) + "'",
            decimal d                     => d.ToString(CultureInfo.InvariantCulture),
            DbLookupGenre v               => ToSql(v.Id, v.Genre),
            DbLookupProfession v          => ToSql(v.Id, v.Profession),
            DbLookupTitleType v           => ToSql(v.Id, v.TitleType),
            DbLookupPrincipalCategory v   => ToSql(v.Id, v.Category),
            DbLookupPrincipalCharacter v => ToSql(v.Id, v.CharacterName),
            DbLookupPrincipalJob v        => ToSql(v.Id, v.Job),
            DbNameBasic v                 => ToSql(v.Id, v.PrimaryName, v.BirthYear, v.DeathYear),
            DbNameKnownForTitle v         => ToSql(v.NameId, v.TitleId),
            DbNameProfession v            => ToSql(v.NameId, v.ProfessionId),
            DbTitleAka v                  => ToSql(v.TitleId, v.Ordering, v.Title, v.Region, v.Language, v.Types, v.Attributes, v.IsOriginalTitle),
            DbTitleBasic v                => ToSql(v.Id, v.TitleType, v.PrimaryTitle, v.OriginalTitle, v.IsAdult, v.StartYear, v.EndYear, v.RuntimeMinutes),
            DbTitleEpisode v              => ToSql(v.Id, v.ParentId, v.SeasonNumber, v.EpisodeNumber),
            DbTitleGenre v                => ToSql(v.TitleId, v.GenreId),
            DbTitlePrincipal v            => ToSql(v.TitleId, v.Ordering, v.NameId, v.Category, v.Job, v.Characters),
            DbTitleRating v               => ToSql(v.Id, v.AverageRating, v.NumVotes),
            _                             => throw new Exception($"ValueToSql: Unrecognized type {value.GetType().Name}: '{value}'")
        };
    }}