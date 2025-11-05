USE [master]
GO
IF EXISTS (SELECT * FROM sysdatabases WHERE name='Trackify')
BEGIN
ALTER DATABASE [Trackify] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
DROP database Trackify
END
GO
CREATE DATABASE Trackify
GO
USE Trackify

CREATE TABLE Users(
UserID int identity(1,1) primary key,
UserName nvarchar(50),
Nickname nvarchar(50),
HashPassword nvarchar(50),
Salt nvarchar(36),
FirstName nvarchar(50),
LastName nvarchar(50),
Email nvarchar(50),
Birthday date,
SubscriptionType int default '0' not null,
pfp nvarchar(255) default '/ImagesAndSongs/Users/Empty-User-pfp.png',
AccountAge date default getdate()
)

CREATE TABLE ArtistApplication(
ApplicationID int identity (1,1) primary key,
UserID int,
ApplicationDate date default getdate(),
ApplicationStatus nvarchar(50)
foreign key (UserID) references Users(UserID)
)

CREATE TABLE Artists(
ArtistID int identity (1,1) primary key,
UserID int,
Verification bit,
foreign key (UserID) references Users(UserID)
)

CREATE TABLE Album(
AlbumID int identity (1,1) primary key,
AlbumTitle nvarchar(255),
AlbumType nvarchar(50),
Artist int,
AlbumImage nvarchar(255) default '/ImagesAndSongs/Albums/Trackify-Album-Placeholder.png',
MadePrivate bit default 1,
foreign key (Artist) references Artists(ArtistID)
)

CREATE TABLE Genre(
GenreID int identity (1,1) primary key,
GenreName nvarchar(50)
)

CREATE TABLE Songs(
SongID int identity (1,1) primary key,
SongTitle nvarchar(255),
Artist int,
AlbumID int null,
SongLength decimal(18,2),
TimesListened int,
SoundFile nvarchar(255),
ThumbnailPath nvarchar(255) default '/ImagesAndSongs/Songs/Trackify-Song-Placeholder.png',
MadePrivate bit default 1,
GenreID int,
foreign key (GenreID) references Genre(GenreID),
foreign key (Artist) references Artists(ArtistID),
foreign key (AlbumID) references Album(AlbumID)
)

CREATE TABLE Playlist(
PlaylistID int identity (1,1) primary key,
UserID int,
PlaylistName nvarchar(50),
MadePrivate bit default 1,
foreign key (UserID) references Users(UserID)
)

CREATE TABLE PlaylistSongs(
SongID int,
PlaylistID int,
PlaylistImage nvarchar(255) null,
foreign key (SongID) references Songs(SongID),
foreign key (PlaylistID) references Playlist(PlaylistID)
)

CREATE TABLE Subscriptions(
SubscriptionID int identity (1,1),
UserID int,
SubscriptionStart date default getdate(),
SubscriptionEnd date,
foreign key (UserID) references Users(UserID)
)
GO
----------------------------------------------------------Procedures-----------------------------------------------------

CREATE OR ALTER PROCEDURE CreateUserSP 	
	@Username nvarchar(50),
	@Password nvarchar(50),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Birthday date,
	@Subscription int
AS
	SET NOCOUNT ON;
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()

	INSERT INTO Users (Username,Nickname,HashPassword,Salt,FirstName,LastName,Email,Birthday,SubscriptionType)
	VALUES (@Username, @Username, HASHBYTES('SHA2_512',@Password+CAST(@Salt AS NVARCHAR(36))), @Salt, @FirstName,@LastName,@Email,@Birthday,@Subscription) SELECT SCOPE_IDENTITY() AS UserID;
GO

CREATE OR ALTER PROCEDURE LoginUsernameSP
	@Username nvarchar(50),
	@Password nvarchar(50)
AS
	SET NOCOUNT ON
	SELECT UserID FROM Users WHERE Username=@Username AND HashPassword=HASHBYTES('SHA2_512', @Password+CAST(Salt AS nvarchar(36))) SELECT SCOPE_IDENTITY() AS UserID
GO

CREATE OR ALTER PROCEDURE LoginEmailSP
	@Email nvarchar(50),
	@Password nvarchar(50)
AS
	SET NOCOUNT ON
	SELECT UserID FROM Users WHERE Email=@Email AND HashPassword=HASHBYTES('SHA2_512', @Password+CAST(Salt AS nvarchar(36))) SELECT SCOPE_IDENTITY() AS UserID
GO


CREATE OR ALTER PROCEDURE GetUserByIDSP
	@UserID int
AS
	SET NOCOUNT ON
	SELECT * FROM Users WHERE UserID=@UserID
GO

CREATE OR ALTER PROCEDURE GetUserByUsernameSP
	@Username int
AS
	SET NOCOUNT ON
	SELECT * FROM Users WHERE Username=@Username
GO

CREATE OR ALTER PROCEDURE GetUserByEmailSP
	@Email int
AS
	SET NOCOUNT ON
	SELECT * FROM Users WHERE Email=@Email
GO


CREATE OR ALTER PROCEDURE UpdateUserSP
	@UserID int,
	@Username nvarchar(50),
	@Nickname nvarchar(50),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Birthday datetime,
	@pfp nvarchar(255),
	@SubscriptionType bit
AS
	SET NOCOUNT ON
	UPDATE Users 
	SET Username=@Username, Nickname=@Nickname, FirstName=@FirstName, LastName=@LastName, Email=@Email, Birthday=@Birthday, pfp=@pfp, SubscriptionType=@SubscriptionType 
	WHERE UserID=@UserID
GO

CREATE OR ALTER PROCEDURE UpdatePasswordSP
	@UserID int,
	@Password nvarchar(50)
AS
	SET NOCOUNT ON
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()

	UPDATE Users
	SET HashPassword=HASHBYTES('SHA2_512',@Password+CAST(@Salt AS NVARCHAR(36))), Salt=@Salt
	WHERE UserID=@UserID
GO