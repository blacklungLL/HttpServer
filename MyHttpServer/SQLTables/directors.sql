create table dbo.directors
(
    id   int          not null
        primary key,
    name varchar(255) not null
)
go

INSERT INTO PersonDB.dbo.directors (id, name) VALUES (1, N'Shawn Levy');
