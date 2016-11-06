SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CriticalAlerts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CriticalAlerts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [nvarchar](50) NULL,
	[Link] [nvarchar](50) NULL,
	[Message] [nvarchar](50) NULL,
	[Originator] [nvarchar](50) NULL,
	[Priority] [nvarchar](50) NULL,
	[Title] [nvarchar](50) NULL,
 CONSTRAINT [PK_dbo.CriticalAlerts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)
END
GO
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersistAlert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		Brian Loesgen
-- Create date: 
-- Description:	Insert critical alert into SQL
-- =============================================
CREATE PROCEDURE [dbo].[PersistAlert] 
	-- Add the parameters for the stored procedure here
           @Link nvarchar(50),
           @Message nvarchar(50),
           @Originator nvarchar(50),
           @Priority nvarchar(50),
           @Title nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

INSERT INTO [EventPoint].[dbo].[CriticalAlerts]
           (
            [Timestamp]
           ,[Link]
           ,[Message]
           ,[Originator]
           ,[Priority]
           ,[Title])
     VALUES
           (CURRENT_TIMESTAMP, 
            @Link,
            @Message,
            @Originator, 
            @Priority, 
            @Title)
END

' 
END
GO

