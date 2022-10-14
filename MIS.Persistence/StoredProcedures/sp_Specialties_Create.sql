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
