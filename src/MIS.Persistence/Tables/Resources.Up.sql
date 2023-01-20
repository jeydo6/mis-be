-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-26>
-- Update date: <2022-11-04>
-- =============================================

IF OBJECT_ID('[dbo].[Resources]', 'U') IS NULL
	CREATE TABLE [dbo].[Resources]
	(
		[ID] INT IDENTITY NOT NULL PRIMARY KEY,
		[Name] NVARCHAR(128) NOT NULL,
		[Type] INT NOT NULL,
		[IsDispanserization] BIT NOT NULL,
		[IsActive] BIT NOT NULL,
		[EmployeeID] INT NOT NULL,
		[RoomID] INT NOT NULL

		CONSTRAINT [FK_Resources_Employees] FOREIGN KEY ([EmployeeID]) REFERENCES [Employees]([ID]),
		CONSTRAINT [FK_Resources_Rooms] FOREIGN KEY ([RoomID]) REFERENCES [Rooms]([ID])
	)
GO
