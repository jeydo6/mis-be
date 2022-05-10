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
-- Create date: <2020-10-21>
-- Update date: <2022-05-09>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_TimeItems_List]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_TimeItems_List]
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_List]
	 @beginDate DATETIME
	,@endDate DATETIME
	,@resourceID INT = 0
AS
BEGIN
	SELECT
		 t.[ID]
		,t.[Date]
		,t.[BeginDateTime]
		,t.[EndDateTime]
		,t.[ResourceID]
		,r.[ID]
		,r.[Name]
		,r.[EmployeeID]
		,r.[RoomID]
		,e.[ID]
		,e.[Code]
		,[dbo].[f_Resources_GetName](e.[FirstName], e.[MiddleName], e.[LastName]) AS [Name]
		,e.[SpecialtyID]
		,s.[ID]
		,s.[Code]
		,s.[Name]
		,rm.[ID]
		,rm.[Code]
		,rm.[Flat]
		,v.[ID]
		,v.[PatientID]
		,v.[TimeItemID]
	FROM
		[dbo].[TimeItems] AS t INNER JOIN
		[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID] LEFT JOIN
		[dbo].[VisitItems] AS v ON v.[TimeItemID] = t.[ID]
	WHERE
		t.[Date] BETWEEN @beginDate AND @endDate
		AND t.[BeginDateTime] >= @beginDate
		AND (@resourceID = 0 OR r.[ID] = @resourceID)
		AND r.[IsActive] = 1
		AND r.[TypeID] = 1

END
GO
