-- =============================================
-- Licensed under the Apache License, Version 2.0 (the "License");
-- you may not use this file except in compliance with the License.
-- You may obtain a copy of the License at
--
--     http://www.apache.org/licenses/LICENSE-2.0
--
-- Unless required by applicable law or agreed to in writing, software
-- distributed under the License is distributed on an "AS IS" BASIS,
-- WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
-- See the License for the specific language governing permissions and
-- limitations under the License.
-- =============================================
 
-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-27>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_VisitItems_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_VisitItems_Get]
GO

CREATE PROCEDURE [dbo].[sp_VisitItems_Get]
	@visitItemID INT
AS
BEGIN
	SELECT
		 v.[DoctorVisitTableID] AS [ID]
		,v.[Comment] AS [PatientString]
		,CASE WHEN CHARINDEX('.', v.[StubNum]) > 0 THEN SUBSTRING(v.[StubNum], 1, CHARINDEX('.', v.[StubNum]) - 1) ELSE v.[StubNum] END AS [TimeString]
		,v.[rf_MKABID] AS [PatientID]
		,v.[rf_DoctorTimeTableID] AS [TimeItemID]
		,t.[DoctorTimeTableID] AS [ID]
		,t.[Date] AS [Date]
		,t.[Begin_Time] AS [BeginDateTime]
		,t.[End_Time] AS [EndDateTime]
		,t.[rf_DocPRVDID] AS [ResourceID]
		,r.[DocPRVDID] AS [ID]
		,(
			LTRIM(RTRIM(d.[FAM_V])) +
			(CASE WHEN LEN(LTRIM(RTRIM(d.[IM_V]))) > 0 THEN ' ' + SUBSTRING(LTRIM(RTRIM(d.[IM_V])), 1, 1) + '.' ELSE '' END) +
			(CASE WHEN LEN(LTRIM(RTRIM(d.[OT_V]))) > 0 THEN ' ' + SUBSTRING(LTRIM(RTRIM(d.[OT_V])), 1, 1) + '.' ELSE '' END)
		) AS [Name]
		,r.[rf_HealingRoomID] AS [RoomID]
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
		[dbo].[hlt_DocPRVD] AS r ON t.[rf_DocPRVDID] = r.[DocPRVDID] INNER JOIN
		[dbo].[hlt_LPUDoctor] AS d ON r.[rf_LPUDoctorID] = d.[LPUDoctorID] INNER JOIN
		[dbo].[oms_PRVS] AS s ON r.[rf_PRVSID] = s.[PRVSID] INNER JOIN
		[dbo].[hlt_HealingRoom] AS room ON r.[rf_HealingRoomID] = room.[HealingRoomID]
	WHERE
		v.[DoctorVisitTableID] = @visitItemID
		AND r.[InTime] = 1
		AND r.[rf_PRVSID] > 0
		AND r.[rf_LPUDoctorID] > 0
		AND r.[rf_PRVSID] > 0
		AND r.[rf_HealingRoomID] > 0
END
GO
