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
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_TimeItems_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_TimeItems_Create]
GO

CREATE PROCEDURE [dbo].[sp_TimeItems_Create]
	 @date DATETIME
	,@beginDateTime DATETIME
	,@endDateTime DATETIME
	,@resourceID INT
AS
BEGIN
	DECLARE @msg VARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[hlt_DoctorTimeTable] AS t
		WHERE
			t.[rf_DocPRVDID] = @resourceID
			AND t.[Begin_Time] = @beginDateTime
			AND t.[End_Time] = @endDateTime
	) > 0
	BEGIN
		IF
		(
			SELECT
				COUNT(*)
			FROM
				[dbo].[hlt_DocPRVD] AS r
			WHERE
				r.[DocPRVDID] = @resourceID
		) > 0
		BEGIN
			SET @msg = 'Resource: (resourceID: ''' + CAST(@resourceID AS VARCHAR(10)) + ''') doesn''t exist'
			RAISERROR(@msg, 10, 1)
		END

		INSERT INTO
			[dbo].[hlt_DoctorTimeTable]
			(
				 [x_Edition]
				,[x_Status]
				,[Begin_Time]
				,[End_Time]
				,[Date]
				,[rf_LPUDoctorID]
				,[rf_DocBusyType]
				,[FlagAccess]
				,[FLAGS]
				,[UGUID]
				,[rf_HealingRoomID]
				,[PlanUE]
				,[TTSource]
				,[rf_DocPRVDID]
				,[LastStubNum]
				,[UsedUE]
			)
		SELECT
			 0
			,0
			,@beginDateTime
			,@endDateTime
			,@date
			,r.[rf_LPUDoctorID]
			,3
			,7
			,0
			,NEWID()
			,r.[rf_HealingRoomID]
			,1
			,0
			,r.[DocPRVDID]
			,0
			,0
		FROM
			[dbo].[hlt_DocPRVD] AS r
		WHERE
			r.[DocPRVDID] = @resourceID

		SELECT CAST(IDENT_CURRENT('[dbo].[hlt_DoctorTimeTable]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'TimeItem: (resourceID: ''' + CAST(@resourceID AS VARCHAR(10)) + ''', beginDateTime: ''' + CONVERT(VARCHAR(16), @beginDateTime, 104) + ''', endDateTime: ''' + CONVERT(VARCHAR(16), @endDateTime, 104) + ''') already exists'
		RAISERROR(@msg, 10, 1)
	END
END
GO