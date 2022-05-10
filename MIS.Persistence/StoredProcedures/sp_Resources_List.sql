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
-- Create date: <2020-10-19>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_Resources_List]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Resources_List]
GO

CREATE PROCEDURE [dbo].[sp_Resources_List]
AS
BEGIN
	SELECT
		 r.[DocPRVDID] AS [ID]
		,ro.[NAME] AS [Name]
		,r.[rf_LPUDoctorID] AS [EmployeeID]
		,r.[rf_HealingRoomID] AS [RoomID]
		,d.[LPUDoctorID] AS [ID]
		,d.[PCOD] AS [Code]
		,[dbo].[f_Resources_GetName](d.[IM_V], d.[OT_V], d.[FAM_V]) AS [Name]
		,r.[rf_PRVSID] AS [SpecialtyID]
		,s.[PRVSID] AS [ID]
		,s.[C_PRVS] AS [Code]
		,s.[PRVS_NAME] AS [Name]
		,room.[HealingRoomID] AS [ID]
		,room.[Num] AS [Code]
		,room.[Flat] AS [Flat]
	FROM
		[dbo].[hlt_DocPRVD] AS r INNER JOIN
		[dbo].[oms_PRVD] AS ro ON ro.[PRVDID] = r.[rf_PRVDID] INNER JOIN
		[dbo].[hlt_LPUDoctor] AS d ON r.[rf_LPUDoctorID] = d.[LPUDoctorID] INNER JOIN
		[dbo].[oms_PRVS] AS s ON r.[rf_PRVSID] = s.[PRVSID] INNER JOIN
		[dbo].[hlt_HealingRoom] AS room ON r.[rf_HealingRoomID] = room.[HealingRoomID]
	WHERE
		r.[InTime] = 1
		AND r.[rf_LPUDoctorID] > 0
		AND r.[rf_PRVSID] > 0
		AND r.[rf_HealingRoomID] > 0
		AND r.[rf_PRVDID] > 0
END
