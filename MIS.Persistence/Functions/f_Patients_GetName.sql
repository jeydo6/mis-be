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
-- Create date: <2022-05-04>
-- Update date: <2022-05-04>
-- =============================================
USE [MIS]
GO

IF OBJECT_ID('[dbo].[f_Patients_GetName]', 'FN') IS NOT NULL
	DROP FUNCTION [dbo].[f_Patients_GetName]
GO

CREATE FUNCTION [dbo].[f_Patients_GetName]
(
	 @firstName NVARCHAR(64) = ''
	,@middleName NVARCHAR(64) = ''
)
RETURNS NVARCHAR(256)
AS
BEGIN
	RETURN
	(
		(CASE WHEN LEN(LTRIM(RTRIM(@firstName))) > 0 THEN LTRIM(RTRIM(@firstName)) ELSE '' END) +
		(CASE WHEN LEN(LTRIM(RTRIM(@middleName))) > 0 THEN ' ' + LTRIM(RTRIM(@middleName)) ELSE '' END)
	)
END
GO
