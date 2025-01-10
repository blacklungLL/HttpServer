create table dbo.popular
(
    id          int          not null
        primary key,
    title       varchar(255) not null,
    cover_image varchar(255) not null,
    year        int
)
go

INSERT INTO PersonDB.dbo.popular (id, title, cover_image, year) VALUES (1, N'Substance', N'../assets/images/substance.png', 2024);
INSERT INTO PersonDB.dbo.popular (id, title, cover_image, year) VALUES (2, N'Terrifying 3', N'../assets/images/terrifier_3.png', 2023);
INSERT INTO PersonDB.dbo.popular (id, title, cover_image, year) VALUES (3, N'Trap', N'../assets/images/trap.png', 2022);
INSERT INTO PersonDB.dbo.popular (id, title, cover_image, year) VALUES (4, N'Hotel Belgrad', N'../assets/images/hotel.png', 2022);
INSERT INTO PersonDB.dbo.popular (id, title, cover_image, year) VALUES (5, N'21 Bridge', N'../assets/images/21_bridges.png', 2021);
