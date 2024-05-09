create table lookup_genres
(
    id    integer      not null primary key,
    genre varchar(255) not null
);

create table lookup_professions
(
    id         integer      not null primary key,
    profession varchar(255) not null
);

create table lookup_title_types
(
    id        integer     not null primary key,
    titleType varchar(80) not null
);

create table lookup_principal_categories
(
    id       integer      not null primary key,
    category varchar(255) not null
);

create table lookup_principal_jobs
(
    id  integer      not null primary key,
    job varchar(255) not null
);

create table lookup_principal_characters
(
    id            integer      not null primary key,
    characterName varchar(512) not null
);

create table name_basics
(
    id                integer      not null primary key,
    primaryName       varchar(255) not null,
    birthYear         integer      null,
    deathYear         integer      null
);

create table name_known_for_titles
(
    nameId  integer not null,
    titleId integer not null,
    primary key (nameId, titleId)
);

create table name_professions
(
    nameId       integer not null,
    professionId integer not null,
    primary key (nameId, professionId)
);

create table title_akas
(
    titleId         integer      not null,
    ordering        integer      not null,
    title           varchar(768) null,
    region          varchar(20)  null,
    language        varchar(20)  null,
    types           varchar(255) null,
    attributes      varchar(255) null,
    isOriginalTitle bit          not null,
    primary key (titleId, ordering)
);

create table title_basics
(
    id             integer      not null primary key,
    titleType      integer      not null,
    primaryTitle   varchar(512) not null,
    originalTitle  varchar(512) null,
    isAdult        bit          not null,
    startYear      integer      null,
    endYear        integer      null,
    runtimeMinutes integer      null
);

create table title_episodes
(
    id             integer not null primary key,
    parentId       integer not null,
    seasonNumber   integer null,
    episodeNumber  integer null
);

create table title_genres
(
    titleId integer not null,
    genreId integer not null,
    primary key (titleId, genreId)
);

create table title_principals
(
    titleId         integer not null,
    ordering        integer not null,
    nameId          integer not null,
    category        integer null,
    job             integer null,
    characters      integer null,
    primary key (titleId, ordering)
);

create table title_ratings
(
    id             integer not null primary key,
    averageRating  decimal(5,2),
    numVotes       integer
);
