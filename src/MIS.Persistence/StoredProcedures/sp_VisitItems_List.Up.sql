-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-27>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_VisitItems_List]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_VisitItems_List]
GO

CREATE PROCEDURE [dbo].[sp_VisitItems_List]
	 @beginDate DATETIME
	,@endDate DATETIME
	,@patientID INT = 0
AS
BEGIN
	SELECT
		 v.[ID]
		,v.[PatientID]
		,v.[TimeItemID]
		,t.[ID]
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
	FROM
		[dbo].[VisitItems] AS v INNER JOIN
		[dbo].[TimeItems] AS t ON t.[ID] = v.[TimeItemID] INNER JOIN
		[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID]
	WHERE
		t.[Date] BETWEEN @beginDate AND @endDate
		AND t.[BeginDateTime] >= @beginDate
		AND (@patientID = 0 OR v.[PatientID] = @patientID)
		AND r.[IsActive] = 1
END
GO
