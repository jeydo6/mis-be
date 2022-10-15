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
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Resources_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Resources_Get]
GO

CREATE PROCEDURE [dbo].[sp_Resources_Get]
	@id INT
AS
BEGIN
	SELECT TOP (1)
		 r.[ID]
		,r.[Name]
		,r.[Type]
		,r.[IsActive]
		,r.[EmployeeID]
		,r.[RoomID]
		,e.[ID]
		,e.[Code]
		,e.[FirstName]
		,e.[MiddleName]
		,e.[LastName]
		,e.[SpecialtyID]
		,s.[ID]
		,s.[Code]
		,s.[Name]
		,room.[ID]
		,room.[Code]
		,room.[Floor]
	FROM
		[dbo].[Resources] AS r INNER JOIN
		[dbo].[Employees] AS e ON r.[EmployeeID] = e.[ID] INNER JOIN
		[dbo].[Specialties] AS s ON e.[SpecialtyID] = s.[ID] INNER JOIN
		[dbo].[Rooms] AS room ON r.[RoomID] = room.[ID]
	WHERE
		r.[ID] = @id
END
GO
