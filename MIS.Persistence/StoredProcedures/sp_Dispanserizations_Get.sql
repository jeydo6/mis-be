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
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Dispanserizations_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Dispanserizations_Get]
GO

CREATE PROCEDURE [dbo].[sp_Dispanserizations_Get]
	@dispanserizationID INT
AS
BEGIN
	SELECT
		 d.[ID]
		,d.[BeginDate]
		,d.[EndDate]
		,d.[IsClosed]
		,d.[PatientID]
		,v.[ID]
		,(r.[Name] + ': ' + CONVERT(VARCHAR(10), t.[Date], 104) + ' (' + rm.[Code] + N' каб.)') AS [Description]
	FROM
		[dbo].[Dispanserizations] AS d INNER JOIN
		[dbo].[VisitItems] AS v ON v.[PatientID] = d.[PatientID] INNER JOIN
		[dbo].[TimeItems] AS t ON t.[ID] = v.[TimeItemID] INNER JOIN
		[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID] INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID]
	WHERE
		d.[ID] = @dispanserizationID
		AND r.[Type] = 2
		AND s.[Name] = N'Диспансеризация'
END
GO
