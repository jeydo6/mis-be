-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-21>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Dispanserizations_List]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Dispanserizations_List]
GO

CREATE PROCEDURE [dbo].[sp_Dispanserizations_List]
	@patientID INT
AS
BEGIN
	SELECT
		 d.[ID]
		,d.[BeginDate]
		,d.[EndDate]
		,d.[IsClosed]
		,d.[PatientID]
		,v.[ID]
		,(r.[Name] + ': ' + CONVERT(VARCHAR(10), t.[Date], 104) + ' (' + rm.[Code] + N' ���.)') AS [Description]
	FROM
		[dbo].[Dispanserizations] AS d INNER JOIN
		[dbo].[VisitItems] AS v ON v.[PatientID] = d.[PatientID] INNER JOIN
		[dbo].[TimeItems] AS t ON t.[ID] = v.[TimeItemID] INNER JOIN
		[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID] INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID]
	WHERE
		d.[PatientID] = @patientID
		AND r.[IsDispanserization] = 1
		AND r.[IsActive] = 1
		AND r.[Type] = 2
END
GO
