-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-21>
-- =============================================
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