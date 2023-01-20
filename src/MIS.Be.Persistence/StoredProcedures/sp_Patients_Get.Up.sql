-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-29>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Patients_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Patients_Get]
GO

CREATE PROCEDURE [dbo].[sp_Patients_Get]
	@id INT
AS
BEGIN
	SELECT TOP (1)
		 p.[ID]
		,p.[Code]
		,p.[FirstName]
		,p.[MiddleName]
		,p.[LastName]
		,p.[BirthDate]
		,p.[Gender]
	FROM
		[dbo].[Patients] AS p
	WHERE
		p.[ID] = @id
END
GO
