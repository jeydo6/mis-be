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

IF OBJECT_ID('[dbo].[sp_Patients_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Patients_Create]
GO

CREATE PROCEDURE [dbo].[sp_Patients_Create]
	 @code NVARCHAR(8)
	,@firstName NVARCHAR(64)
	,@middleName NVARCHAR(64)
	,@lastName NVARCHAR(128)
	,@birthDate DATETIME
	,@gender NVARCHAR(16)
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[hlt_MKAB] AS p
		WHERE
			p.[NUM] = @code
	) = 0
	BEGIN
		DECLARE @genderInt INT = CASE WHEN UPPER(SUBSTRING(LTRIM(RTRIM(@gender)), 1, 1)) = N'М' THEN 1 ELSE 0 END

		INSERT INTO
			[dbo].[hlt_MKAB]
			(
				 [x_Edition]
				,[x_Status]
				,[FAMILY]
				,[NAME]
				,[OT]
				,[SS]
				,[D_TYPE]
				,[NUM]
				,[W]
				,[DATE_BD]
				,[ADRES]
				,[rf_INVID]
				,[rf_SMOID]
				,[PhoneWork]
				,[PhoneHome]
				,[Work]
				,[Profession]
				,[Post]
				,[Dependent]
				,[rf_LPUID]
				,[rf_GroupOfBloodID]
				,[KindCod]
				,[RH]
				,[COD_Person]
				,[MilitaryCOD]
				,[rf_UchastokID]
				,[rf_CitizenID]
				,[rf_TYPEDOCID]
				,[S_POL]
				,[N_POL]
				,[UGUID]
				,[rf_OtherSMOID]
				,[S_DOC]
				,[N_DOC]
				,[IsWorker]
				,[rf_MKABLocationID]
				,[rf_SpecEventCertID]
				,[AdresFact]
				,[DatePolBegin]
				,[DatePolEnd]
				,[rf_EnterpriseID]
				,[BlackLabel]
				,[rf_OKATOID]
				,[FLAGS]
				,[isClosed]
				,[rf_ReasonCloseMKABID]
				,[DateClose]
				,[rf_kl_PrivilegeCategoryID]
				,[rf_omsOKVEDID]
				,[rf_kl_SocStatusID]
				,[rf_kl_SexID]
				,[rf_kl_HealthGroupID]
				,[rf_AddressLiveID]
				,[rf_AddressRegID]
				,[Hash0]
				,[Hash1]
				,[Hash2]
				,[Hash3]
				,[Hash4]
				,[isEncrypted]
				,[RIDN]
				,[mainContact]
				,[contactConfirm]
				,[contactEmail]
				,[contactMPhone]
				,[ConfirmAgree]
				,[ConfirmDate]
				,[ConfirmUserFIO]
				,[CreateUserName]
				,[EditUserName]
				,[MessageFLAG]
				,[rf_ConfirmUserID]
				,[rf_CreateUserID]
				,[rf_EditUserID]
				,[rf_kl_TipOMSID]
				,[Birthplace]
				,[MKABInfo]
				,[DateDoc]
				,[DateMKAB]
				,[isLSHome]
				,[MainMKABGuid]
			)
			VALUES
			(
				 0
				,0
				,@lastName
				,@firstName
				,@middleName
				,''
				,''
				,@code
				,@genderInt
				,@birthDate
				,''
				,0
				,0
				,''
				,''
				,''
				,''
				,''
				,0
				,921
				,0
				,0
				,0
				,@code
				,''
				,0
				,0
				,0
				,''
				,''
				,NEWID()
				,0
				,''
				,''
				,0
				,1
				,0
				,''
				,GETDATE()
				,GETDATE()
				,0
				,0
				,0
				,0
				,0
				,0
				,'1900-01-01'
				,0
				,0
				,28
				,0
				,0
				,0
				,0
				,''
				,''
				,''
				,''
				,''
				,0
				,''
				,0
				,0
				,''
				,''
				,0
				,'1900-01-01'
				,''
				,'Администратор'
				,'Администратор'
				,0
				,0
				,1
				,1
				,0
				,''
				,'<row TAPCount="0" SttMedHistoryCount="0" AnamnezCount="0" LSPurposeCount="0" ResearchCount="0" VisitHistoryCount="0" MedRecordCount="0" DirectionCount="0" FluorCardCount="0" NotWorkCount="0" DirectionManipulationCount="0" />'
				,'1900-01-01'
				,GETDATE()
				,0
				,0x0
			)

		SELECT CAST(IDENT_CURRENT('[dbo].[hlt_MKAB]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Patient: (code: ''' + @code + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO