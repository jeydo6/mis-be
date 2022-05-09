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
-- Update date: <2022-05-10>
-- =============================================
USE [MIS]
GO

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
			[dbo].[TimeItems] AS t
		WHERE
			t.[ResourceID] = @resourceID
			AND t.[BeginDateTime] = @beginDateTime
			AND t.[EndDateTime] = @endDateTime
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
