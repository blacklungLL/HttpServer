create table dbo.movies
(
    id          int          not null
        primary key,
    title       varchar(255) not null,
    cover_image varchar(255) not null,
    Year        int
)
go

INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (1, N'Terrifying 3', N'../assets/images/terrifier_3.png', 2023);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (2, N'three cats', N'../assets/images/cats.webp', 2015);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (3, N'True Detective', N'../assets/images/True-Detective.jpg', 2012);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (4, N'Earth abides', N'../assets/images/earth.jpg', 2014);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (5, N'Black materia', N'../assets/images/black.jpeg', 2017);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (6, N'Soprano 6 season', N'../assets/images/Soprano.jpg', 2019);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (7, N'Stay Alive', N'../assets/images/alive.webp', 2022);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (8, N'Sanditon 3 season', N'../assets/images/Sanditon.jpg', 2020);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (9, N'Dexter 8 season', N'../assets/images/dexter.jpeg', 2024);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (10, N'My name is Erl', N'../assets/images/Erl.webp', 2022);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (11, N'Vampire Diaries', N'../assets/images/vampire.jpg', 2021);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (12, N'Acolyte 1 season', N'../assets/images/acolyte.jpg', 2018);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (13, N'Diavolik', N'../assets/images/diabolik.jpeg', 2013);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (14, N'Fake police', N'../assets/images/police.jpg', 2016);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (15, N'Secret Level', N'../assets/images/secret.webp', 2017);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (16, N'A hundred years alone', N'../assets/images/hundred.webp', 2020);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (17, N'Dark materia', N'../assets/images/black.jpeg', 2018);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (18, N'Big Little Lies', N'../assets/images/lie.webp', 2020);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (19, N'The breakout Return', N'../assets/images/Escape.jpg', 2021);
INSERT INTO PersonDB.dbo.movies (id, title, cover_image, Year) VALUES (20, N'Angry: story of a withc from West', N'../assets/images/witch.webp', 2016);
