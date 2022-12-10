-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-26>
-- Update date: <2022-11-04>
-- =============================================

IF OBJECT_ID('[dbo].[Specialties]', 'U') IS NULL
	CREATE TABLE [dbo].[Specialties]
	(
		[ID] INT IDENTITY NOT NULL PRIMARY KEY,
		[Code] NVARCHAR(16) NOT NULL,
		[Name] NVARCHAR(128) NOT NULL
	)
GO
