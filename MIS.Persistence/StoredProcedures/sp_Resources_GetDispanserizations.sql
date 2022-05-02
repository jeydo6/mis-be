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
-- Create date: <2020-10-26>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_Resources_GetDispanserizations]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Resources_GetDispanserizations]
GO

CREATE PROCEDURE [dbo].[sp_Resources_GetDispanserizations]
AS
BEGIN
	SELECT
		 r.[DocPRVDID] AS [ID]
		,(
			LTRIM(RTRIM(d.[FAM_V])) +
			(CASE WHEN LEN(LTRIM(RTRIM(d.[IM_V]))) > 0 THEN ' ' + SUBSTRING(LTRIM(RTRIM(d.[IM_V])), 1, 1) + '.' ELSE '' END) +
			(CASE WHEN LEN(LTRIM(RTRIM(d.[OT_V]))) > 0 THEN ' ' + SUBSTRING(LTRIM(RTRIM(d.[OT_V])), 1, 1) + '.' ELSE '' END)
		) AS [Name]
		,r.[rf_LPUDoctorID] AS [EmployeeID]
		,d.[LPUDoctorID] AS [ID]
		,d.[PCOD] AS [Code]
		,d.[IM_V] AS [FirstName]
		,d.[OT_V] AS [MiddleName]
		,d.[FAM_V] AS [LastName]
		,r.[rf_PRVSID] AS [SpecialtyID]
		,s.[PRVSID] AS [ID]
		,s.[C_PRVS] AS [Code]
		,s.[PRVS_NAME] AS [Name]
		,r.[rf_HealingRoomID] AS [RoomID]
		,room.[HealingRoomID] AS [ID]
		,room.[Num] AS [Code]
		,room.[Flat] AS [Flat]
	FROM
		[dbo].[hlt_DocPRVD] AS r INNER JOIN
		[dbo].[hlt_LPUDoctor] AS d ON r.[rf_LPUDoctorID] = d.[LPUDoctorID] INNER JOIN
		[dbo].[oms_PRVS] AS s ON r.[rf_PRVSID] = s.[PRVSID] INNER JOIN
		[dbo].[hlt_HealingRoom] AS room ON r.[rf_HealingRoomID] = room.[HealingRoomID] INNER JOIN
		[dbo].[dd_DDService] AS ds ON r.[rf_HealingRoomID] = ds.[rf_HealingRoomID]
	WHERE
		r.[InTime] = 1
		AND ds.[rf_HealingRoomID] > 0
		AND ds.[IsParaclinic] = 1
		AND ds.[rf_ServiceTypeID] = 2
END
GO
