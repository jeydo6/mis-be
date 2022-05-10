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
		 t.[rf_DocPRVDID] AS [ResourceID]
		,t.[Date] AS [Date]
		,MIN(t.[Begin_Time]) AS [BeginTime]
		,MAX(t.[End_Time]) AS [EndTime]
		,COUNT(t.[DoctorTimeTableID]) AS [TimesCount]
		,COUNT(v.[DoctorVisitTableID]) AS [VisitsCount]
	FROM
		[dbo].[hlt_DoctorTimeTable] AS t LEFT OUTER JOIN
		[dbo].[hlt_DoctorVisitTable] AS v ON t.[DoctorTimeTableID] = v.[rf_DoctorTimeTableID] INNER JOIN
		[dbo].[hlt_DocPRVD] AS r ON t.[rf_DocPRVDID] = r.[DocPRVDID]
	WHERE
		t.[Date] BETWEEN @beginDate AND @endDate
		AND t.[Begin_Time] >= @beginDate
		AND t.[FlagAccess] BETWEEN 4 AND 7
		AND (@specialtyID = 0 OR r.[rf_PRVSID] = @specialtyID)
		AND r.[InTime] = 1
		AND r.[rf_LPUDoctorID] > 0
		AND r.[rf_PRVSID] > 0
		AND r.[rf_HealingRoomID] > 0
		AND r.[rf_PRVDID] > 0
	GROUP BY
		 t.[rf_DocPRVDID]
		,t.[Date]
END
GO