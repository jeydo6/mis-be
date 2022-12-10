-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2022-11-04>
-- Update date: <2022-11-04>
-- =============================================

IF OBJECT_ID('[dbo].[Dispanserizations]', 'U') IS NULL
	CREATE TABLE [dbo].[Dispanserizations]
	(
		[ID] INT IDENTITY  NOT NULL PRIMARY KEY, 
		[BeginDate] DATETIME NOT NULL,
		[EndDate] DATETIME NOT NULL, 
		[IsClosed] BIT NOT NULL,
		[PatientID] INT NOT NULL, 

		CONSTRAINT [FK_Dispanserizations_Patients] FOREIGN KEY ([PatientID]) REFERENCES [Patients]([ID])
	)
GO
