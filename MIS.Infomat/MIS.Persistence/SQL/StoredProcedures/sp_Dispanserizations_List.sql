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
-- =============================================
USE MIS
GO

DROP PROCEDURE IF EXISTS [dbo].[sp_Dispanserizations_List]
GO

CREATE PROCEDURE [dbo].[sp_Dispanserizations_List]
	@patientID INT
AS
BEGIN
	SELECT
		 d.[DDFormID] AS [ID]
		,d.[dateDispBeg] AS [BeginDate]
		,d.[DateENDDD] AS [EndDate]
		,d.[IsClosed] AS [IsClosed]
		,p.[MKABID] AS [PatientID]
		,dt.[DDResearchLFID] AS [ID]
		,dt.[AddInfo] AS [Description]
	FROM
		[dbo].[dd_DDForm] AS d INNER JOIN
		[dbo].[hlt_MKAB] AS p ON d.[MKABGuid] = p.[UGUID] INNER JOIN
		[dbo].[dd_DDResearchLF] AS dt ON d.[DDFormID] = dt.[rf_DDFormID]
	WHERE
		p.[MKABID] = @patientID
END
GO