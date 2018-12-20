USE [master]
GO

/****** Object:  Database [TestAppAuth]    Script Date: 12/20/2018 11:15:24 ******/
CREATE DATABASE [TestAppAuth]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TestAppAuth', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\TestAppAuth.mdf' , SIZE = 8192KB , MAXSIZE = 1024000KB , FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TestAppAuth_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\TestAppAuth_log.ldf' , SIZE = 8192KB , MAXSIZE = 1024000KB , FILEGROWTH = 65536KB )
GO

ALTER DATABASE [TestAppAuth] SET COMPATIBILITY_LEVEL = 140
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TestAppAuth].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [TestAppAuth] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TestAppAuth] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TestAppAuth] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TestAppAuth] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TestAppAuth] SET ARITHABORT OFF 
GO

ALTER DATABASE [TestAppAuth] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [TestAppAuth] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [TestAppAuth] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TestAppAuth] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TestAppAuth] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TestAppAuth] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TestAppAuth] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TestAppAuth] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TestAppAuth] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TestAppAuth] SET  DISABLE_BROKER 
GO

ALTER DATABASE [TestAppAuth] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TestAppAuth] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TestAppAuth] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TestAppAuth] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TestAppAuth] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TestAppAuth] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [TestAppAuth] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TestAppAuth] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [TestAppAuth] SET  MULTI_USER 
GO

ALTER DATABASE [TestAppAuth] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TestAppAuth] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TestAppAuth] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [TestAppAuth] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [TestAppAuth] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [TestAppAuth] SET QUERY_STORE = OFF
GO

ALTER DATABASE [TestAppAuth] SET  READ_WRITE 
GO


