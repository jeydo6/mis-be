-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-26>
-- Update date: <2022-11-02>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Resources_GetDispanserizations]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Resources_GetDispanserizations]
GO

CREATE PROCEDURE [dbo].[sp_Resources_GetDispanserizations]
AS
BEGIN
	SELECT
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
		,rm.[ID]
		,rm.[Code]
		,rm.[Floor]
	FROM
		[dbo].[Resources] AS r INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID]
	WHERE
		r.[IsDispanserization] = 1
		AND r.[IsActive] = 1
		AND r.[Type] = 2
END
GO
