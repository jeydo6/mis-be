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
-- Update date: <2022-04-26>
-- =============================================

IF OBJECT_ID('[dbo].[Dispanserizations]', 'U') IS NOT NULL
	DROP TABLE [dbo].[Dispanserizations]
GO

CREATE TABLE [dbo].[Dispanserizations]
(
	[ID] INT IDENTITY  NOT NULL PRIMARY KEY, 
	[BeginDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL, 
	[IsClosed] BIT NOT NULL,
	[PatientID] INT NOT NULL, 

	CONSTRAINT [FK_Dispanserizations_Patients] FOREIGN KEY ([PatientID]) REFERENCES [Patients]([ID])
)
GO
