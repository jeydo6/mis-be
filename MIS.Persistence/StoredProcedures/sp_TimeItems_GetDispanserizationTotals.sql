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
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_TimeItems_GetDispanserizationTotals]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_TimeItems_GetDispanserizationTotals]
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_GetDispanserizationTotals]
	@beginDate DATETIME,
	@endDate DATETIME
AS
BEGIN
	SELECT
		 t.[ResourceID]
		,t.[Date]
		,MIN(t.[BeginDateTime]) AS [BeginDateTime]
		,MAX(t.[EndDateTime]) AS [EndDateTime]
		,COUNT(t.[ID]) AS [TimesCount]
		,COUNT(v.[ID]) AS [VisitsCount]
	FROM
		[dbo].[TimeItems] AS t INNER JOIN
		[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID] LEFT JOIN
		[dbo].[VisitItems] AS v ON v.[TimeItemID] = t.[ID]
	WHERE
		t.[Date] BETWEEN @beginDate AND @endDate
		AND r.[IsDispanserization] = 1
		AND r.[IsActive] = 1
		AND r.[Type] = 2
	GROUP BY
		 t.[ResourceID]
		,t.[Date]
END
GO
