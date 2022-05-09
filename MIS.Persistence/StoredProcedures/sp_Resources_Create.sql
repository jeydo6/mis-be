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

IF OBJECT_ID('[dbo].[sp_Resources_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Resources_Create]
GO

-- DEPRECATED
CREATE PROCEDURE [dbo].[sp_Resources_Create]
	 @employeeID INT
	,@employeeCode NVARCHAR(16) = ''
	,@employeeFirstName NVARCHAR(64) = ''
	,@employeeMiddleName NVARCHAR(64) = ''
	,@employeeLastName NVARCHAR(128) = ''
	,@specialtyID INT
	,@specialtyCode NVARCHAR(16) = ''
	,@specialtyName NVARCHAR(128) = ''
	,@roomID INT
	,@roomCode NVARCHAR(16) = ''
	,@roomFlat INT = 0
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[hlt_DocPRVD] AS r
		WHERE
			r.[rf_LPUDoctorID] = @employeeID
			AND r.[rf_HealingRoomID] = @roomID
	) = 0
	BEGIN
		IF @roomID < 0
		BEGIN
			INSERT INTO
				[dbo].[hlt_HealingRoom]
				(
					 [x_Edition]
					,[x_Status]
					,[Num]
					,[Comment]
					,[Flat]
					,[UGUID]
					,[InTime]
					,[rf_DepartmentID]
				)
			VALUES
				(
					 0
					,0
					,@roomCode
					,''
					,@roomFlat
					,NEWID()
					,1
					,0
				)

			SET @roomID = CAST(IDENT_CURRENT('[dbo].[hlt_HealingRoom]') AS INT)
		END
		IF @specialtyID < 0
		BEGIN
			INSERT INTO
				[dbo].[oms_PRVS]
				(
					 [x_Edition]
					,[x_Status]
					,[C_PRVS]
					,[PRVS_NAME]
					,[MSG_TEXT]
					,[I_PRVS]
					,[Date_Beg]
					,[Date_End]
					,[rf_MainPRVSID]
					,[rf_PRVSID]
				)
			VALUES
				(
					 0
					,0
					,@specialtyCode
					,@specialtyName
					,''
					,0
					,GETDATE()
					,DATEADD(YEAR, 100, GETDATE())
					,0
					,0
				)

			SET @specialtyID = CAST(IDENT_CURRENT('[dbo].[oms_PRVS]') AS INT)
		END
		IF @employeeID < 0
		BEGIN
			INSERT INTO
				[dbo].[hlt_LPUDoctor]
				(
					 [x_Edition]
					,[x_Status]
					,[PCOD]
					,[OT_V]
					,[IM_V]
					,[D_SER]
					,[rf_PRVSID]
					,[FAM_V]
					,[rf_KV_KATID]
					,[MSG_Text]
					,[rf_LPUID]
					,[isDoctor]
					,[rf_HealingRoomID]
					,[inTime]
					,[DR]
					,[IsSpecial]
					,[rf_PRVDID]
					,[UGUID]
					,[SS]
					,[rf_DepartmentID]
				)
			VALUES
				(
					 0
					,0
					,@employeeCode
					,@employeeMiddleName
					,@employeeFirstName
					,GETDATE()
					,@specialtyID
					,@employeeLastName
					,0
					,''
					,921
					,1
					,@roomID
					,1
					,GETDATE()
					,0
					,0
					,NEWID()
					,''
					,0
				)

			SET @employeeID = CAST(IDENT_CURRENT('[dbo].[hlt_LPUDoctor]') AS INT)
		END

		INSERT INTO
			[dbo].[hlt_DocPRVD]
				(
					[x_Edition]
				   ,[x_Status]
				   ,[rf_LPUDoctorID]
				   ,[D_PRIK]
				   ,[S_ST]
				   ,[D_END]
				   ,[rf_PRVSID]
				   ,[rf_HealingRoomID]
				   ,[rf_KV_KATID]
				   ,[rf_DepartmentID]
				   ,[MainWorkPlace]
				   ,[InTime]
				   ,[GUID]
				   ,[rf_PRVDID]
				   ,[Name]
				   ,[rf_EquipmentID]
				   ,[rf_ResourceTypeID]
				   ,[ShownInSchedule]
				   ,[ERID]
				   ,[ERName]
				   ,[NomServiceCode]
				   ,[isSpecial]
				   ,[rf_kl_DepartmentTypeID]
				   ,[isDismissal]
				   ,[Interval]
				   ,[isUseInterval]
				   ,[PCOD]
				)
			 VALUES
				(
					 0
					,0
					,@employeeID
					,GETDATE()
					,1
					,DATEADD(YEAR, 100, GETDATE())
					,@specialtyID
					,@roomID
					,0
					,0
					,1
					,1
					,NEWID()
					,0
					,''
					,0
					,1
					,1
					,''
					,''
					,''
					,0
					,0
					,0
					,0
					,0
					,''
				)

		SELECT CAST(IDENT_CURRENT('[dbo].[hlt_DocPRVD]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Resource: (doctorID: ''' + CAST(@employeeID AS NVARCHAR(10)) + ''', roomID: ''' + CAST(@roomID AS NVARCHAR(10)) + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO