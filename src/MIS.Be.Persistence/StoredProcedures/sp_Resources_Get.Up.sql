-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Resources_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Resources_Get]
GO

CREATE PROCEDURE [dbo].[sp_Resources_Get]
	@id INT
AS
BEGIN
	SELECT TOP (1)
		 r.[ID]
		,r.[Name]
		,r.[Type]
		,r.[EmployeeID]
		,r.[RoomID]
		,e.[ID]
		,e.[Code]
		,e.[FirstName]
		,e.[MiddleName]
		,e.[LastName]
		,e.[SpecialtyID]
		,s.[ID]
		,s.[Code]
		,s.[Name]
		,room.[ID]
		,room.[Code]
		,room.[Floor]
	FROM
		[dbo].[Resources] AS r INNER JOIN
		[dbo].[Employees] AS e ON r.[EmployeeID] = e.[ID] INNER JOIN
		[dbo].[Specialties] AS s ON e.[SpecialtyID] = s.[ID] INNER JOIN
		[dbo].[Rooms] AS room ON r.[RoomID] = room.[ID]
	WHERE
		r.[ID] = @id
		AND r.[IsActive] = 1
END
GO
