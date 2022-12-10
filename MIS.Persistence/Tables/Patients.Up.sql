-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-26>
-- Update date: <2022-11-04>
-- =============================================

IF OBJECT_ID('[dbo].[Patients]', 'U') IS NULL
	CREATE TABLE [dbo].[Patients]
	(
		[ID] INT IDENTITY NOT NULL PRIMARY KEY,
		[Code] NVARCHAR(16) NOT NULL,
		[FirstName] NVARCHAR(128) NOT NULL,
		[MiddleName] NVARCHAR(128) NOT NULL,
		[LastName] NVARCHAR(128) NOT NULL,
		[BirthDate] DATE NOT NULL,
		[Gender] INT NOT NULL
	)
GO
