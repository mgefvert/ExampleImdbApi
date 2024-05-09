using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using Dapper;
using ImdbData.Entities;
using MySqlConnector;

namespace ImdbDataLoader;

public class ImdbWriter
{
    private readonly MySqlConnection _connection;

    public List<DbLookupGenre> LookupGenres { get; } = [];
    public List<DbLookupProfession> LookupProfessions { get; } = [];
    public List<DbLookupTitleType> LookupTitleTypes { get; } = [];
    public List<DbLookupPrincipalCategory> LookupPrincipalCategories { get; } = [];
    public List<DbLookupPrincipalCharacter> LookupPrincipalCharacters { get; } = [];
    public List<DbLookupPrincipalJob> LookupPrincipalJobs { get; } = [];
    public List<DbNameBasic> NameBasics { get; } = [];
    public List<DbNameKnownForTitle> NameKnownForTitles { get; } = [];
    public List<DbNameProfession> NameProfessions { get; } = [];
    public List<DbTitleAka> TitleAkas { get; } = [];
    public List<DbTitleBasic> TitleBasics { get; } = [];
    public List<DbTitleEpisode> TitleEpisodes { get; } = [];
    public List<DbTitleGenre> TitleGenres { get; } = [];
    public List<DbTitlePrincipal> TitlePrincipals { get; } = [];
    public List<DbTitleRating> TitleRating { get; } = [];

    public ImdbWriter(MySqlConnection connection)
    {
        _connection = connection;
    }

    public void Persist()
    {
        PersistObjects(LookupGenres);
        PersistObjects(LookupProfessions);
        PersistObjects(LookupTitleTypes);
        PersistObjects(LookupPrincipalCategories);
        PersistObjects(LookupPrincipalCharacters);
        PersistObjects(LookupPrincipalJobs);
        PersistObjects(NameBasics);
        PersistObjects(NameKnownForTitles);
        PersistObjects(NameProfessions);
        PersistObjects(TitleAkas);
        PersistObjects(TitleBasics);
        PersistObjects(TitleEpisodes);
        PersistObjects(TitleGenres);
        PersistObjects(TitlePrincipals);
        PersistObjects(TitleRating);
    }

    private void PersistObjects<T>(List<T> list) where T : class
    {
        if (list.Count == 0)
            return;
        
        var builder = new StringBuilder();
        var table = typeof(T).GetCustomAttribute<TableAttribute>()?.Name
                    ?? throw new Exception($"No table name defined for {list.GetType().Name}");
                    
        builder.Append($"insert into {table} values ");
        var strings = list.Select(EntitiesToSql.ToSql).ToArray();
        for (var i = 0; i < strings.Length; i++)
        {
            builder.Append('(');
            builder.Append(strings[i]);
            builder.Append(')');
            if (i < strings.Length - 1)
                builder.Append(',');
        }

        var sql = builder.ToString();
        list.Clear();
        _connection.Execute(sql);
    }
}