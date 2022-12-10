-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2022-05-03>
-- Update date: <2022-11-04>
-- =============================================

IF OBJECT_ID('[dbo].[VisitItems]', 'U') IS NULL
	CREATE TABLE [dbo].[VisitItems]
	(
		[ID] INT IDENTITY NOT NULL PRIMARY KEY,
		[PatientID] INT NOT NULL,
		[TimeItemID] INT NOT NULL

		CONSTRAINT [FK_VisitItems_Patients] FOREIGN KEY ([PatientID]) REFERENCES [Patients]([ID]),
		CONSTRAINT [FK_VisitItems_TimeItems] FOREIGN KEY ([TimeItemID]) REFERENCES [TimeItems]([ID])
	)
GO
