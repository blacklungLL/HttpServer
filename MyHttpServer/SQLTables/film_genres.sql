create table dbo.film_genres
(
    film_id  int not null
        references dbo.films,
    genre_id int not null
        references dbo.countries,
    primary key (film_id, genre_id)
)
go

INSERT INTO PersonDB.dbo.film_genres (film_id, genre_id) VALUES (1, 1);
