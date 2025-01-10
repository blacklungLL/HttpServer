create table dbo.categories
(
    id   int         not null
        primary key,
    name varchar(55) not null
)
go

INSERT INTO PersonDB.dbo.categories (id, name) VALUES (1, N'Films');
INSERT INTO PersonDB.dbo.categories (id, name) VALUES (2, N'TV Series');
INSERT INTO PersonDB.dbo.categories (id, name) VALUES (3, N'Cartoons');
INSERT INTO PersonDB.dbo.categories (id, name) VALUES (4, N'4K');
INSERT INTO PersonDB.dbo.categories (id, name) VALUES (5, N'Random Film');
