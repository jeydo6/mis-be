-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_TimeItems_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_TimeItems_Create]
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_Create]
	 @date DATE
	,@beginDateTime DATETIME
	,@endDateTime DATETIME
	,@resourceID INT
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[TimeItems]
		WHERE
			[ResourceID] = @resourceID
			AND [BeginDateTime] = @beginDateTime
			AND [EndDateTime] = @endDateTime
	) = 0
	BEGIN
		INSERT INTO
			[dbo].[TimeItems] (
				 [Date]
				,[BeginDateTime]
				,[EndDateTime]
				,[ResourceID]
			)
		VALUES
		(
			 @date
			,@beginDateTime
			,@endDateTime
			,@resourceID
		)

		SELECT CAST(IDENT_CURRENT('[dbo].[TimeItems]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'TimeItem: (resourceID: ''' + CAST(@resourceID AS NVARCHAR(10)) + ''', beginDateTime: ''' + CONVERT(VARCHAR(16), @beginDateTime, 104) + ''', endDateTime: ''' + CONVERT(VARCHAR(16), @endDateTime, 104) + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO
