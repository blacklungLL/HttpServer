create table dbo.genres
(
    id   int          not null
        primary key,
    name varchar(100) not null
)
go

INSERT INTO PersonDB.dbo.genres (id, name) VALUES (1, N'Comedy');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (2, N'Horror');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (3, N'Detective');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (4, N'Crime');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (5, N'Science Fiction');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (6, N'Thriller');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (7, N'Action');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (8, N'Drama');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (9, N'Romance');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (10, N'Adventure');
INSERT INTO PersonDB.dbo.genres (id, name) VALUES (11, N'Fantasy');
