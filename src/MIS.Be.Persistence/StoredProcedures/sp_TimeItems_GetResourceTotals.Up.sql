-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-19>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_TimeItems_GetResourceTotals]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_TimeItems_GetResourceTotals]
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_GetResourceTotals]
	@beginDate DATETIME,
	@endDate DATETIME,
	@specialtyID INT = 0
AS
BEGIN
	SELECT
		 t.[ResourceID]
		,t.[Date]
		,MIN(t.[BeginDateTime]) AS [BeginDateTime]
		,MAX(t.[EndDateTime]) AS [EndDateTime]
		,COUNT(t.[ID]) AS [TimesCount]
		,COUNT(v.[ID]) AS [VisitsCount]
	FROM
		[dbo].[TimeItems] AS t INNER JOIN
		[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] LEFT JOIN
		[dbo].[VisitItems] AS v ON v.[TimeItemID] = t.[ID]
	WHERE
		t.[Date] BETWEEN @beginDate AND @endDate
		AND t.[BeginDateTime] >= @beginDate
		AND (@specialtyID = 0 OR s.[ID] = @specialtyID)
		AND r.[IsDispanserization] = 0
		AND r.[IsActive] = 1
	GROUP BY
		 t.[ResourceID]
		,t.[Date]
END
GO
