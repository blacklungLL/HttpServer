create table dbo.Users
(
    Id       int not null
        primary key,
    Login    nvarchar(50),
    Password nvarchar(50),
    Email    nvarchar(50)
)
go

INSERT INTO PersonDB.dbo.Users (Id, Login, Password, Email) VALUES (1, N'test', N'12345678', N'test');
