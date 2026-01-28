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
Color nvarchar(50) default '#121212',
IsAdmin bit default 0
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
Verification bit default 0,
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
ArtistID int,
AlbumID int null,
SongLength int,
TimesListened int default 0,
SoundFile nvarchar(255) null,
--ThumbnailPath nvarchar(255) default '/ImagesAndSongs/Songs/Trackify-Song-Placeholder.png',
MadePrivate bit default 1,
AlbumTrackNumber int default 0,
GenreID int null,
foreign key (GenreID) references Genre(GenreID),
foreign key (ArtistID) references Artists(ArtistID),
foreign key (AlbumID) references Album(AlbumID)
)

CREATE TABLE Playlist(
PlaylistID int identity (1,1) primary key,
UserID int,
PlaylistName nvarchar(50),
PlaylistImage nvarchar(255) null,
PlaylistColor nvarchar(50),
MadePrivate bit default 1,
foreign key (UserID) references Users(UserID) on delete cascade
)

CREATE TABLE PlaylistSongs(
PlaylistSongID int identity (1,1),
SongID int,
PlaylistID int,
foreign key (SongID) references Songs(SongID) on delete cascade, 
foreign key (PlaylistID) references Playlist(PlaylistID) on delete cascade
)

CREATE TABLE Subscriptions(
SubscriptionID int identity (1,1),
UserID int,
SubscriptionStart date default getdate(),
SubscriptionEnd date,
foreign key (UserID) references Users(UserID)
)
GO
----------------------------------------------------------Procedures--------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE CreateAdminUserSP
AS
	SET NOCOUNT ON;
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()

	INSERT INTO Users(Username,NickName,HashPassword,Salt,FirstName,LastName,Email,Birthday,SubscriptionType,pfp,IsAdmin)
VALUES ('Admin','Admin', HASHBYTES('SHA2_512','12345'+CAST(@Salt AS NVARCHAR(36))), @Salt,'Admin','Admin','admin@mail.com',CAST(getdate() AS date),1,'/ImagesAndSongs/Users/Empty-User-pfp.png',1)

GO
-------------------------Users---------------------------------------------------------------------------------------------------------------------------------------------------------
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

CREATE OR ALTER PROCEDURE GetAllUsersSP
AS
	SET NOCOUNT ON
	SELECT * FROM Users
GO
-----------------Applications---------------------------------------------------------------------------------------------------------------------------------------------------------
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
--------------------Artist----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE CreateArtistSP
	@UserID int
AS
	SET NOCOUNT ON

	INSERT INTO Artists (UserID)
	VALUES (@UserID) SELECT SCOPE_IDENTITY() AS ArtistID
GO

CREATE OR ALTER PROCEDURE ShowArtistByUserIDSP
	@UserID int
AS
	SELECT * FROM Artists WHERE UserID=@UserID
GO

CREATE OR ALTER PROCEDURE VerifyArtistSP
	@ArtistID int,
	@Verification bit
AS
	UPDATE Artists
	SET Verification=@Verification
	WHERE ArtistID=@ArtistID
GO

CREATE OR ALTER PROCEDURE ShowArtistByIDSP
	@ArtistID int
AS
	SET NOCOUNT ON
	SELECT * FROM Artists WHERE ArtistID=@ArtistID
GO

CREATE OR ALTER PROCEDURE ShowAllArtistsSP
AS
	SET NOCOUNT ON
	SELECT * FROM Artists
GO
--------------------Album-----------------------------------------------------------------------------------------------------------------------------------------------------------
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
CREATE OR ALTER PROCEDURE GetAlbumByIDSP
	@AlbumID int
AS
	SET NOCOUNT ON
	SELECT * FROM Album WHERE AlbumID=@AlbumID
GO

CREATE OR ALTER PROCEDURE ShowAllAlbumsSP
AS
	SET NOCOUNT ON
	SELECT * FROM Album
GO

CREATE OR ALTER PROCEDURE GetAllAlbumsByArtistSP
	@Artist int
AS
	SET NOCOUNT ON
	SELECT * FROM Album WHERE Artist=@Artist
GO

--------------------Songs-----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE CreateSongSP
	@SongTitle nvarchar(255),
	@ArtistID int,
	@AlbumID int,
	@SongLength int,
	@SoundFile nvarchar(255),
	@MadePrivate bit,
	@GenreID int
AS
	SET NOCOUNT ON
	INSERT INTO Songs(SongTitle,ArtistID,AlbumID,SongLength,SoundFile,MadePrivate,GenreID)
	VALUES (@SongTitle,@ArtistID,@AlbumID,@SongLength,@SoundFile,@MadePrivate,@GenreID)
