create table dbo.film_actors
(
    film_id  int not null
        references dbo.films,
    actor_id int not null
        references dbo.actors,
    primary key (film_id, actor_id)
)
go

INSERT INTO PersonDB.dbo.film_actors (film_id, actor_id) VALUES (1, 1);
