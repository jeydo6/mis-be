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
USE [MIS]
GO

IF OBJECT_ID('[dbo].[ResourceTypes]', 'U') IS NOT NULL
	DROP TABLE [dbo].[ResourceTypes]
GO

CREATE TABLE [dbo].[ResourceTypes]
(
	[ID] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(64) NOT NULL
)
GO
