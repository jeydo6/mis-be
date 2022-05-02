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
-- Create date: <2020-10-26>
-- Update date: <2022-04-25>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[sp_Dispanserizations_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Dispanserizations_Create]
GO

CREATE PROCEDURE [dbo].[sp_Dispanserizations_Create]
	 @patientID INT
	,@beginDate DATETIME
	,@endDate DATETIME
AS
BEGIN
	DECLARE @msg VARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[dd_DDForm] AS d INNER JOIN
			[dbo].[hlt_MKAB] AS p ON d.[MKABGuid] = p.[UGUID]
		WHERE
			p.[MKABID] = @patientID
			AND d.[IsClosed] = 0
			AND YEAR(d.[dateDispBeg]) = YEAR(@beginDate)
	) = 0
	BEGIN
		DECLARE @tapGUID UNIQUEIDENTIFIER = NEWID()
		DECLARE @dispanserizationGUID UNIQUEIDENTIFIER = NEWID()
		DECLARE @researchGroupGUID UNIQUEIDENTIFIER = NEWID()

		DECLARE @tapID INT
		DECLARE @dispanserizationID INT

		DECLARE @patientDescription NVARCHAR(128)

		SELECT TOP (1)
			@patientDescription = (p.[NUM] + ', ' + p.[NAME] + ' ' + p.[OT] + ', ' + CAST(YEAR(p.[DATE_BD]) AS VARCHAR(4)) + N' г.р.')
		FROM
			[dbo].[hlt_MKAB] AS p
		WHERE
			p.[MKABID] = @patientID

		INSERT INTO
			[dbo].[hlt_TAP] (
				 [x_Edition]
				,[x_Status]
				,[rf_MKABID]
				,[rf_KATLID]
				,[rf_MKBID]
				,[rf_MKB2ID]
				,[rf_ReasonCareID]
				,[CarePersonSex]
				,[CarePersonAge]
				,[DateTAP]
				,[rf_INVID]
				,[rf_NotWorkDocStatusID]
				,[DateClose]
				,[rf_LPUDoctorID]
				,[rf_LPUDoctor_SID]
				,[UGUID]
				,[rf_SMOID]
				,[S_POL]
				,[N_POL]
				,[IsClosed]
				,[rf_OtherSMOID]
				,[isWorker]
				,[rf_SpecEventCertID]
				,[CareBegin]
				,[CareEnd]
				,[FAMILY]
				,[rf_MKABSheetFinalDSID]
				,[rf_DirectionID]
				,[rf_OutcomeVisitID]
				,[Description]
				,[rf_kl_ProfitTypeID]
				,[rf_kl_DispRegStateID]
				,[rf_kl_DispRegState2ID]
				,[rf_kl_DiseaseTypeID]
				,[rf_kl_DiseaseType2ID]
				,[rf_kl_TraumaTypeID]
				,[rf_kl_SickListReasonID]
				,[rf_kl_VisitResultID]
				,[rf_kl_RegistrationEndReasonID]
				,[rf_kl_RegistrationEndReason2ID]
				,[rf_kl_HealthGroupID]
				,[rf_kl_VisitPlaceID]
				,[rf_kl_ReasonTypeID]
				,[rf_DepartmentID]
				,[rf_kl_SocStatusID]
				,[FlagStatist]
				,[FlagBill]
				,[rf_TypeTAPID]
				,[rf_PolisMKABID]
				,[CreateUserName]
				,[EditUserName]
				,[rf_CreateUserID]
				,[rf_EditUserID]
				,[rf_kl_StatCureResultID]
				,[rf_RegistrPatientID]
				,[rf_DocPRVDID]
				,[DateCreateTAP]
				,[NumInOtherSystem]
				,[rf_mkp_CardGUID]
				,[Flags]
			)
		SELECT
			 0
			,0
			,p.[MKABID]
			,0
			,0
			,0
			,0
			,0
			,0
			,@beginDate
			,0
			,0
			,DATEADD(YEAR, 100, @beginDate)
			,0
			,0
			,@tapGUID
			,0
			,''
			,''
			,0
			,0
			,0
			,0
			,@beginDate
			,@beginDate
			,p.[NUM]
			,0
			,0
			,0
			,''
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,0
			,p.[rf_kl_SocStatusID]
			,0
			,0
			,0
			,0
			,'Infomat'
			,'Infomat'
			,133
			,133
			,0
			,0
			,0
			,GETDATE()
			,''
			,0x0
			,0
		FROM
			[dbo].[hlt_MKAB] AS p
		WHERE
			p.[MKABID] = @patientID

		SET @tapID = CAST(IDENT_CURRENT('[dbo].[hlt_TAP]') AS INT)

		INSERT INTO
			[dbo].[dd_DDForm] (
				 [x_Edition]
				,[x_Status]
				,[rf_DDReestrID]
				,[rf_MKBID]
				,[rf_LPUID]
				,[DateENDDD]
				,[COMMITTED]
				,[rf_LPUMainID]
				,[DDFormGUID]
				,[rf_MKB6ID]
				,[rf_MKBDeathID]
				,[dateDispBeg]
				,[SendedToDD]
				,[rf_DispEndReasonGUID]
				,[rf_DDSendedToGUID]
				,[rf_LPUDoctorGUID]
				,[rf_DDAttachGUID]
				,[rf_DDRRGGUID]
				,[rf_DDNIRRGGUID]
				,[rf_DDTypeGUID]
				,[Flag]
				,[IsClosed]
				,[SS]
				,[S_POL]
				,[N_POL]
				,[DatePolBegin]
				,[DatePolEnd]
				,[Family]
				,[Name]
				,[Patronymic]
				,[W]
				,[DR]
				,[ADDRESS]
				,[SN_DOC]
				,[rf_CitizenGUID]
				,[rf_DDENTGUID]
				,[rf_DDScheduleGUID]
				,[rf_DDNormCostGUID]
				,[MKABGuid]
				,[TAPGuid]
				,[rf_HealthGroupGUID]
				,[rf_OKVEDID]
				,[rf_SMOID]
				,[rf_SocStatusID]
				,[rf_TYPEDOCID]
				,[rf_SportGroupID]
				,[MKABNum]
				,[rf_OKATOID]
				,[DateAccept]
				,[IsBrainBlood]
				,[IsMobileTeam]
				,[IsOtkaz]
				,[IsSeverNarod]
				,[NameMobileTeam]
				,[PhoneNumber]
				,[rf_AddressRegID]
				,[rf_SocialGroupUGUID]
			)
		SELECT
			 0
			,0
			,0
			,0
			,921
			,@endDate
			,0
			,921
			,@dispanserizationGUID
			,0
			,0
			,@beginDate
			,GETDATE()
			,0x0
			,0x0
			,0x0
			,0x0
			,0x0
			,0x0
			,'084C2416-E793-4E61-A472-E2067D80AF2E'
			,0
			,0
			,''
			,''
			,''
			,p.[DatePolBegin]
			,p.[DatePolEnd]
			,p.[NUM]
			,p.[NAME]
			,p.[OT]
			,p.[W]
			,p.[DATE_BD]
			,''
			,''
			,0x0
			,0x0
			,0x0
			,0x0
			,p.[UGUID]
			,t.[UGUID]
			,0x0
			,0
			,0
			,0
			,0
			,0
			,p.[NUM]
			,0
			,'19000101'
			,0
			,0
			,0
			,0
			,''
			,''
			,0
			,0x0
		FROM
			[dbo].[hlt_MKAB] AS p INNER JOIN
			[dbo].[hlt_TAP] AS t ON p.[MKABID] = t.[rf_MKABID]
		WHERE
			p.[MKABID] = @patientID
			AND t.[UGUID] = @tapGUID

		SET @dispanserizationID = CAST(IDENT_CURRENT('[dbo].[dd_DDForm]') AS INT)

		INSERT INTO
			[dbo].[lbr_LaboratoryResearch] (
				 [x_Edition]
				,[x_Status]
				,[Number]
				,[rf_LPUDoctorID]
				,[rf_LaboratoryID]
				,[rf_MKABID]
				,[Pat_Family]
				,[Pat_Name]
				,[Pat_Ot]
				,[DOCT_FIO]
				,[DOCT_PCOD]
				,[Date_Direction]
				,[Flag]
				,[GUID]
				,[FlagUnload]
				,[Pat_Birthday]
				,[Pat_W]
				,[Priority]
				,[rf_DepartmentID]
				,[rf_MedicalHistoryID]
				,[rf_MKBID]
				,[rf_TAPID]
			)
		SELECT
			 0
			,0
			,''
			,0
			,0
			,p.[MKABID]
			,p.[NUM]
			,p.[NAME]
			,p.[OT]
			,''
			,''
			,GETDATE()
			,0
			,@researchGroupGUID
			,0
			,p.[DATE_BD]
			,p.[W]
			,0
			,0
			,0
			,0
			,t.[TAPID]
		FROM
			[dbo].[hlt_MKAB] AS p INNER JOIN
			[dbo].[hlt_TAP] AS t ON p.[MKABID] = t.[rf_MKABID]
		WHERE
			p.[MKABID] = @patientID
			AND t.[UGUID] = @tapGUID

		INSERT INTO
			[dbo].[dd_DDResearchLF] (
				 [x_Edition]
				,[x_Status]
				,[rf_DDFormID]
				,[DateBegin]
				,[DateEnd]
				,[IsParaclinic]
				,[DDResearchLFGUID]
				,[rf_DDServiceGUID]
				,[rf_DDChildFormID]
				,[Value]
				,[rf_CategoryServiceID]
				,[AddInfo]
				,[IsOtherLPU]
				,[IsOtkaz]
				,[OtkazDate]
				,[rf_DDFormGUID]
				,[rf_DDTypeGUID]
				,[StatusRecord]
			)
		SELECT
			 0
			,0
			,@dispanserizationID
			,@beginDate
			,DATEADD(DAY, 1, @beginDate)
			,0
			,NEWID()
			,ds.[UGUID]
			,0
			,''
			,0
			,(ds.[Description] + ': ' + CONVERT(VARCHAR(10), @beginDate, 104) + ' (' + r.[Num] + N' каб.)')
			,0
			,0
			,'19000101'
			,@dispanserizationGUID
			,'084C2416-E793-4E61-A472-E2067D80AF2E'
			,0
		FROM
			[dbo].[dd_DDService] AS ds INNER JOIN
			[dbo].[hlt_HealingRoom] AS r ON ds.[rf_HealingRoomID] = r.[HealingRoomID] INNER JOIN
			[dbo].[lbr_ResearchType] AS rt ON ds.[HLRCode] = rt.[Code]
		WHERE
			ds.[rf_HealingRoomID] > 0
			AND ds.[IsParaclinic] = 1
			AND ds.[rf_ServiceTypeID] = 2

		INSERT INTO
			[dbo].[lbr_Research] (
				 [x_Edition]
				,[x_Status]
				,[rf_LabDoctorID]
				,[LAB_DOCT_FIO]
				,[LAB_DOCT_PCOD]
				,[Date_Complete]
				,[isComplete]
				,[GUID]
				,[Flag]
				,[Number]
				,[rf_LaboratoryResearchGUID]
				,[rf_PrevResearchGUID]
				,[rf_ResearchTypeUGUID]
				,[rf_TAPID]
				,[rf_MedicalHistoryID]
			)
		SELECT
			 0
			,0
			,0
			,''
			,''
			,@beginDate
			,0
			,NEWID()
			,0
			,''
			,@researchGroupGUID
			,0x0
			,rt.[UGUID]
			,@tapID
			,0
		FROM
			[dbo].[dd_DDService] AS ds INNER JOIN
			[dbo].[hlt_HealingRoom] AS r ON ds.[rf_HealingRoomID] = r.[HealingRoomID] INNER JOIN
			[dbo].[lbr_ResearchType] AS rt ON ds.[HLRCode] = rt.[Code]
		WHERE
			ds.[rf_HealingRoomID] > 0
			AND ds.[IsParaclinic] = 1
			AND ds.[rf_ServiceTypeID] = 2

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
			 0
			,0
			,t.[DoctorTimeTableID]
			,@tapID
			,@patientDescription
			,@patientID
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
			,DATEADD(YEAR, 100, @beginDate)
			,'19000101'
			,'19000101'
		FROM
			[dbo].[hlt_DoctorTimeTable] AS t INNER JOIN
			(
				SELECT
					t.[rf_DocPRVDID]
					,MIN(t.[Begin_Time]) AS [Begin_Time]
				FROM
					[dbo].[hlt_DocPRVD] AS dr INNER JOIN
					[dbo].[dd_DDService] AS ds ON dr.[rf_HealingRoomID] = ds.[rf_HealingRoomID] INNER JOIN
					[dbo].[hlt_DoctorTimeTable] AS t ON t.[rf_DocPRVDID] = dr.[DocPRVDID] LEFT JOIN
					[dbo].[hlt_DoctorVisitTable] AS v ON t.[DoctorTimeTableID] = v.[rf_DoctorTimeTableID]
				WHERE
					t.[Date] = @beginDate
					AND t.[FlagAccess] BETWEEN 4 AND 7
					AND v.[DoctorVisitTableID] IS NULL
					AND dr.[InTime] = 1
					AND ds.[rf_HealingRoomID] > 0
					AND ds.[IsParaclinic] = 1
					AND ds.[rf_ServiceTypeID] = 2
				GROUP BY
					t.[rf_DocPRVDID]
			) AS tg ON t.[rf_DocPRVDID] = tg.[rf_DocPRVDID] AND t.[Begin_Time] = tg.[Begin_Time]

		SELECT @dispanserizationID AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Dispanserization: (patientID: ''' + CAST(@patientID AS VARCHAR(10)) + ''', year: ''' + CAST(YEAR(@beginDate) AS VARCHAR(4)) + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO