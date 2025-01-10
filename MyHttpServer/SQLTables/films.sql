create table dbo.films
(
    id            int           not null
        primary key,
    title         varchar(255)  not null,
    year          int           not null,
    video_quality varchar(50)   not null,
    kp_rating     decimal(3, 1) not null,
    country       varchar(100)  not null,
    duration      int           not null,
    cover_image   varchar(255)  not null,
    plot_summary  text          not null,
    director_id   int
        references dbo.directors
)
go

INSERT INTO PersonDB.dbo.films (id, title, year, video_quality, kp_rating, country, duration, cover_image, plot_summary, director_id) VALUES (1, N'Deadpool&Wolverine', 2024, N'HD, FullHD, 720, 1080, 2K, 4K', 7.4, N'USA', 127, N'../assets/images/Deadpool_&_Wolverine.png', N'Wade Wilson is taken into the organization "Time Change Management", which forces him to return to his alter ego Deadpool and change history with the help of Wolverine.

Here watch Deadpool and Wolverine movie online for free in good quality', 1);
