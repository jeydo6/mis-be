-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-10-24>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_VisitItems_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_VisitItems_Create]
GO

CREATE PROCEDURE [dbo].[sp_VisitItems_Create]
	 @patientID INT
	,@timeItemID INT
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF (SELECT COUNT(*) FROM [dbo].[VisitItems] AS v WHERE v.[TimeItemID] = @timeItemID) = 0
	BEGIN
		INSERT INTO
			[dbo].[VisitItems] (
				 [PatientID]
				,[TimeItemID]
			)
		VALUES
		(
			 @patientID
			,@timeItemID
		)

		SELECT CAST(IDENT_CURRENT('[dbo].[VisitItems]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'VisitItem: (timeItemID: ''' + CAST(@timeItemID AS NVARCHAR(10)) + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO
