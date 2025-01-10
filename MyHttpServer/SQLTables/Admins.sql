create table dbo.Admins
(
    id       int          not null
        primary key,
    login    nvarchar(50) not null,
    password nvarchar(50) not null,
    email    nvarchar(50) not null
)
go

INSERT INTO PersonDB.dbo.Admins (id, login, password, email) VALUES (1, N'Admin', N'87654321', N'Admin');
