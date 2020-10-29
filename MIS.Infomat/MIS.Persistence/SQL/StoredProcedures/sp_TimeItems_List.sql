-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-21>
-- =============================================
USE MIS
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_List]
	 @beginDate DATETIME
	,@endDate DATETIME
	,@resourceID INT = 0
AS
BEGIN
	SELECT
		 t.[DoctorTimeTableID] AS [ID]
		,t.[Date] AS [Date]
		,t.[Begin_Time] AS [BeginDateTime]
		,t.[End_Time] AS [EndDateTime]
		,t.[rf_DocPRVDID] AS [ResourceID]
		,r.[DocPRVDID] AS [ID]
		,r.[rf_LPUDoctorID] AS [DoctorID]
		,r.[rf_HealingRoomID] AS [RoomID]
		,d.[LPUDoctorID] AS [ID]
		,d.[PCOD] AS [Code]
		,d.[IM_V] AS [FirstName]
		,d.[OT_V] AS [MiddleName]
		,d.[FAM_V] AS [LastName]
		,r.[rf_PRVSID] AS [SpecialtyID]
		,s.[PRVSID] AS [ID]
		,s.[C_PRVS] AS [Code]
		,s.[PRVS_NAME] AS [Name]
		,room.[HealingRoomID] AS [ID]
		,room.[Num] AS [Code]
		,room.[Flat] AS [Flat]
		,v.[DoctorVisitTableID] AS [ID]
		,v.[Comment] AS [PatientString]
		,IIF(CHARINDEX('.', v.[StubNum]) > 0, SUBSTRING(v.[StubNum], 1, CHARINDEX('.', v.[StubNum]) - 1), v.[StubNum]) AS [TimeString]
		,v.[rf_MKABID] AS [PatientID]
		,v.[rf_DoctorTimeTableID] AS [TimeItemID]
	FROM
		[dbo].[hlt_DoctorTimeTable] AS t LEFT OUTER JOIN
		[dbo].[hlt_DoctorVisitTable] AS v ON t.[DoctorTimeTableID] = v.[rf_DoctorTimeTableID] INNER JOIN
		[dbo].[hlt_DocPRVD] AS r ON t.[rf_DocPRVDID] = r.[DocPRVDID] AND r.[rf_PRVSID] > 0 INNER JOIN
		[dbo].[hlt_LPUDoctor] AS d ON r.[rf_LPUDoctorID] = d.[LPUDoctorID] AND r.[rf_LPUDoctorID] > 0 INNER JOIN
		[dbo].[oms_PRVS] AS s ON r.[rf_PRVSID] = s.[PRVSID] AND r.[rf_PRVSID] > 0 INNER JOIN
		[dbo].[hlt_HealingRoom] AS room ON r.[rf_HealingRoomID] = room.[HealingRoomID] AND r.[rf_HealingRoomID] > 0
	WHERE
		t.[Date] BETWEEN @beginDate AND @endDate
		AND t.[Begin_Time] > '19000101'
		AND t.[FlagAccess] BETWEEN 4 AND 7
		AND (@resourceID = 0 OR t.[rf_DocPRVDID] = @resourceID)
		AND r.[InTime] = 1
END
GO
