alter table lookup_principal_categories add index ixCategory (category);
alter table lookup_principal_characters add index ixCharacterName (characterName(30));
alter table lookup_principal_jobs add index ixJob (job);
alter table lookup_professions add index ixProfession (profession);

alter table name_basics add index ixPrimaryName (primaryName(30));
alter table name_basics add index ixBirthYear (birthYear);
alter table name_basics add index ixDeathYear (deathYear);

alter table name_known_for_titles add index ixReverse (titleId, nameId);

alter table name_professions add index ixReverse (professionId, nameId);

alter table title_basics add index ixPrimaryTitle (primaryTitle(30));
alter table title_basics add index ixOriginalTitle (originalTitle(30));
alter table title_basics add index ixStartYear (startYear);

alter table title_episodes add index ixParentId (parentId);

alter table title_genres add index ixReverse (genreId, titleId);

alter table title_principals add index ixNameId (nameId);
alter table title_principals add index ixCharacter (characters);

alter table title_ratings add index ixAverageRating (averageRating);
alter table title_ratings add index ixNumVotes (numVotes);
