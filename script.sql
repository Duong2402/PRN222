USE [master]
GO
/****** Object:  Database [OnlineMusicDB]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE DATABASE [OnlineMusicDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OnlineMusicDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\OnlineMusicDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OnlineMusicDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\OnlineMusicDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [OnlineMusicDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OnlineMusicDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OnlineMusicDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OnlineMusicDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OnlineMusicDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [OnlineMusicDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OnlineMusicDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [OnlineMusicDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET RECOVERY FULL 
GO
ALTER DATABASE [OnlineMusicDB] SET  MULTI_USER 
GO
ALTER DATABASE [OnlineMusicDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OnlineMusicDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OnlineMusicDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OnlineMusicDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OnlineMusicDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OnlineMusicDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'OnlineMusicDB', N'ON'
GO
ALTER DATABASE [OnlineMusicDB] SET QUERY_STORE = OFF
GO
USE [OnlineMusicDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Albums]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Albums](
	[AlbumId] [uniqueidentifier] NOT NULL,
	[Album_Image] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ArtistId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Albums] PRIMARY KEY CLUSTERED 
(
	[AlbumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlbumSongs]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlbumSongs](
	[AlbumId] [uniqueidentifier] NOT NULL,
	[SongId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AlbumSongs] PRIMARY KEY CLUSTERED 
(
	[AlbumId] ASC,
	[SongId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Artists]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artists](
	[ArtistId] [uniqueidentifier] NOT NULL,
	[ArtistImage] [nvarchar](255) NOT NULL,
	[ArtistName] [nvarchar](255) NOT NULL,
	[Bio] [nvarchar](max) NULL,
 CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED 
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[Avatar] [nvarchar](max) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Histories]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Histories](
	[HistoryId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[SongId] [uniqueidentifier] NULL,
	[AlbumId] [uniqueidentifier] NULL,
	[PlayedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Histories] PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Playlists]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Playlists](
	[PlaylistId] [uniqueidentifier] NOT NULL,
	[PlaylistName] [nvarchar](255) NOT NULL,
	[PlaylistImage] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[IsPublic] [bit] NOT NULL,
 CONSTRAINT [PK_Playlists] PRIMARY KEY CLUSTERED 
(
	[PlaylistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlaylistSongs]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaylistSongs](
	[PlaylistId] [uniqueidentifier] NOT NULL,
	[SongId] [uniqueidentifier] NOT NULL,
	[AddAt] [datetime2](7) NULL,
	[PlayCount] [int] NULL,
 CONSTRAINT [PK_PlaylistSongs] PRIMARY KEY CLUSTERED 
(
	[PlaylistId] ASC,
	[SongId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[JwtId] [nvarchar](max) NOT NULL,
	[IsRevoked] [bit] NOT NULL,
	[AddedDate] [datetime2](7) NOT NULL,
	[ExpiryDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongGenres]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongGenres](
	[GenreId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_SongGenres] PRIMARY KEY CLUSTERED 
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Songs]    Script Date: 3/29/2025 10:20:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Songs](
	[SongId] [uniqueidentifier] NOT NULL,
	[ImageSong] [nvarchar](255) NULL,
	[NameSong] [nvarchar](255) NOT NULL,
	[FilePath] [nvarchar](255) NOT NULL,
	[Lyrics] [nvarchar](max) NULL,
	[NumberOfListeners] [int] NOT NULL,
	[GenreId] [uniqueidentifier] NOT NULL,
	[ArtistId] [uniqueidentifier] NOT NULL,
	[IsPublic] [bit] NULL,
	[UserId] [nvarchar](450) NULL,
 CONSTRAINT [PK_Songs] PRIMARY KEY CLUSTERED 
(
	[SongId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250223061626_Init', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250306092226_AddNewModels', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250306094535_FixForeignKeyNaming', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309090138_InitialCreate', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309104349_AddArtistSongRelationship', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309105050_AddRelationship', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309105655_AddLyricsToSongs', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250309110550_AddPlaylistSongRelationships', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250311140259_AddNumberOfListenersToSongs', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250311143027_SetDefaultNumberOfListeners', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250311150130_SetDefaultNumberOfListeners', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250312084630_AvatarToUsers', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250312144704_ImageToPlaylist', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250315033638_InitialCreate', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250326140316_InitialCreate', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250327034616_AddNewTable', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250327042047_InitialCreate', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250327044234_fixValidation', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250327153505_AddBoolToHistories', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250328003347_FixHistories', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250328154454_FixTableHistories', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250328190114_AddRelationshipUserAndSongs', N'8.0.13')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250329021223_FixTableSongs', N'8.0.13')
GO
INSERT [dbo].[Albums] ([AlbumId], [Album_Image], [Title], [CreatedAt], [ArtistId], [UserId]) VALUES (N'd00fd68b-224c-4eba-8fd3-0303163fa567', N'/img/album-img/album2.jpg', N'Khoảng lặng', CAST(N'2025-03-27T23:22:00.3100000' AS DateTime2), N'5b66202f-3dec-43cb-97be-c9dd819f8264', NULL)
INSERT [dbo].[Albums] ([AlbumId], [Album_Image], [Title], [CreatedAt], [ArtistId], [UserId]) VALUES (N'c6faf79e-9954-4c93-8c79-41484115272f', N'/img/album-img/album5.jpg', N'Gió và Sóng', CAST(N'2025-03-27T23:22:00.3100000' AS DateTime2), N'038a5239-9d88-45b5-967a-e0fac36b5b22', NULL)
INSERT [dbo].[Albums] ([AlbumId], [Album_Image], [Title], [CreatedAt], [ArtistId], [UserId]) VALUES (N'dbe9a90c-2b5e-471e-bebd-8d98d902ca9d', N'/img/album-img/album4.jpg', N'Albums 22', CAST(N'2025-03-27T23:22:00.3100000' AS DateTime2), N'c0c1ec1d-9946-419a-abc0-168ac88bc3f2', NULL)
INSERT [dbo].[Albums] ([AlbumId], [Album_Image], [Title], [CreatedAt], [ArtistId], [UserId]) VALUES (N'b4a2a68d-36dc-48ad-b4cc-ba8b9485f247', N'/img/album-img/album1.jpg', N'Dancing the moon', CAST(N'2025-03-27T23:22:00.3100000' AS DateTime2), N'b9cc2b4a-ab5e-460a-9b05-a91516319608', NULL)
INSERT [dbo].[Albums] ([AlbumId], [Album_Image], [Title], [CreatedAt], [ArtistId], [UserId]) VALUES (N'72cf04d0-dc56-44e9-9b41-c454e5c22f26', N'/img/album-img/album3.jpg', N'Hồi ức', CAST(N'2025-03-27T23:22:00.3100000' AS DateTime2), N'a5090dc5-a49f-48b2-aa2e-4427abe2edb5', NULL)
GO
INSERT [dbo].[AlbumSongs] ([AlbumId], [SongId]) VALUES (N'72cf04d0-dc56-44e9-9b41-c454e5c22f26', N'570126fd-3b06-4e6a-bc5e-277d51b38c64')
INSERT [dbo].[AlbumSongs] ([AlbumId], [SongId]) VALUES (N'd00fd68b-224c-4eba-8fd3-0303163fa567', N'56971968-406f-42d4-a9e0-4b7914e1d08b')
INSERT [dbo].[AlbumSongs] ([AlbumId], [SongId]) VALUES (N'c6faf79e-9954-4c93-8c79-41484115272f', N'0be0ef25-77ed-4686-8a37-596c7576db6c')
INSERT [dbo].[AlbumSongs] ([AlbumId], [SongId]) VALUES (N'dbe9a90c-2b5e-471e-bebd-8d98d902ca9d', N'7e5480c5-d24c-471b-910d-75b9aaefbb71')
INSERT [dbo].[AlbumSongs] ([AlbumId], [SongId]) VALUES (N'b4a2a68d-36dc-48ad-b4cc-ba8b9485f247', N'2d305829-28ab-4f67-84f1-986bfc6701de')
INSERT [dbo].[AlbumSongs] ([AlbumId], [SongId]) VALUES (N'b4a2a68d-36dc-48ad-b4cc-ba8b9485f247', N'f708374f-d654-4254-843f-9b3b8ad2e642')
GO
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'c0c1ec1d-9946-419a-abc0-168ac88bc3f2', N'/img/artist-image/Artist2.jpg', N'Kai Đinh', N'Đinh Lê Hoàng Vỹ, thường được biết đến với nghệ danh Kai Đinh (sinh ngày 17 tháng 10 năm 1992), là một nam nhạc sĩ, nhà sản xuất thu âm kiêm ca sĩ người Việt Nam.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'a5090dc5-a49f-48b2-aa2e-4427abe2edb5', N'/img/artist-image/Artist7.jpg', N'Đen Vâu', N'Nguyễn Đức Cường, thường được biết đến với nghệ danh Đen hay Đen Vâu, là một nam rapper, nhạc sĩ và người dẫn chương trình người Việt Nam. Đen Vâu là "một trong số ít nghệ sĩ thành công từ làn sóng underground và âm nhạc indie" của Việt Nam. Anh sở hữu những bài hát đạt hàng triệu lượt xem trên YouTube và là chủ nhân của một số giải thưởng âm nhạc.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'ef35e706-d6a3-48d7-b4fe-499d9c6581d4', N'/img/artist-image/Artist.jpg', N'Hari Won', N'Luu Esther, thu?ng du?c bi?t d?n v?i ngh? danh Hari Won, là m?t n? ca si, di?n viên kiêm ngu?i d?n chuong trình mang hai dòng máu Vi?t Nam và Hàn Qu?c. Cô chính th?c ho?t d?ng ngh? thu?t ? Vi?t Nam k? t? nam 2014.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'597e96ea-f571-4878-bd83-5e44d7b324b2', N'/img/artist-image/Artist6.jpg', N'Bảo Anh', N'Nguyễn Hoài Bảo Anh (sinh ngày 3 tháng 9 năm 1992), thường được biết đến với nghệ danh Bảo Anh, là một nữ ca sĩ kiêm diễn viên người Việt Nam.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'2a580bef-82a9-497a-9165-807f795f4f59', N'/img/artist-image/Artist4.jpg', N'Phan Mạnh Quỳnh', N'Phan Mạnh Quỳnh (sinh ngày 10 tháng 1 năm 1990) là một nam ca sĩ kiêm sáng tác nhạc người Việt Nam. Anh giành được một đề cử Cống hiến cho bài -Huyền thoại- năm 2019.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'b9cc2b4a-ab5e-460a-9b05-a91516319608', N'/img/artist-image/Artist1.jpg', N'Soobin Hoàng Sơn', N'Nguyễn Huỳnh Sơn (sinh ngày 10 tháng 9 năm 1992), thường được biết đến với nghệ danh Soobin hay với tên cũ Soobin Hoàng Sơn (viết cách điệu là SOOBIN ), là một nam ca sĩ kiêm nhạc sĩ sáng tác ca khúc người Việt Nam.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'9187781a-b7d4-4abd-8305-ad6086aa370a', N'/img/artist-image/Artist10.jpg', N'Sơn Tùng MTP', N'Nguyễn Thanh Tùng (sinh ngày 5 tháng 7 năm 1994), thường được biết đến với nghệ danh Sơn Tùng M-TP, là một nam ca sĩ kiêm nhạc sĩ sáng tác bài hát, nhà sản xuất thu âm, rapper và diễn viên người Việt Nam .')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'702399a7-7aca-4a90-8b81-afc87ea6dd1e', N'/img/artist-image/Artist5.jpg', N'Hoàng Tôn', N'Hoàng Tôn sinh ra trong gia đình nghệ thuật, mẹ là giảng viên thanh nhạc còn bố là nhạc công guitar. Năm 2013, anh tham gia The Voice, giành giải Á Quân.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'b0d62183-6d34-4ea4-bbc9-b94c3ff09541', N'/img/artist-image/Artist11.jpg', N'Dương Domic', N'Trần Đăng Dương (sinh ngày 31 tháng 8 năm 2000 ), thường được biết đến với nghệ danh Dương Domic, là một nam ca sĩ kiêm sáng tác nhạc, rapper người Việt Nam.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'aa0619db-a2b1-4de3-b182-be2968797711', N'/img/artist-image/Artist9.jpg', N'Min', N'Nguyễn Minh Hằng (sinh ngày 7 tháng 12 năm 1988 ), thường được biết đến với nghệ danh Min (cách điệu là MIN ), là một nữ ca sĩ kiêm vũ công người Việt Nam .')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'5b66202f-3dec-43cb-97be-c9dd819f8264', N'/img/artist-image/Artist3.jpg', N'Bùi Lan Hương', N'Bùi Lan Hương (sinh ngày 11 tháng 6 năm 1989), là một nữ ca sĩ kiêm sáng tác nhạc người Việt Nam. Cô từng giành được một giải Cống hiến ở hạng mục -Nghệ sĩ mới của năm- vào năm 2019.')
INSERT [dbo].[Artists] ([ArtistId], [ArtistImage], [ArtistName], [Bio]) VALUES (N'038a5239-9d88-45b5-967a-e0fac36b5b22', N'/img/artist-image/Artist8.jpg', N'MCK', N'Nghiêm Vũ Hoàng Long (sinh ngày 2 tháng 3 năm 1999), thường được biết đến với nghệ danh MCK, là một nam rapper và ca sĩ kiêm sáng tác nhạc người Việt Nam .')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'470d5a85-979a-4256-9a6b-58c941a037f7', N'User', N'USER', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'a019da0e-e323-4b6a-b428-aefcfd39166a', N'Admin', N'ADMIN', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'470d5a85-979a-4256-9a6b-58c941a037f7')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'833131f0-9339-43f0-b53e-155a86d8569e', N'a019da0e-e323-4b6a-b428-aefcfd39166a')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FullName], [Avatar], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'833131f0-9339-43f0-b53e-155a86d8569e', N'Adminne', NULL, N'admin@gmail.com', N'ADMIN@GMAIL.COM', N'admin@gmail.com', N'ADMIN@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEIPDGLnnnkkfNtqmkkvT93ekvDUYEYUW2PvbxY8CxTbFFfg1+EAZ6M6D9obx8CZdMg==', N'HQRH57KSXTNLAUGL7TKULANJI4MXLH7S', N'73dbfbb3-8b33-4923-891e-a4bbfcb4c31c', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FullName], [Avatar], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'9a442f6c-0670-4be6-88ae-82e770586898', N'boipho', NULL, N'huypn6@gmail.com', N'HUYPN6@GMAIL.COM', N'huypn6@gmail.com', N'HUYPN6@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEMRuOO2Cp67FE87xmoaUgauWOpq8yEOK9bpaVIGOigVN5OOdR2KseQkRQ+TVoE+2+g==', N'64C3OWO2AFOZH72J4JSFUZRFMGVOLYJO', N'c5772ccd-44cf-4933-a870-8e5687747681', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FullName], [Avatar], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'boipho', NULL, N'ducbinhnd01@gmail.com', N'DUCBINHND01@GMAIL.COM', N'ducbinhnd01@gmail.com', N'DUCBINHND01@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEC8RcFw9vdRW0W02gcpwYbJWqQq3/IFzTu17po1sp4g9kOMcTaUMRmKpr9lT5zi6xQ==', N'AVNP7ZQ7KBWYI6FT7LFO2O7MEWPW65T7', N'c3845a0c-b5b5-4a36-b55e-af346a150c09', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Histories] ([HistoryId], [UserId], [SongId], [AlbumId], [PlayedAt]) VALUES (N'4e3094f5-e5af-4c74-9534-168553acd767', N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'15825e4b-587e-4ed1-b3f6-4a31290baaca', NULL, CAST(N'2025-03-28T12:01:39.0530408' AS DateTime2))
INSERT [dbo].[Histories] ([HistoryId], [UserId], [SongId], [AlbumId], [PlayedAt]) VALUES (N'95375cdd-5725-46a6-99a4-4f48b4a6274e', N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'0db4fdda-15b5-48ee-8062-3d805a79421d', NULL, CAST(N'2025-03-28T12:02:59.5977324' AS DateTime2))
INSERT [dbo].[Histories] ([HistoryId], [UserId], [SongId], [AlbumId], [PlayedAt]) VALUES (N'752eaa3b-0f6f-41df-b8fa-be8fe887cc11', N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'03c56437-f04f-4676-b0c1-7c696bff9cb9', NULL, CAST(N'2025-03-28T11:57:45.8060477' AS DateTime2))
INSERT [dbo].[Histories] ([HistoryId], [UserId], [SongId], [AlbumId], [PlayedAt]) VALUES (N'6cf083ca-0ce0-490c-a1e8-ed74e13c07dc', N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', NULL, N'b4a2a68d-36dc-48ad-b4cc-ba8b9485f247', CAST(N'2025-03-28T22:47:09.4115350' AS DateTime2))
GO
INSERT [dbo].[Playlists] ([PlaylistId], [PlaylistName], [PlaylistImage], [UserId], [CreatedAt], [IsPublic]) VALUES (N'541daf4d-2ba8-4332-ab0b-87788c3202d0', N'Dancing the moon', N'/img/album-img/album1.jpg', N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', CAST(N'2025-03-28T21:25:29.7589684' AS DateTime2), 0)
INSERT [dbo].[Playlists] ([PlaylistId], [PlaylistName], [PlaylistImage], [UserId], [CreatedAt], [IsPublic]) VALUES (N'bd6fce7b-a9eb-4b80-aa24-ebcad788a693', N'One', N'/img/playlist-img/133845819849470401.jpg', N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', CAST(N'2025-03-28T11:57:59.5984284' AS DateTime2), 0)
GO
INSERT [dbo].[PlaylistSongs] ([PlaylistId], [SongId], [AddAt], [PlayCount]) VALUES (N'bd6fce7b-a9eb-4b80-aa24-ebcad788a693', N'2d305829-28ab-4f67-84f1-986bfc6701de', CAST(N'2025-03-28T09:38:56.0503807' AS DateTime2), NULL)
INSERT [dbo].[PlaylistSongs] ([PlaylistId], [SongId], [AddAt], [PlayCount]) VALUES (N'bd6fce7b-a9eb-4b80-aa24-ebcad788a693', N'f708374f-d654-4254-843f-9b3b8ad2e642', CAST(N'2025-03-28T10:59:00.8605177' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[RefreshTokens] ON 

INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [JwtId], [IsRevoked], [AddedDate], [ExpiryDate]) VALUES (2, N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'8fb8ae83-359b-4fe0-94e9-46e71d221c0f-e6c7830b-858f-47bf-abef-71f3df259bd0', N'4e4e4ccc-9b5d-4d8b-b5da-895a0bb750eb', 0, CAST(N'2025-03-26T21:17:00.7057364' AS DateTime2), CAST(N'2025-03-26T21:19:00.7057366' AS DateTime2))
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [JwtId], [IsRevoked], [AddedDate], [ExpiryDate]) VALUES (3, N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'c6596b3a-b46c-4b2d-a3b3-cae31f429b56-56364988-8765-47e3-8e29-79b1c4f7106f', N'9ee6f639-883a-4aaf-8edc-87958945999f', 0, CAST(N'2025-03-26T21:17:46.6056298' AS DateTime2), CAST(N'2025-03-26T21:19:46.6056305' AS DateTime2))
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [JwtId], [IsRevoked], [AddedDate], [ExpiryDate]) VALUES (6, N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'3b479a6d-2c92-4024-b16f-98be550c26a3-5276c65e-9b71-4d3e-9a62-06a1c72d3e5b', N'54fbda07-8fa9-409d-8c1d-dd0cdc13467c', 0, CAST(N'2025-03-27T11:49:35.0238884' AS DateTime2), CAST(N'2025-03-27T11:51:35.0239323' AS DateTime2))
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [JwtId], [IsRevoked], [AddedDate], [ExpiryDate]) VALUES (17, N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'b02e4bec-dca0-4063-9723-f15ce9ce1c87-ac4775f8-735e-4b3b-8045-6acbb5c5ead0', N'301173d5-63ac-4d4f-9e1b-e191f3912df3', 0, CAST(N'2025-03-28T12:11:19.4801554' AS DateTime2), CAST(N'2025-03-28T12:13:19.4801749' AS DateTime2))
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [JwtId], [IsRevoked], [AddedDate], [ExpiryDate]) VALUES (1013, N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'd56a2d9a-e1b5-45c2-a5b1-17f9a6267d98-37828bfa-b601-4422-b7c3-1c37796e9106', N'abc12672-f3db-4004-97ab-20e4d8dadccf', 0, CAST(N'2025-03-28T23:27:36.7068494' AS DateTime2), CAST(N'2025-03-28T23:29:36.7068898' AS DateTime2))
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [JwtId], [IsRevoked], [AddedDate], [ExpiryDate]) VALUES (2007, N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c', N'40caa2cc-b39c-420b-88bb-711557330734-8b79b4e9-8f76-45ab-8501-a6cc74d2d317', N'a05886eb-9f40-4aed-910f-62e008dade3a', 0, CAST(N'2025-03-29T04:27:38.1689828' AS DateTime2), CAST(N'2025-03-29T04:29:38.1690415' AS DateTime2))
SET IDENTITY_INSERT [dbo].[RefreshTokens] OFF
GO
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'Pop', NULL)
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'440082fe-50ba-452a-9678-1c90ff065f09', N'Tiền chiến', NULL)
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'c946b68e-5952-4366-9dcd-30e961d435f2', N'Cổ điển', NULL)
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'fd2b3c2b-67e1-41d9-9094-608c1d40dab9', N'Ballad', NULL)
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'2f883c78-655f-4850-afbf-6260d61e1b69', N'Hip-hop / Rap', NULL)
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'fd03bf9b-a9ce-4ac8-9dd2-77ddc3320c39', N'Rock', NULL)
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'e771c896-277b-4f04-8733-7c8fed8a53e3', N'Nhạc Trẻ', NULL)
INSERT [dbo].[SongGenres] ([GenreId], [Name], [Description]) VALUES (N'2d1fd6d7-3b5f-4313-9c83-f105e1a4e6bb', N'Cách mạng', NULL)
GO
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'570126fd-3b06-4e6a-bc5e-277d51b38c64', N'/img/song-image/song10.jpg', N'Một Triệu Like', N'/audio/Một Triệu Like.mp3', N'', 7, N'2f883c78-655f-4850-afbf-6260d61e1b69', N'a5090dc5-a49f-48b2-aa2e-4427abe2edb5', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'0db4fdda-15b5-48ee-8062-3d805a79421d', N'/img/song-image/song11.jpg', N'Mất Kết Nối', N'/audio/Mất Kết Nối.mp3', N'', 31, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'b0d62183-6d34-4ea4-bbc9-b94c3ff09541', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'15825e4b-587e-4ed1-b3f6-4a31290baaca', N'/img/song-image/song8.jpg', N'Yêu một người vô tâm', N'/audio/Yeu-Mot-Nguoi-Vo-Tam-Bao-Anh.mp3', N'Σạnh bên một người vô tâm.
Là nước mắt tuôn âm thầm.
Là уêu quá lâu một người đã chẳng còn chút động lòng.
Đợi anh mới biết đêm dài.
Tin anh chẳng chút nghi ngại.
Vì уêu nên em trao hết tương lai.
Tại sao không giống như xưa.
Đến lúc nước mắt thaу mưa.
Ɲhưng hối hận đã không kịp nữa.
[Điệp khúc]
Ɲhìn lại người con gái anh từng rất nâng niu.
Ɗù ngàу đêm vẫn không ngại gió mưa.
Quấn quýt thuở ban đầu anh giờ đâu nhớ.
Đánh đổi tất cả bình уên em đã có.
Để rồi đau thấu đến nghẹn lời....
Ɲhớ một thời ai hứa không xa rời.
Tưởng rằng mình maу mắn khi được ở bên anh.
Σhuуện tình ta ngỡ tồn tại suốt đời.
Ɲgỡ sẽ уêu lâu dài ai ngờ không phải.
Ɛm từng cố dỗ dành con tim уếu đuối.
Để rồi đau đớn thấu Trời.
Phút xa rời ai cũng cạn lời.
Ɲgoài ô cửa là mưa rơi...
Ɛm phải trách mình ngâу thơ...
Tại sao cứ đợi anh quan tâm nhiều hơn...
Ɛm chỉ đứng đằng sau nhiều điều.
trong cuộc sống của anh bâу giờ.
Σhỉ vì.
Vì không thể giữ dâу tơ... tình mình.
Tình уêu mù quáng là khi уêu lầm.
Một người vương vấn còn уêu đơn phương một người nhẫn tâm há ha há ha...
Ɲgoài em anh có уêu ai.
Quan tâm chăm sóc cho ai.
Mà sao anh vô tâm với em hoài...
Rồi bỗng sét đánh bên tai.
Ɲgười nói "Tất cả đã phai.
Σhẳng có gì tốt trên đời mãi".
[Điệp khúc]
Ɲhìn lại người con gái anh từng rất nâng niu.
Ɗù ngàу đêm vẫn không ngại gió mưa.
Quấn quýt thuở ban đầu anh giờ đâu nhớ.
Đánh đổi tất cả bình уên em đã có.
Để rồi đau thấu đến nghẹn lời...
Ɲhớ một thời ai hứa không xa rời.
Tưởng rằng mình maу mắn khi được ở bên anh.
Σhuуện tình ta ngỡ tồn tại suốt đời.
Ɲgỡ sẽ уêu lâu dài ai ngờ không phải.
Ɛm từng cố dỗ dành con tim уếu đuối.
Để rồi đau đớn thấu Trời.
Phút xa rời ai cũng cạn lời.
Thật lòng em đã nghĩ... anh từng rất уêu em.
Thật lòng уêu đến cuối cuộc đời...
Σhẳng để tâm bão giông... lúc nào sẽ tới.
Σhỉ là chút rung động phải không anh ơi?
Σhỉ là giâу phút nhất thời?.
Trách duуên trời đã khiến ta xa rời...
Σhỉ là chút rung động mà sao em ơi.
Σhỉ là... giâу phút nhất thời.
Vỡ duуên trời, anh mất em... một đời.', 235, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'597e96ea-f571-4878-bd83-5e44d7b324b2', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'56971968-406f-42d4-a9e0-4b7914e1d08b', N'/img/song-image/song4.jpg', N'Ngày chưa giống bão', N'/audio/Ngay-Chua-Giong-Bao-Bui-Lan-Huong.mp3', N'Vì ta уêu nhau như cơn sóng vỗ.
	Quẩn quanh bao năm không buông mặt hồ.
	Thuуền anh đi xa bờ thì em vẫn dõi chờ.
	Ɗuуên mình dịu êm thơ rất thơ.
	Và anh nâng niu em như đoá hoa.
    Ϲòn em xem anh như trăng ngọc ngà.
    Tự do như mâу vàng mình phiêu du non ngàn.
    Ɗẫu trần gian bao la đến đâu nơi anh là nhà... à a á... à a á a à...
    [Điệp khúc].
	Khi anh qua thung lũng.
	Và bóng đêm ghì bàn chân.
	Đời khiến anh chẳng còn nhiều luуến lưu.
	Ɛm mong lau mắt anh khô.
	Ta уêu sai haу đúng?.
	Ϲòn thấу đau là còn thương.
	Khi bão qua rồi biết đâu sẽ đi tới nơi của ngàу đầu.
	Hết muộn sầu.
	hà ha ha... hà há ha hà...
	Lạc bước giữa những đam mê tăm tối.
	Liệu máu vẫn nóng nơi tim bồi hồi?.
	Ɲgười năm xưa đâu rồi, lạnh băng tiếng khóc cười.
	Anh ở nơi xa xôi vô lối.
	Mặt đất níu giữ đôi chân chúng ta.
	Thì baу lên trong cơn mơ kỳ lạ.
	Ở đó anh vẫn là người уêu thương chan hoà.
	Ɗẫu trần gian cho anh đắng caу nơi em là nhà... à ha há ha hà...
	Hừ hư hứ hứ hư hừ...', 13, N'e771c896-277b-4f04-8733-7c8fed8a53e3', N'5b66202f-3dec-43cb-97be-c9dd819f8264', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'0be0ef25-77ed-4686-8a37-596c7576db6c', N'/img/song-image/song13.jpg', N'Chìm Sâu', N'/audio/Chìm Sâu.mp3', N'', 0, N'2f883c78-655f-4850-afbf-6260d61e1b69', N'038a5239-9d88-45b5-967a-e0fac36b5b22', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'e54ffcf5-1953-4e47-9d7c-66a175c3b54a', N'/img/song-image/song9.jpg', N'Hãy trao cho anh', N'/audio/HayTraoChoAnh-SonTungMTPSnoopDogg-6010660.mp3', N'', 27, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'9187781a-b7d4-4abd-8305-ad6086aa370a', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'7e5480c5-d24c-471b-910d-75b9aaefbb71', N'/img/song-image/song3.jpg', N'Điều buồn nhất', N'/audio/Dieu-Buon-Nhat-Kai-Dinh.mp3', N'Thời gian trôi.
	Từng sáng tối.
    Sao quá vội.
	Vẫn luôn là mình em thôi.
	Nhìn theo anh.
    Tim tan vỡ.
    Gần bên anh.
    Mãi là một giấc mơ.
    Biết yêu riêng ai là rất buồn.
    Biết yêu đơn phương là sẽ luôn.
	Còn mãi trong lòng.
    Những tổn thương.
	Sợ anh biết lại sợ anh không biết.
	Muốn anh biết lại muốn anh không biết.
	Điều buồn nhất là.
	Là anh biết lại làm như không biết.
	Sợ em sẽ khóc lại vờ như không khóc.
	Lúc muốn khóc lại giữ trong lòng.
	Điều buồn nhất là.
	Là anh biết lại làm như không biết Em yêu anh.', 3, N'fd2b3c2b-67e1-41d9-9094-608c1d40dab9', N'c0c1ec1d-9946-419a-abc0-168ac88bc3f2', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'03c56437-f04f-4676-b0c1-7c696bff9cb9', N'/img/song-image/song12.jpg', N'Vì Yêu Cứ Đâm Đầu', N'/audio/Vì Yêu Cứ Đâm Đầu.mp3', N'', 33, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'aa0619db-a2b1-4de3-b182-be2968797711', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'3c989441-2509-4997-9a5c-7f1bec2b31a2', N'/img/artist-image/Artist10.jpg', N'Em của ngày hôm qua', N'/audio/ EmCuaNgayHomQuaSkyTour2019-SonTungMTP-6301041.mp3', NULL, 0, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'9187781a-b7d4-4abd-8305-ad6086aa370a', 0, N'9b1d2f24-f1ad-4531-8d1f-bc97d2bf409c')
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'b30794a1-623d-4c89-8e17-8b0d00304827', N'/img/song-image/song1.jpg', N'Anh cứ đi đi', N'/audio/Anh-Cu-Di-Di-Hari-Won.mp3', N'Tình yêu là thế...Đôi khi làm mình say mê, đôi khi làm mình ngô nghê.
	Tin một người đến nỗi rơi lệ.Là thế...Khi yêu ai chẳng cần biết nữa, khi yêu ai thì dù trong mưa.
	Vẫn cảm thấy ấm áp dư thừa...Rồi khi em thấy.
	Anh trong tay cùng người khác ấy, sao em quên được khoảnh khắc đấy.
	Anh bên ai hạnh phúc như vậy?.
	Thì thôi....Buông đôi tay và để anh đi. 
	Xem như ta lần đầu chia ly.
	Cũng là lần cuối nghĩ suy.. .
	Thì anh.. cứ đi đi hãy cứ xa em và đừng ngẫm nghĩ.
	Hạnh phúc ra sao yêu thương nhường nào chỉ thêm thời gian lãng phí.
	Ừ thì anh cứ đi đi và đừng nhớ nhung chi.
	Về đâu khi ta đã lạc mất nhau??.
	Mình buồn vì tim mình đau.
	Mình buồn thì ai thấu đâu?.
	Từng lời buông chưa hết câu, nước mắt đã dâng khóe sầu.
	Đừng bên nhau nếu không vui, em muốn thấy anh cười.
	Vì yêu nên em xin anh cứ đi...Bỏ mặc em!', 4, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'ef35e706-d6a3-48d7-b4fe-499d9c6581d4', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'2d305829-28ab-4f67-84f1-986bfc6701de', N'/img/song-image/song2.jpg', N'Anh đã quen với cô đơn', N'/audio/Anh-Da-Quen-Voi-Co-Don-Soobin-Hoang-Son.mp3', N'Anh thường xuyên nằm mơ.
	Về một ngôi nhà, tại một nơi chỉ có chúng ta.
    Nơi mà anh và em xây dựng từng câu chuyện.
	Chia sẻ những ước mơ.
	Trên bầu trời cao, với muôn vì sao, soi sáng mỗi con đường.
	Hòa cùng những đám mây và gió đưa đến nơi này.
	Em đã nói sẽ ở bên anh lâu dài.
	Những cảm xúc không thể nào quên được.
	Có thể anh chỉ là mơ.
	Chỉ là giấc mơ về một hạnh phúc.
	Nắng tàn trên đôi mi của ai đó.
	Giờ đây, em ở đâu? Có biết nơi này không?.
	Có lẽ em đã quên hết
	Từng cái ôm trong tiết trời đông lạnh lùng.
	Những ngón tay của chúng ta vẫn kề nhau chặt chẽ.
	Nhìn nhau lâu và không cần nói điều gì.
	[Điệp khúc].
	Mỗi đêm lẻ loi.
	Đừng để giọt nước mắt lăn dài tàn hoa khắp nơi, hãy giữ lại em babe.
	Mỗi đêm cô đơn.
	Lại nhắc tên những kí ức đầy nỗi nhớ.
	Những đam mê đã nhanh chóng phai mờ.
Yêu em không biết đúng sai.
Chỉ bằng cảm xúc, bằng lý trí, bằng tất cả những gì anh có.
Hay em đưa ra.
Khi hai trái tim chệch nhịp.
Dù anh cố gắng như thế nào, em vẫn không hiểu được anh.
Như là chính em.', 3, N'fd2b3c2b-67e1-41d9-9094-608c1d40dab9', N'b9cc2b4a-ab5e-460a-9b05-a91516319608', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'f708374f-d654-4254-843f-9b3b8ad2e642', N'/img/song-image/song6.jpg', N'Xin đừng lặng im', N'/audio/Xin-Dung-Lang-Im-Soobin-Hoang-Son.mp3', N'Anh vẫn nhớ phút ấy khi anh rời xa.
	Người chẳng nói một lời.
	Chỉ đứng yên vậy thôi.
	Em đã cất giấu hết bao nhiêu buồn đau.
	Còn kí ức ngọt ngào.
	Anh mang theo dù ở đâu.
	Làm gì để trở về?.
    [Điệp khúc].
	Người đừng lặng im đến thế.
	Vì lặng im sẽ giết chết con tim.
	Dù yêu thương chẳng còn.
	Anh vẫn xin em nói một lời.
	Ngoài kia bao la thế giới.
	Nhưng trong anh thế giới chỉ là em thôi.
	Mình xa nhau thật rồi.
	Nhưng anh vẫn chờ đợi.
	Đã có lúc cố gắng để hiểu được em.
	Rồi cứ thế mỏi mệt.
	Chỉ biết bên cạnh em.
	Đây có lẽ chẳng phải yêu thương trọn vẹn.
	Người tìm nơi bắt đầu.
	Người bỏ đi ở phía sau.
	Với tất cả nỗi sầu.
	Anh chẳng biết được là.
	Khi tất cả đã qua.
	Em đã nghĩ điều gì?
	Ngày anh cất bước đi.
	Thà một lời rồi cùng buồn đau.
	Còn hơn riêng em cố giấu.
	Anh chẳng biết được là.
	Xa nhau hay bước tiếp?.
	Êm đềm hay muộn phiền?.
	Cho nhau hai lối riêng?.
	Một lần để mai sau chẳng phải hối tiếc.', 23, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'b9cc2b4a-ab5e-460a-9b05-a91516319608', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'52097ae8-b3e7-4b0a-895d-b941d06c4e8c', N'/img/song-image/song7.jpg', N'Yêu em rất nhiều', N'/audio/Yeu-Em-Rat-Nhieu-Hoang-Ton.mp3', N'Ngồi giữa bóng đêm anh chưa thể ngủ được.
Vì nỗi nhớ em vu vơ bất thường.
Cảm giác đắm say cứ thế đong đầy trong nơi trái tim.
Là thứ bấy lâu nay anh đã kiếm tìm.
Những câu chuyện thật dài mỗi tối.
Những phút giây ngập ngừng bối rối.
Tiếng em cười và lời em nói.
Khiến anh giờ đang như quên mất lối.
Những khi chạm nhìn vào đôi mắt.
Anh chỉ muốn ôm em thật chặt.
Để anh nói em nghe, nói em nghe lòng anh.
[ĐK:]
Người ơi em có biết anh đã yêu em rất nhiều.
Chẳng cần những lý lẽ để nói nên câu tình yêu.
Làm như không quan tâm nhưng anh thực sự nhớ em.
Muốn được chở che cho em những đêm lạnh về.
Anh muốn nói yêu em rất nhiều.
Những câu chuyện thật dài mỗi tối.
Những phút giây ngập ngừng bối rối.
Tiếng em cười và lời em nói.
Khiến anh giờ đang như quên mất lối.
Từ khi đã trót lỡ yêu rồi.
Anh chỉ mong gần em mãi thôi.
Để anh hôn nhẹ lên, hôn nhẹ lên bờ môi.
Và anh muốn hát cho em bài hát trong tâm hồn anh.
Thật mạnh mẽ nhưng sao nhiều lúc cô đơn mỏng manh.
Chuyện tình yêu chẳng thể nói trước điều gì.
Nhưng thôi ta mặc kệ cứ yêu đi.
Nguyện bên em trên con đường dài, thủy chung mãi mãi.', 0, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'702399a7-7aca-4a90-8b81-afc87ea6dd1e', 1, NULL)
INSERT [dbo].[Songs] ([SongId], [ImageSong], [NameSong], [FilePath], [Lyrics], [NumberOfListeners], [GenreId], [ArtistId], [IsPublic], [UserId]) VALUES (N'4881b6b4-f9c0-45db-80d5-cd8115f3d221', N'/img/song-image/song5.jpg', N'Vợ người ta', N'/audio/Vo-Nguoi-Ta-Phan-Manh-Quynh.mp3', N'Tấm thiệp mời trên bàn.
	Thời gian địa điểm rõ ràng.
	Lại một đám mừng ở trong làng.
	Ngó tên bỗng dưng thấy hoang mang.
	Rồi ngày cưới rộn ràng khắp vùng.
	Ai theo chân ai tới già trẻ đi cùng.
	Nhiều ngày tháng giờ này tương phùng.
	Mà lòng cay cay cay.
    [Điệp khúc].
	Giờ em đã là vợ người ta.
	Áo trắng cô dâu cầm hoa.
	Nhạc tung tóe thanh niên hòa ca.
	Vài ba đứa lên lắc lư theo.
	Ấy là thành đám cưới em với người ta.
	Anh biết do anh mà ra.
	Tình yêu ấy nay xa càng xa.
	Buồn thay la la lá.
	Nghĩ nhiều chuyện trong đời.
	Anh thấy lòng càng rối bời.
	Liệu ngày đó nhiệt tình ngỏ lời.
	Chúng ta lấy nhau chứ em ơi.
	Đành bảo phó mặc ở duyên trời.
	Nhưng thâm tâm anh trách nàng tại sao vội.
	Một người bước, một người không đợi.
	Thì đành tìm ai ai ai.
	Giờ em đã là vợ người ta.
	Hãy sống vui hơn ngày qua.
	Nhạc cũng tắt thanh niên rời bar.
	Còn năm sáu tên đứng lơ ngơ.
	Ấy là tàn lễ cưới em theo người ta.
	Anh bước đi như hồn ma.
	Ngày hôm ấy như kéo dài ra.
	Buồn thay la la lá.', 1, N'962e11e9-2fd6-40c8-8a37-0ffa9c6b82d6', N'2a580bef-82a9-497a-9165-807f795f4f59', 1, NULL)
GO
/****** Object:  Index [IX_Albums_ArtistId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Albums_ArtistId] ON [dbo].[Albums]
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Albums_UserId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Albums_UserId] ON [dbo].[Albums]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AlbumSongs_SongId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_AlbumSongs_SongId] ON [dbo].[AlbumSongs]
(
	[SongId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Histories_AlbumId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Histories_AlbumId] ON [dbo].[Histories]
(
	[AlbumId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Histories_SongId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Histories_SongId] ON [dbo].[Histories]
(
	[SongId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Histories_UserId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Histories_UserId] ON [dbo].[Histories]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Playlists_UserId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Playlists_UserId] ON [dbo].[Playlists]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PlaylistSongs_SongId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_PlaylistSongs_SongId] ON [dbo].[PlaylistSongs]
(
	[SongId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RefreshTokens_UserId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] ON [dbo].[RefreshTokens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Songs_ArtistId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Songs_ArtistId] ON [dbo].[Songs]
(
	[ArtistId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Songs_GenreId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Songs_GenreId] ON [dbo].[Songs]
(
	[GenreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Songs_UserId]    Script Date: 3/29/2025 10:20:45 AM ******/
