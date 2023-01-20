-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-21>
-- Update date: <2022-10-20>
-- =============================================

IF OBJECT_ID('[dbo].[sp_TimeItems_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_TimeItems_Get]
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_Get]
	 @id INT
AS
BEGIN
	SELECT TOP (1)
		 t.[ID]
		,t.[Date]
		,t.[BeginDateTime]
		,t.[EndDateTime]
		,t.[ResourceID]
		,r.[ID]
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
		,v.[ID]
		,v.[PatientID]
		,v.[TimeItemID]
	FROM
		[dbo].[TimeItems] AS t INNER JOIN
		[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID] LEFT JOIN
		[dbo].[VisitItems] AS v ON v.[TimeItemID] = t.[ID]
	WHERE
		t.[ID] = @id
		AND r.[IsActive] = 1
END
GO
