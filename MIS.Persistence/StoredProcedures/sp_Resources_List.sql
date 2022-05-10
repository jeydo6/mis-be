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
-- Update date: <2022-05-09>
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
		 r.[ID] AS [ID]
		,r.[Name] AS [Name]
		,r.[EmployeeID] AS [EmployeeID]
		,r.[RoomID] AS [RoomID]
		,e.[ID] AS [ID]
		,e.[Code] AS [Code]
		,[dbo].[f_Persons_GetName](e.[FirstName], e.[MiddleName], e.[LastName]) AS [Name]
		,e.[SpecialtyID] AS [SpecialtyID]
		,s.[ID] AS [ID]
		,s.[Code] AS [Code]
		,s.[Name] AS [Name]
		,rm.[ID] AS [ID]
		,rm.[Code] AS [Code]
		,rm.[Flat] AS [Flat]
	FROM
		[dbo].[Resources] AS r INNER JOIN
		[dbo].[Employees] AS e ON r.[EmployeeID] = e.[ID] INNER JOIN
		[dbo].[Specialties] AS s ON e.[SpecialtyID] = s.[ID] INNER JOIN
		[dbo].[Rooms] AS rm ON r.[RoomID] = rm.[ID]
	WHERE
		r.[IsActive] = 1
		AND r.[TypeID] = 1
END
