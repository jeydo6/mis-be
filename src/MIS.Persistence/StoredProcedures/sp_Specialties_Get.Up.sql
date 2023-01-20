-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Specialties_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Specialties_Get]
GO

CREATE PROCEDURE [dbo].[sp_Specialties_Get]
	@id INT
AS
BEGIN
	SELECT TOP (1)
		 s.[ID]
		,s.[Code]
		,s.[Name]
	FROM
		[dbo].[Specialties] AS s
	WHERE
		s.[ID] = @id
END
GO
