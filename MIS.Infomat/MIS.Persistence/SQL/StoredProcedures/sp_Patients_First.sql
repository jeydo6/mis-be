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
USE MIS
GO

DROP PROCEDURE IF EXISTS [dbo].[sp_Patients_First]
GO

CREATE PROCEDURE [dbo].[sp_Patients_First]
	@code NVARCHAR(8),
	@birthDate DATETIME
AS
BEGIN
	SELECT TOP (1)
		 p.[MKABID] AS [ID]
		,p.[NUM] AS [Code]
		,p.[NAME] AS [FirstName]
		,p.[OT] AS [MiddleName]
		,p.[DATE_BD] AS [BirthDate]
		,p.[W] AS [Gender]
	FROM
		[dbo].[hlt_MKAB] AS p
	WHERE
		p.[NUM] = @code
		AND p.[DATE_BD] = @birthDate
END
GO