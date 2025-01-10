create table dbo.countries
(
    id   int          not null
        primary key,
    name varchar(100) not null
)
go

INSERT INTO PersonDB.dbo.countries (id, name) VALUES (1, N'New');
INSERT INTO PersonDB.dbo.countries (id, name) VALUES (2, N'Russian Series');
INSERT INTO PersonDB.dbo.countries (id, name) VALUES (3, N'Foreign Series');
INSERT INTO PersonDB.dbo.countries (id, name) VALUES (4, N'Russian Films');
INSERT INTO PersonDB.dbo.countries (id, name) VALUES (5, N'Foreign Films');
INSERT INTO PersonDB.dbo.countries (id, name) VALUES (6, N'Popular films in a month');
INSERT INTO PersonDB.dbo.countries (id, name) VALUES (7, N'Popular Series in a month');
INSERT INTO PersonDB.dbo.countries (id, name) VALUES (8, N'Popular cartoons in a month ');