GO

CREATE OR ALTER PROCEDURE UpdateSongSP
	@SongID int,
	@SongTitle nvarchar,
	@ArtistID int,
	@AlbumID int,
	@SongLength int,
	@SoundFile nvarchar(255),
	@MadePrivate bit,
	@GenreID int
AS
	SET NOCOUNT ON
	UPDATE Songs
	SET SongTitle=@SongTitle, ArtistID=@ArtistID, AlbumID=@AlbumID, SongLength=@SongLength, SoundFile=@SoundFile, MadePrivate=@MadePrivate, GenreID=@GenreID
	WHERE SongID=@SongID
GO

CREATE OR ALTER PROCEDURE GetSongsByAlbumSP
	@AlbumID int
AS
	SET NOCOUNT ON
	SELECT * FROM Songs WHERE AlbumID=@AlbumID
GO

CREATE OR ALTER PROCEDURE GetSongsByArtistSP
	@ArtistID int
AS
	SET NOCOUNT ON
	SELECT * FROM Songs WHERE ArtistID=@ArtistID
GO

CREATE OR ALTER PROCEDURE GetSongByIDSP
	@SongID int
AS
	SET NOCOUNT ON
	SELECT * FROM Songs WHERE SongID=@SongID
GO
--------------------Playlist--------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE CreatePlaylistSP
	@UserID int,
	@PlaylistName nvarchar(50),
	@PlaylistImage nvarchar(255),
	@PlaylistColor nvarchar(50),
	@MadePrivate bit
AS
	INSERT INTO Playlist (UserID, PlaylistName, PlaylistImage, PlaylistColor, MadePrivate)
	VALUES (@UserID, @PlaylistName, @PlaylistImage, @PlaylistColor, @MadePrivate) SELECT SCOPE_IDENTITY() AS PlaylistID
GO

CREATE OR ALTER PROCEDURE EditPlaylistSP
	@PlaylistID int,
	@PlaylistName nvarchar(50),
	@PlaylistImage nvarchar(255),
	@PlaylistColor nvarchar(50),
	@MadePrivate bit
AS
	UPDATE Playlist
	SET PlaylistName=@PlaylistName, PlaylistImage=@PlaylistImage, PlaylistColor=@PlaylistColor, MadePrivate=@MadePrivate
	WHERE PlaylistID=@PlaylistID
GO

CREATE OR ALTER PROCEDURE DeletePlaylistByIDSP
	@PlaylistID int
AS
	DELETE Playlist WHERE PlaylistID=@PlaylistID
GO

CREATE OR ALTER PROCEDURE GetAllUserPlaylists
	@UserID int
AS
	SET NOCOUNT ON
	SELECT * FROM Playlist WHERE UserID = @UserID
GO

CREATE OR ALTER PROCEDURE GetPlaylistByIDSP
	@PlaylistID int
AS
	SELECT * FROM Playlist WHERE PlaylistID=@PlaylistID
GO
------------------------------------------------------
CREATE OR ALTER PROCEDURE AddSongToPlaylistSP
	@SongID int,
	@PlaylistID int
AS
	INSERT INTO PlaylistSongs (SongID, PlaylistID)
	VALUES (@SongID, @PlaylistID)
GO

CREATE OR ALTER PROCEDURE RemoveSongFromPlaylistSP
	@PlaylistID int,
	@PlaylistSongID int
AS
	DELETE PlaylistSongs WHERE PlaylistID=@PlaylistID and PlaylistSongID = @PlaylistSongID
GO

CREATE OR ALTER PROCEDURE GetPlaylistSongsSP
	@PlaylistID int
AS
	SELECT *
	FROM PlaylistSongs
	INNER JOIN Songs ON PlaylistSongs.SongID = Songs.SongID
GO

--------------------Genre-----------------------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE CreateGenreSP
	@GenreName nvarchar(50)
AS
	SET NOCOUNT ON
	INSERT INTO Genre(GenreName)
	VALUES (@GenreName) SELECT SCOPE_IDENTITY() AS GenreID
GO

CREATE OR ALTER PROCEDURE GetAllGenresSP
AS
	SET NOCOUNT ON
	SELECT * FROM Genre
GO

CREATE OR ALTER PROCEDURE GetGenreByIDSP
	@GenreID int
AS
	SET NOCOUNT ON
	SELECT * FROM Genre WHERE GenreID=@Genreid
GO
--------------------Subcriptions----------------------------------------------------------------------------------------------------------------------------------------------------
--------------------?Customer Support?------------------------------------------------------------------------------------------------------------------------------------------------
