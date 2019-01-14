USE [TestAppAuth]
GO

IF OBJECT_ID('dbo.User', 'U') IS NULL
BEGIN

	CREATE TABLE [dbo].[User](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Username] [nvarchar](256) NOT NULL,
		[Password] [nvarchar](256) NOT NULL,
	 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Token]') AND type in (N'U'))
ALTER TABLE [dbo].[Token] DROP CONSTRAINT IF EXISTS [DF_Token_CreatedAt]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Token]') AND type in (N'U'))
ALTER TABLE [dbo].[Token] DROP CONSTRAINT IF EXISTS [DF_Token_Id]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Token]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Token](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedAtUtc] [datetime] NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Token_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_Id]  DEFAULT (newid()) FOR [Id]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Token_CreatedAtUtc]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_Token_CreatedAtUtc]  DEFAULT (getutcdate()) FOR [CreatedAtUtc]
END
GO