CREATE NONCLUSTERED INDEX [IX_Songs_UserId] ON [dbo].[Songs]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Playlists] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsPublic]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_Artists_ArtistId] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artists] ([ArtistId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_Artists_ArtistId]
GO
ALTER TABLE [dbo].[Albums]  WITH CHECK ADD  CONSTRAINT [FK_Albums_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Albums] CHECK CONSTRAINT [FK_Albums_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AlbumSongs]  WITH CHECK ADD  CONSTRAINT [FK_AlbumSongs_Albums_AlbumId] FOREIGN KEY([AlbumId])
REFERENCES [dbo].[Albums] ([AlbumId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlbumSongs] CHECK CONSTRAINT [FK_AlbumSongs_Albums_AlbumId]
GO
ALTER TABLE [dbo].[AlbumSongs]  WITH CHECK ADD  CONSTRAINT [FK_AlbumSongs_Songs_SongId] FOREIGN KEY([SongId])
REFERENCES [dbo].[Songs] ([SongId])
GO
ALTER TABLE [dbo].[AlbumSongs] CHECK CONSTRAINT [FK_AlbumSongs_Songs_SongId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Histories]  WITH CHECK ADD  CONSTRAINT [FK_Histories_Albums_AlbumId] FOREIGN KEY([AlbumId])
REFERENCES [dbo].[Albums] ([AlbumId])
GO
ALTER TABLE [dbo].[Histories] CHECK CONSTRAINT [FK_Histories_Albums_AlbumId]
GO
ALTER TABLE [dbo].[Histories]  WITH CHECK ADD  CONSTRAINT [FK_Histories_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Histories] CHECK CONSTRAINT [FK_Histories_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Histories]  WITH CHECK ADD  CONSTRAINT [FK_Histories_Songs_SongId] FOREIGN KEY([SongId])
REFERENCES [dbo].[Songs] ([SongId])
GO
ALTER TABLE [dbo].[Histories] CHECK CONSTRAINT [FK_Histories_Songs_SongId]
GO
ALTER TABLE [dbo].[Playlists]  WITH CHECK ADD  CONSTRAINT [FK_Playlists_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Playlists] CHECK CONSTRAINT [FK_Playlists_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[PlaylistSongs]  WITH CHECK ADD  CONSTRAINT [FK_PlaylistSongs_Playlists_PlaylistId] FOREIGN KEY([PlaylistId])
REFERENCES [dbo].[Playlists] ([PlaylistId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlaylistSongs] CHECK CONSTRAINT [FK_PlaylistSongs_Playlists_PlaylistId]
GO
ALTER TABLE [dbo].[PlaylistSongs]  WITH CHECK ADD  CONSTRAINT [FK_PlaylistSongs_Songs_SongId] FOREIGN KEY([SongId])
REFERENCES [dbo].[Songs] ([SongId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlaylistSongs] CHECK CONSTRAINT [FK_PlaylistSongs_Songs_SongId]
GO
ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_Artists_ArtistId] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artists] ([ArtistId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK_Songs_Artists_ArtistId]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK_Songs_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK_Songs_SongGenres_GenreId] FOREIGN KEY([GenreId])
REFERENCES [dbo].[SongGenres] ([GenreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK_Songs_SongGenres_GenreId]
GO
USE [master]
GO
ALTER DATABASE [OnlineMusicDB] SET  READ_WRITE 
GO
