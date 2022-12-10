-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2022-05-03>
-- Update date: <2022-11-04>
-- =============================================

IF OBJECT_ID('[dbo].[TimeItems]', 'U') IS NULL
	CREATE TABLE [dbo].[TimeItems]
	(
		[ID] INT IDENTITY NOT NULL PRIMARY KEY,
		[Date] DATE NOT NULL,
		[BeginDateTime] DATETIME NOT NULL,
		[EndDateTime] DATETIME NOT NULL,
		[ResourceID] INT NOT NULL

		CONSTRAINT [FK_TimeItems_Resources] FOREIGN KEY ([ResourceID]) REFERENCES [Resources]([ID])
	)
GO
