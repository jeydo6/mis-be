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
-- Create date: <2020-10-24>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_VisitItems_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_VisitItems_Create]
GO

CREATE PROCEDURE [dbo].[sp_VisitItems_Create]
	 @patientID INT
	,@timeItemID INT
AS
BEGIN
	DECLARE @msg VARCHAR(128)

	IF (SELECT COUNT(*) FROM [dbo].[hlt_DoctorVisitTable] AS v WHERE v.[rf_DoctorTimeTableID] = @timeItemID) = 0
	BEGIN
		INSERT INTO
			[dbo].[hlt_DoctorVisitTable] (
				 [x_Edition]
				,[x_Status] 
				,[rf_DoctorTimeTableID] 
				,[rf_TAPID] 
				,[Comment]
				,[rf_MKABID]
				,[StubPrintCounter]
				,[VisitStatus]
				,[Flags]
				,[UGUID]
				,[rf_UserID]
				,[rf_DocPRVDID]
				,[fromReg]
				,[fromDoc]
				,[fromInfomat]
				,[fromInternet]
				,[fromTel]
				,[NormaUE]
				,[StubNum]
				,[confirmRequired]
				,[editHistory]
				,[fromOtherLPU]
				,[rf_DirectionID]
				,[ERID]
				,[ReservedTo]
				,[FactBeginTime]
				,[FactEndTime]
			)
		SELECT
			 1
			,1
			,t.[DoctorTimeTableID]
			,0
			,(p.[NUM] + ', ' + p.[NAME] + ' ' + p.[OT] + ', ' + CAST(YEAR(p.[DATE_BD]) AS VARCHAR(4)) + ' ã.ð.')
			,p.[MKABID]
			,0
			,0
			,4
			,NEWID()
			,133
			,t.[rf_DocPRVDID]
			,0
			,0
			,1
			,0
			,0
			,1
			,CONVERT(NVARCHAR(2), DATEPART(HOUR, t.[Begin_Time])) + ':' + CONVERT(NVARCHAR(2), DATEPART(MINUTE, t.[Begin_Time]))
			,0
			,''
			,0
			,0
			,''
			,DATEADD(YEAR, 100, GETDATE())
			,GETDATE()
			,DATEADD(DAY, 1, GETDATE())
		FROM
			[dbo].[hlt_MKAB] AS p CROSS JOIN
			[dbo].[hlt_DoctorTimeTable] AS t
		WHERE
			p.[MKABID] = @patientID
			AND t.[DoctorTimeTableID] = @timeItemID

		SELECT CAST(IDENT_CURRENT('[dbo].[hlt_DoctorVisitTable]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'VisitItem: (timeItemID: ''' + CAST(@timeItemID AS VARCHAR(10)) + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO