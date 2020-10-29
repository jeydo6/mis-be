-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-26>
-- =============================================
USE [MIS]
GO

CREATE PROCEDURE [dbo].[sp_Resources_GetDispanserizations]
AS
BEGIN
	SELECT
		 res.[DocPRVDID] AS [ID]
		,res.[rf_LPUDoctorID] AS [DoctorID]
		,res.[rf_HealingRoomID] AS [RoomID]
		,doc.[LPUDoctorID] AS [ID]
		,doc.[PCOD] AS [Code]
		,doc.[IM_V] AS [FirstName]
		,doc.[OT_V] AS [MiddleName]
		,doc.[FAM_V] AS [LastName]
		,res.[rf_PRVSID] AS [SpecialtyID]
		,spec.[PRVSID] AS [ID]
		,spec.[C_PRVS] AS [Code]
		,spec.[PRVS_NAME] AS [Name]
		,room.[HealingRoomID] AS [ID]
		,room.[Num] AS [Code]
		,room.[Flat] AS [Flat]
	FROM
		[dbo].[hlt_DocPRVD] AS res INNER JOIN
		[dbo].[hlt_LPUDoctor] AS doc ON res.[rf_LPUDoctorID] = doc.[LPUDoctorID] INNER JOIN
		[dbo].[oms_PRVS] AS spec ON res.[rf_PRVSID] = spec.[PRVSID] INNER JOIN
		[dbo].[hlt_HealingRoom] AS room ON res.[rf_HealingRoomID] = room.[HealingRoomID] INNER JOIN
		[dbo].[dd_DDService] AS ds ON res.[rf_HealingRoomID] = ds.[rf_HealingRoomID]
	WHERE
		res.[InTime] = 1
		AND res.[rf_ResourceTypeID] = 2
		AND ds.[rf_HealingRoomID] > 0
		AND ds.[IsParaclinic] = 1
		AND ds.[rf_ServiceTypeID] = 2
END
GO
