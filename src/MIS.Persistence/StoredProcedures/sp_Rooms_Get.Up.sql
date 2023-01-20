-- =============================================
-- Author:		<Vladimir Deryagin>
-- Create date: <2020-11-06>
-- Update date: <2022-10-16>
-- =============================================

IF OBJECT_ID('[dbo].[sp_Rooms_Get]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[sp_Rooms_Get]
GO

CREATE PROCEDURE [dbo].[sp_Rooms_Get]
	@id INT
AS
BEGIN
	SELECT TOP (1)
		 room.[ID]
		,room.[Code]
		,room.[Floor]
	FROM
		[dbo].[Rooms] AS room
	WHERE
		room.[ID] = @id
END
GO
