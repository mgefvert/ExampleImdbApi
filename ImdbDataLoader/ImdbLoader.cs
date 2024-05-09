using ImdbData.Entities;

namespace ImdbDataLoader;

public class ImdbLoader
{
    private readonly ImdbWriter _writer;

    private readonly Dictionary<string, int> _genres = new(StringComparer.CurrentCultureIgnoreCase);
    private readonly Dictionary<string, int> _professions = new(StringComparer.CurrentCultureIgnoreCase);
    private readonly Dictionary<string, int> _titleTypes = new(StringComparer.CurrentCultureIgnoreCase);

    private readonly Dictionary<string, int> _principalCategories = new(StringComparer.CurrentCultureIgnoreCase);
    private readonly Dictionary<string, int> _principalCharacters = new(StringComparer.CurrentCultureIgnoreCase);
    private readonly Dictionary<string, int> _principalJobs = new(StringComparer.CurrentCultureIgnoreCase);

    private static string? MaxLen(string? value, int maxLen) => value != null && value.Length > maxLen ? value[..maxLen] : value;
    private static bool ParseBool(string value) => int.Parse(value) != 0;
    private static int? ParseInt(string value) => value == "\\N" ? null : int.Parse(value);
    private static decimal? ParseDecimal(string value) => value == "\\N" ? null : decimal.Parse(value);
    private static string? ParseStr(string value) => value == "\\N" ? null : value;
    private static string ParseStrRequired(string value) =>
        value == "\\N" ? throw new Exception("Value required but \\N found") : value;

    public ImdbLoader(ImdbWriter writer)
    {
        _writer = writer;
    }
    
    private int ParseKey(string prefix, string value)
    {
        value = ParseStrRequired(value);
        if (value.StartsWith(prefix))
            return int.Parse(value.Substring(prefix.Length));

        throw new Exception($"Expected '{prefix}' key but found '{value}'");
    }

    public void LoadNameBasics(string fileName)
    {
        var professionCounter = 0;
        
        LoadRecords("NameBasics", fileName, fields =>
        {
            // Load NameBasic record and save
            var nameId = ParseKey("nm", fields[0]);
            _writer.NameBasics.Add(new DbNameBasic
            {
                Id             = nameId,
                PrimaryName    = ParseStr(fields[1]) ?? "",
                BirthYear      = ParseInt(fields[2]),
                DeathYear      = ParseInt(fields[3]),
            });

            // Investigate profession field
            var professions = ParseStr(fields[4])?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (professions != null && professions.Any())
            {
                // See if the index entry exists
                foreach (var profession in professions)
                    if (!_professions.ContainsKey(profession))
                    {
                        _professions.Add(profession, ++professionCounter);
                        _writer.LookupProfessions.Add(new DbLookupProfession
                        {
                            Id = professionCounter,
                            Profession = profession
                        });
                    }

                // Write the 1-M record
                _writer.NameProfessions.AddRange(professions
                    .Select(n => new DbNameProfession
                    {
                        NameId       = nameId,
                        ProfessionId = _professions[n]
                    })
                );
            }

            var knownForTitles = ParseStr(fields[5])?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (knownForTitles != null && knownForTitles.Any())
                _writer.NameKnownForTitles.AddRange(knownForTitles
                    .Select(n => new DbNameKnownForTitle
                    {
                        NameId  = nameId,
                        TitleId = ParseKey("tt", n),
                    })
                );
        });
    }

    public void LoadTitleAkas(string fileName)
    {
        List<string?> allowed = ["US", "GB", "FR", "ES", "DK", "SE", "IT", "JP", "DE"];
        
        LoadRecords("TitleAkas", fileName, fields =>
        {
            var region = ParseStr(fields[3]);
            if (!allowed.Contains(region))
                return;
            
            _writer.TitleAkas.Add(new DbTitleAka
            {
                TitleId         = ParseKey("tt", fields[0]),
                Ordering        = int.Parse(fields[1]),
                Title           = MaxLen(ParseStr(fields[2]), 512),
                Region          = region,
                Language        = ParseStr(fields[4]),
                Types           = ParseStr(fields[5]),
                Attributes      = ParseStr(fields[6]),
                IsOriginalTitle = ParseBool(fields[7])
            });
        });
    }

