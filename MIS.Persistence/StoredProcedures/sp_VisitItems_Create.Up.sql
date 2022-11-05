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
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_VisitItems_Create]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_VisitItems_Create]
GO

CREATE PROCEDURE [dbo].[sp_VisitItems_Create]
	 @patientID INT
	,@timeItemID INT
AS
BEGIN
	DECLARE @msg NVARCHAR(128)

	IF (SELECT COUNT(*) FROM [dbo].[VisitItems] AS v WHERE v.[TimeItemID] = @timeItemID) = 0
	BEGIN
		INSERT INTO
			[dbo].[VisitItems] (
				 [PatientID]
				,[TimeItemID]
			)
		VALUES
		(
			 @patientID
			,@timeItemID
		)

		SELECT CAST(IDENT_CURRENT('[dbo].[VisitItems]') AS INT) AS [ID]
	END
	ELSE
	BEGIN
		SET @msg = 'VisitItem: (timeItemID: ''' + CAST(@timeItemID AS NVARCHAR(10)) + ''') already exists'
		RAISERROR(@msg, 16, 1)
	END
END
GO
