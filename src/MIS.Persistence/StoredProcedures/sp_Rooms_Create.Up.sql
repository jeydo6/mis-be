-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Rooms_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Rooms_Create]
GO

CREATE PROCEDURE [dbo].[sp_Rooms_Create]
	 @code NVARCHAR(16)
	,@floor INT
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[Rooms]
		WHERE
			[Code] = @code
	) = 0
	BEGIN
		INSERT INTO
			[dbo].[Rooms]
			(
				 [Code]
				,[Floor]
			)
			VALUES
			(
				 @code
				,@floor
			)

		SELECT CAST(IDENT_CURRENT('[dbo].[Rooms]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Room: (code: ''' + @code + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO
