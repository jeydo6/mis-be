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
-- Update date: <2022-05-09>
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
		 r.[ID] AS [ID]
		,r.[Name] AS [Name]
		,r.[EmployeeID] AS [EmployeeID]
		,r.[RoomID] AS [RoomID]
		,e.[ID] AS [ID]
		,e.[Code] AS [Code]
		,[dbo].[f_Resources_GetName](e.[FirstName], e.[MiddleName], e.[LastName]) AS [Name]
		,e.[SpecialtyID] AS [SpecialtyID]
		,s.[ID] AS [ID]
		,s.[Code] AS [Code]
		,s.[Name] AS [Name]
		,rm.[ID] AS [ID]
		,rm.[Code] AS [Code]
		,rm.[Flat] AS [Flat]
	FROM
		[dbo].[Resources] AS r INNER JOIN
		[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
		[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] INNER JOIN
		[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID]
	WHERE
		r.[IsActive] = 1
		AND r.[TypeID] = 2
		AND s.[Name] = N'Диспансеризация'
END
GO
