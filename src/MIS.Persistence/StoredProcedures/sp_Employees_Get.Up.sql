-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-29>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Employees_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Employees_Get]
GO

CREATE PROCEDURE [dbo].[sp_Employees_Get]
	@id INT
AS
BEGIN
	SELECT TOP (1)
		 e.[ID]
		,e.[Code]
		,e.[FirstName]
		,e.[MiddleName]
		,e.[LastName]
		,e.[SpecialtyID]
		,s.[ID]
		,s.[Code]
		,s.[Name]
	FROM
		[dbo].[Employees] AS e INNER JOIN
		[dbo].[Specialties] AS s ON e.[SpecialtyID] = s.[ID]
	WHERE
		e.[ID] = @id
END
GO
