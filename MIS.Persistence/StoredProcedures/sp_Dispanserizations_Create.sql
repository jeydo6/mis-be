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
-- Update date: <2022-05-09>
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
	DECLARE @msg NVARCHAR(128)

	IF
	(
		SELECT
			COUNT(*)
		FROM
			[dbo].[Dispanserizations] AS d
		WHERE
			d.[PatientID] = @patientID
			AND d.[IsClosed] = 0
			AND YEAR(d.[BeginDate]) = YEAR(@beginDate)
	) = 0
	BEGIN
		DECLARE @dispanserizationTimeItems TABLE
		(
			 [ID] INT
		)

		INSERT INTO
			[dbo].[Dispanserizations] (
				 [BeginDate]
				,[EndDate]
				,[IsClosed]
				,[PatientID]
			)
		VALUES
		(
			 @beginDate
			,@endDate
			,0
			,@patientID
		)

		INSERT INTO
			@dispanserizationTimeItems (
				 [ID]
			)
		SELECT
			 t.[ID]
		FROM
			[dbo].[TimeItems] AS t INNER JOIN
			(
				SELECT
					 t.[ResourceID]
					,MIN(t.[BeginDateTime]) AS [BeginDateTime]
				FROM
					[dbo].[TimeItems] AS t INNER JOIN
					[dbo].[Resources] AS r ON r.[ID] = t.[ResourceID] INNER JOIN
					[dbo].[Employees] AS e ON e.[ID] = r.[EmployeeID] INNER JOIN
					[dbo].[Specialties] AS s ON s.[ID] = e.[SpecialtyID] INNER JOIN
					[dbo].[Rooms] AS rm ON rm.[ID] = r.[RoomID] LEFT JOIN
					[dbo].[VisitItems] AS v ON v.[TimeItemID] = t.[ID]
				WHERE
					t.[Date] = @beginDate
					AND r.[IsActive] = 1
					AND r.[TypeID] = 2
					AND s.[Name] = N'Диспансеризация'
					AND v.[ID] IS NULL
				GROUP BY
					 t.[ResourceID]
			) AS tg ON t.[ResourceID] = tg.[ResourceID] AND t.[BeginDateTime] = tg.[BeginDateTime]

		INSERT INTO
			[dbo].[VisitItems] (
				 [PatientID]
				,[TimeItemID]
			)
		SELECT
			 @patientID
			,t.[ID]
		FROM
			@dispanserizationTimeItems AS t

		SELECT CAST(IDENT_CURRENT('[dbo].[Dispanserizations]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'Dispanserization: (patientID: ''' + CAST(@patientID AS NVARCHAR(10)) + ''', year: ''' + CAST(YEAR(@beginDate) AS NVARCHAR(4)) + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO
