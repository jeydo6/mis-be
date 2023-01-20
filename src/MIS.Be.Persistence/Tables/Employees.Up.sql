-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-26>
-- Update date: <2022-11-04>
-- =============================================

IF OBJECT_ID('[dbo].[Employees]', 'U') IS NULL
	CREATE TABLE [dbo].[Employees]
	(
		[ID] INT IDENTITY NOT NULL PRIMARY KEY,
		[Code] NVARCHAR(16) NOT NULL,
		[FirstName] NVARCHAR(128) NOT NULL,
		[MiddleName] NVARCHAR(128) NOT NULL,
		[LastName] NVARCHAR(128) NOT NULL,
		[SpecialtyID] INT NOT NULL,

		CONSTRAINT [FK_Employees_Specialties] FOREIGN KEY ([SpecialtyID]) REFERENCES [Specialties]([ID])
	)
GO
