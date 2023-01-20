-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Specialties_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Specialties_Create]
GO

CREATE PROCEDURE [dbo].[sp_Specialties_Create]
	 @code NVARCHAR(16)
	,@name NVARCHAR(128)
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[Specialties]
		WHERE
			[Code] = @code
	) = 0
	BEGIN
		INSERT INTO
			[dbo].[Specialties]
			(
				 [Code]
				,[Name]
			)
			VALUES
			(
				 @code
				,@name
			)

		SELECT CAST(IDENT_CURRENT('[dbo].[Specialties]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Specialty: (code: ''' + @code + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO
