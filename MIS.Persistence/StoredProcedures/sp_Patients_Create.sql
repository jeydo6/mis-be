-- =============================================
-- Licensed under the Apache License, Version 2.0 (the "License");
-- you may not use this file except in compliance with the License.
-- You may obtain a copy of the License at
--
--     http://www.apache.org/licenses/LICENSE-2.0
--
-- Unless required by applicable law or agreed to in writing, software
-- distributed under the License is distributed on an "AS IS" BASIS,
-- WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
-- See the License for the specific language governing permissions and
-- limitations under the License.
-- =============================================
 
-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-05-04>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_Patients_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Patients_Create]
GO

CREATE PROCEDURE [dbo].[sp_Patients_Create]
	 @code NVARCHAR(8)
	,@firstName NVARCHAR(64)
	,@middleName NVARCHAR(64)
	,@lastName NVARCHAR(128)
	,@birthDate DATETIME
	,@genderID NVARCHAR(16)
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[Patients] AS p
		WHERE
			p.[Code] = @code
	) = 0
	BEGIN
		INSERT INTO
			[dbo].[Patients]
			(
				 p.[Code]
				,p.[FirstName]
				,p.[MiddleName]
				,p.[LastName]
				,p.[BirthDate]
				,p.[GenderID]
			)
			VALUES
			(
				 @code
				,@firstName
				,@middleName
				,@lastName
				,@birthDate
				,@genderID
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
