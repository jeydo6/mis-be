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
-- Create date: <2020-10-19>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Resources_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Resources_Create]
GO

CREATE PROCEDURE [dbo].[sp_Resources_Create]
	 @name NVARCHAR(128)
	,@type INT
	,@isDispanserization BIT
	,@employeeID INT
	,@roomID INT
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[Resources]
		WHERE
			[Name] = @name
	) = 0
	BEGIN
		INSERT INTO
			[dbo].[Resources]
			(
				 [Name]
				,[Type]
				,[IsDispanserization]
				,[IsActive]
				,[EmployeeID]
				,[RoomID]
			)
			VALUES
			(
				 @name
				,@type
				,@isDispanserization
				,1
				,@employeeID
				,@roomID
			)

		SELECT CAST(IDENT_CURRENT('[dbo].[Resources]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Resource: (name: ''' + @name + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
