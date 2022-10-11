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
-- Update date: <2022-05-04>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_Patients_First]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Patients_First]
GO

CREATE PROCEDURE [dbo].[sp_Patients_First]
	@code NVARCHAR(8),
	@birthDate DATE
AS
BEGIN
	SELECT TOP (1)
		 p.[ID]
		,p.[Code]
		,p.[FirstName]
		,p.[MiddleName]
		,p.[LastName]
		,p.[BirthDate]
		,p.[GenderID]
	FROM
		[dbo].[Patients] AS p
	WHERE
		p.[Code] = @code
		AND YEAR(p.[BirthDate]) = YEAR(@birthDate)
END
GO
