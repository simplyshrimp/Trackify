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
AccountAge date default getdate(),
Color nvarchar(50) default '#121212'
)

CREATE TABLE ArtistApplication(
ApplicationID int identity (1,1) primary key,
UserID int,
ApplicationDate date default getdate(),
ApplicationStatus int default 0,
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
AlbumType int,
Artist int,
AlbumImage nvarchar(255) default '/ImagesAndSongs/Albums/Trackify-Album-Placeholder.png',
MadePrivate bit default 1,
Color nvarchar(50) default '#121212',
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
-------------------------Users---------------------------------
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
	@Username nvarchar(50)
AS
	SET NOCOUNT ON
	SELECT * FROM Users WHERE Username=@Username
GO

CREATE OR ALTER PROCEDURE GetUserByEmailSP
	@Email nvarchar(50)
AS
	SET NOCOUNT ON
	SELECT * FROM Users WHERE Email=@Email
GO


CREATE OR ALTER PROCEDURE UpdateUserSP
	@UserID int,
	@Username nvarchar(50),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(50),
	@Birthday datetime
AS
	SET NOCOUNT ON
	UPDATE Users 
	SET Username=@Username, FirstName=@FirstName, LastName=@LastName, Email=@Email, Birthday=@Birthday 
	WHERE UserID=@UserID
GO

CREATE OR ALTER PROCEDURE UpdateProfileSP
	@UserID int,
	@Nickname nvarchar(50),
	@pfp nvarchar(255),
	@Color nvarchar(50)
AS
	SET NOCOUNT ON
	UPDATE Users 
	SET Nickname=@Nickname, pfp=@pfp, Color=@Color
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
-----------------Applications-----------------
CREATE OR ALTER PROCEDURE CreateApplicationSP
	@UserID int
AS
	SET NOCOUNT ON

	INSERT INTO ArtistApplication (UserID) 
	VALUES (@UserID)
GO

CREATE OR ALTER PROCEDURE ShowAllApplicationsSP
AS
	SET NOCOUNT ON

	SELECT * FROM ArtistApplication
GO

CREATE OR ALTER PROCEDURE ShowApplicationByIDSP
	@UserID int
AS
	SET NOCOUNT ON

	SELECT * FROM ArtistApplication WHERE UserID=@UserID
GO

CREATE OR ALTER PROCEDURE ChangeApplicationStatusSP
	@UserID int,
	@Status int
AS
	SET NOCOUNT ON

	UPDATE ArtistApplication
	SET ApplicationStatus=@Status
	WHERE UserID=@UserID
GO
--------------------Artist------------------
--------------------Album-------------------
CREATE OR ALTER PROCEDURE CreateAlbumSP
	@Artist int,
	@AlbumTitle nvarchar(255),
	@AlbumType int,
	@Color nvarchar(50)
AS
	SET NOCOUNT ON

	INSERT INTO Album (AlbumTitle, AlbumType, Artist, Color)
	VALUES (@AlbumTitle, @AlbumType, @Artist, @Color) SELECT SCOPE_IDENTITY() AS AlbumID 
GO

CREATE OR ALTER PROCEDURE UpdateAlbumSP
	@AlbumID int,
	@AlbumTitle nvarchar(255),
	@AlbumType int,
	@Artist int,
	@AlbumImage nvarchar(255),
	@MadePrivate bit,
	@Color nvarchar(50)
AS
	SET NOCOUNT ON
	UPDATE Album
	SET AlbumTitle=@AlbumTitle, AlbumType=@AlbumType, Artist=@Artist, AlbumImage=@AlbumImage, MadePrivate=@MadePrivate, Color=@Color
	WHERE AlbumID=@AlbumID
GO
--------------------Songs-------------------
--------------------Playlist----------------
--------------------Genre-------------------
--------------------Subcriptions------------
