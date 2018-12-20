USE [TestAppAuth]
GO

TRUNCATE TABLE dbo.[User]

INSERT INTO dbo.[User] (Username, [Password])
SELECT 'User1', 'password' UNION ALL
SELECT 'User2', 'password' UNION ALL
SELECT 'User3', 'password' UNION ALL
SELECT 'User4', 'password'

GO