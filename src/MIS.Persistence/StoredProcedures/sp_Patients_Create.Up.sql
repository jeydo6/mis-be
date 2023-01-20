-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Patients_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Patients_Create]
GO

CREATE PROCEDURE [dbo].[sp_Patients_Create]
	 @code NVARCHAR(8)
	,@firstName NVARCHAR(128)
	,@middleName NVARCHAR(128)
	,@lastName NVARCHAR(128)
	,@birthDate DATETIME
	,@gender INT
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[Patients]
		WHERE
			[Code] = @code
	) = 0
	BEGIN
		INSERT INTO
			[dbo].[Patients]
			(
				 [Code]
				,[FirstName]
				,[MiddleName]
				,[LastName]
				,[BirthDate]
				,[Gender]
			)
			VALUES
			(
				 @code
				,@firstName
				,@middleName
				,@lastName
				,@birthDate
				,@gender
			)

		SELECT CAST(IDENT_CURRENT('[dbo].[Patients]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Patient: (code: ''' + @code + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO
