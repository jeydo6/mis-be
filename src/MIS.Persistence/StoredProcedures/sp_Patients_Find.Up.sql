-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-19>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Patients_Find]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Patients_Find]
GO

CREATE PROCEDURE [dbo].[sp_Patients_Find]
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
		,p.[Gender]
	FROM
		[dbo].[Patients] AS p
	WHERE
		p.[Code] = @code
		AND YEAR(p.[BirthDate]) = YEAR(@birthDate)
END
GO
