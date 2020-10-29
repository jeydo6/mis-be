-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-19>
-- =============================================
USE MIS
GO

CREATE PROCEDURE [dbo].[sp_Resources_List]
AS
BEGIN
	SELECT
		 r.[DocPRVDID] AS [ID]
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
		[dbo].[hlt_DocPRVD] AS r INNER JOIN
		[dbo].[hlt_LPUDoctor] AS d ON r.[rf_LPUDoctorID] = d.[LPUDoctorID] AND r.[rf_LPUDoctorID] > 0 INNER JOIN
		[dbo].[oms_PRVS] AS s ON r.[rf_PRVSID] = s.[PRVSID] AND r.[rf_PRVSID] > 0 INNER JOIN
		[dbo].[hlt_HealingRoom] AS room ON r.[rf_HealingRoomID] = room.[HealingRoomID] AND r.[rf_HealingRoomID] > 0
	WHERE
		r.[InTime] = 1
END
