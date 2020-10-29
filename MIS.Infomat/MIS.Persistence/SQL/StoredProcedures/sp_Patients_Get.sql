-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-29>
-- =============================================
USE MIS
GO

CREATE PROCEDURE [dbo].[sp_Patients_Get]
	@patientID INT
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
		p.[MKABID] = @patientID
END
GO