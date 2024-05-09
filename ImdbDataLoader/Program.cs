using ImdbDataLoader;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

Console.Write("Recreate database and continue? [y/N] ");
var c = char.ToUpper(Console.ReadKey().KeyChar);
Console.WriteLine();
if (c != 'Y')
    return 1;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connection = new MySqlConnection(config.GetConnectionString("imdb"));
await connection.OpenAsync();

var sourceFiles = config["sourceFiles"] ?? throw new Exception("Undefined config value sourceFiles");

var db = new ImdbDatabase(connection);
await db.DropAllTables();
await db.RecreateTables();

var writer = new ImdbWriter(connection);
var loader = new ImdbLoader(writer);

loader.LoadNameBasics(Path.Combine(sourceFiles, "name.basics.tsv.gz"));
loader.LoadTitleAkas(Path.Combine(sourceFiles, "title.akas.tsv.gz"));
loader.LoadTitleBasics(Path.Combine(sourceFiles, "title.basics.tsv.gz"));
loader.LoadTitleEpisodes(Path.Combine(sourceFiles, "title.episode.tsv.gz"));
loader.LoadTitlePrincipals(Path.Combine(sourceFiles, "title.principals.tsv.gz"));
loader.LoadTitleRatings(Path.Combine(sourceFiles, "title.ratings.tsv.gz"));

await db.PostProcess();

Console.WriteLine("All done");
return 0;

