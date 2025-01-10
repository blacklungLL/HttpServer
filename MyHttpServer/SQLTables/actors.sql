create table dbo.actors
(
    id   int          not null
        primary key,
    name varchar(255) not null
)
go

INSERT INTO PersonDB.dbo.actors (id, name) VALUES (1, N'Rayan Reynolds, Hugh Jackman, Emma Corrin, Morena Baccarin, Rob Delaney, Karan Soni, Leslie Uggams, Matthew Macfadyen, Tyler Mane, Jennifer Garner');
