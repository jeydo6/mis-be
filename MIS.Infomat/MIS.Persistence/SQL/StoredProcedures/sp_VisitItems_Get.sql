-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-27>
-- =============================================
CREATE PROCEDURE [dbo].[sp_VisitItems_Get]
	@visitItemID INT
AS
BEGIN
SELECT
	 v.[DoctorVisitTableID] AS [ID]
	,v.[Comment] AS [PatientString]
	,IIF(CHARINDEX('.', v.[StubNum]) > 0, SUBSTRING(v.[StubNum], 1, CHARINDEX('.', v.[StubNum]) - 1), v.[StubNum]) AS [TimeString]
	,v.[rf_MKABID] AS [PatientID]
	,v.[rf_DoctorTimeTableID] AS [TimeItemID]
	,t.[DoctorTimeTableID] AS [ID]
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
FROM
	[dbo].[hlt_DoctorVisitTable] AS v INNER JOIN
	[dbo].[hlt_DoctorTimeTable] AS t ON v.[rf_DoctorTimeTableID] = t.[DoctorTimeTableID] INNER JOIN
	[dbo].[hlt_DocPRVD] AS r ON t.[rf_DocPRVDID] = r.[DocPRVDID] AND r.[rf_PRVSID] > 0 INNER JOIN
	[dbo].[hlt_LPUDoctor] AS d ON r.[rf_LPUDoctorID] = d.[LPUDoctorID] AND r.[rf_LPUDoctorID] > 0 INNER JOIN
	[dbo].[oms_PRVS] AS s ON r.[rf_PRVSID] = s.[PRVSID] AND r.[rf_PRVSID] > 0 INNER JOIN
	[dbo].[hlt_HealingRoom] AS room ON r.[rf_HealingRoomID] = room.[HealingRoomID] AND r.[rf_HealingRoomID] > 0
WHERE
	v.[DoctorVisitTableID] = @visitItemID
END
GO