    public void LoadTitleBasics(string fileName)
    {
        var genreCounter = 0;
        var titleTypeCounter = 0;
        
        LoadRecords("TitleBasics", fileName, fields =>
        {
            // Load main record
            var id = ParseKey("tt", fields[0]);
            var primaryTitle = ParseStrRequired(fields[2]);
            var originalTitle = ParseStr(fields[3]);
            if (originalTitle == primaryTitle)
                originalTitle = null;
            
            // Load and parse genres
            var genres = ParseStr(fields[8])?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (genres != null && genres.Any())
            {
                // Add to index if it doesn't exist
                foreach (var genre in genres)
                    if (!_genres.ContainsKey(genre))
                    {
                        _genres.Add(genre, ++genreCounter);
                        _writer.LookupGenres.Add(new DbLookupGenre
                        {
                            Id = genreCounter,
                            Genre = genre
                        });
                    }

                // Create the 1-M records
                _writer.TitleGenres.AddRange(genres.Select(n => new DbTitleGenre
                {
                    TitleId = id,
                    GenreId = _genres[n]
                }));
            }

            // Load and index title type
            var titleType = ParseStr(fields[1]) ?? throw new Exception("Undefined titleType");
            if (!_titleTypes.ContainsKey(titleType))
            {
                _titleTypes.Add(titleType, ++titleTypeCounter);
                _writer.LookupTitleTypes.Add(new DbLookupTitleType
                {
                    Id = titleTypeCounter,
                    TitleType = titleType
                });
            }

            // Save main record
            _writer.TitleBasics.Add(new DbTitleBasic
            {
                Id             = id,
                TitleType      = _titleTypes[titleType],
                PrimaryTitle   = primaryTitle,
                OriginalTitle  = originalTitle,
                IsAdult        = ParseBool(fields[4]),
                StartYear      = ParseInt(fields[5]),
                EndYear        = ParseInt(fields[6]),
                RuntimeMinutes = ParseInt(fields[7])
            });
        });
    }

    public void LoadTitleEpisodes(string fileName)
    {
        LoadRecords("TitleEpisodes", fileName, fields =>
        {
            _writer.TitleEpisodes.Add(new DbTitleEpisode
            {
                Id            = ParseKey("tt", fields[0]),
                ParentId      = ParseKey("tt", fields[1]),
                SeasonNumber  = ParseInt(fields[2]),
                EpisodeNumber = ParseInt(fields[3])
            });
        });
    }

    public void LoadTitlePrincipals(string fileName)
    {
        var categoryCounter = 0;
        var characterCounter = 0;
        var jobCounter = 0;
        
        LoadRecords("TitlePrincipals", fileName, fields =>
        {
            var category = MaxLen(ParseStr(fields[3]), 255);
            if (!string.IsNullOrEmpty(category) && !_principalCategories.ContainsKey(category))
            {
                _principalCategories.Add(category, ++categoryCounter);
                _writer.LookupPrincipalCategories.Add(new DbLookupPrincipalCategory
                {
                    Id = categoryCounter,
                    Category = category
                });
            }

            var job = MaxLen(ParseStr(fields[4]), 255);
            if (!string.IsNullOrEmpty(job) && !_principalJobs.ContainsKey(job))
            {
                _principalJobs.Add(job, ++jobCounter);
                _writer.LookupPrincipalJobs.Add(new DbLookupPrincipalJob
                {
                    Id  = jobCounter,
                    Job = job
                });
            }

            var character = MaxLen(ParseStr(fields[5]), 512)
                ?.Replace("[", "")
                .Replace("]", "")
                .Replace("\"", "");
            if (!string.IsNullOrEmpty(character) && !_principalCharacters.ContainsKey(character))
            {
                _principalCharacters.Add(character, ++characterCounter);
                _writer.LookupPrincipalCharacters.Add(new DbLookupPrincipalCharacter
                {
                    Id            = characterCounter,
                    CharacterName = character
                });
            }

            _writer.TitlePrincipals.Add(new DbTitlePrincipal
            {
                TitleId    = ParseKey("tt", fields[0]),
                Ordering   = int.Parse(fields[1]),
                NameId     = ParseKey("nm", fields[2]),
                Category   = !string.IsNullOrEmpty(category) ? _principalCategories[category] : null,
                Job        = !string.IsNullOrEmpty(job) ? _principalJobs[job] : null,
                Characters = !string.IsNullOrEmpty(character) ? _principalCharacters[character] : null
            });
        });
    }

    public void LoadTitleRatings(string fileName)
    {
        LoadRecords("TitleRatings", fileName, fields =>
        {
            _writer.TitleRating.Add(new DbTitleRating
            {
                Id            = ParseKey("tt", fields[0]),
                AverageRating = ParseDecimal(fields[1]),
                NumVotes      = ParseInt(fields[2])
            });
        });
    }

    private void LoadRecords(string name, string fileName, Action<string[]> processObject)
    {
        using var reader = new GzipFileReader(fileName);

        var c = 0;
        foreach (var fields in reader.Read(true))
        {
            try
            {
                c++;
                processObject(fields);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Line {c}: {ex.Message} -> data={string.Join("; ", fields)}");
            }

            if (c % 10000 == 0)
                Save();
        }

        Save();
        Console.WriteLine($"{name}: complete, {c:N0} records saved");
        return;

        void Save()
        {
            Console.Write($"{name}: {c:N0} ({reader.PercentRead:P1})...\r");
            _writer.Persist();
        }
    }
}