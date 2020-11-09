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
-- Create date: <2020-10-24>
-- =============================================
USE MIS
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_GetDispanserizationTotals]
	@beginDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	DECLARE @roomIDs TABLE (
		[ID] INT NOT NULL
	)

	SELECT
		t.[rf_DocPRVDID] AS [ResourceID]
		,t.[Date] AS [Date]
		,COUNT(t.[DoctorTimeTableID]) AS [TimesCount]
		,COUNT(v.[DoctorVisitTableID]) AS [VisitsCount]
	FROM
		[dbo].[hlt_DoctorTimeTable] AS t LEFT OUTER JOIN
		[dbo].[hlt_DoctorVisitTable] AS v ON t.[DoctorTimeTableID] = v.[rf_DoctorTimeTableID] INNER JOIN
		[dbo].[hlt_DocPRVD] AS r ON t.[rf_DocPRVDID] = r.[DocPRVDID] INNER JOIN
		[dbo].[dd_DDService] AS ds ON r.[rf_HealingRoomID] = ds.[rf_HealingRoomID]
	WHERE
		t.[Date] BETWEEN @beginDate AND @endDate
		AND t.[Begin_Time] > '19000101'
		AND t.[FlagAccess] BETWEEN 4 AND 7
		AND r.[InTime] = 1
		AND ds.[rf_HealingRoomID] > 0
		AND ds.[IsParaclinic] = 1
		AND ds.[rf_ServiceTypeID] = 2
	GROUP BY
		t.[rf_DocPRVDID]
		,t.[Date]
END
GO