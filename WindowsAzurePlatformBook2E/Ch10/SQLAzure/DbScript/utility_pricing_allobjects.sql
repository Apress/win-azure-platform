--use [master]
--CREATE DATABASE utility_pricing;

use [utility_pricing]


CREATE TABLE [dbo].[PricingLocations](
	[LocationId] [int] NOT NULL PRIMARY KEY CLUSTERED ,
	[Description] [varchar](100) NOT NULL)
	GO
CREATE TABLE [dbo].[PricingCalendar_kWh](
	[PricingId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED ,
	[PricingStartDate] [datetime] NOT NULL,
	[PricingEndDate] [datetime] NOT NULL,
	[PricePerkWh] [float] NOT NULL,
	[LocationId] [int] NOT NULL)
GO

CREATE PROCEDURE [dbo].[InsertPricingLocations]
	@locationId int,
	@description varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  INSERT INTO PricingLocations(LocationId, [Description]) VALUES (@locationId, @description);
   
END
GO
CREATE PROCEDURE [dbo].[InsertPricingCalendar_kWh]
	@pricingStartDate datetime,
	@pricingEndDate datetime,
	@pricePerkWh float,
	@locationId int
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO PricingCalendar_kWh(PricingStartDate, PricingEndDate, PricePerkWh, LocationId)
  VALUES (@pricingStartDate, @pricingEndDate, @pricePerkWh, @locationId);
   
    
END
GO

GO
/****** Object:  ForeignKey [FK_PricingCalendar_kWh_PricingLocations]    Script Date: 08/27/2009 16:36:11 ******/
ALTER TABLE [dbo].[PricingCalendar_kWh]  WITH CHECK ADD  CONSTRAINT [FK_PricingCalendar_kWh_PricingLocations] FOREIGN KEY([LocationId])
REFERENCES [dbo].[PricingLocations] ([LocationId])
GO

CREATE PROCEDURE AddSampleData
@NumRows int
AS
DECLARE @counter int
DECLARE @locationId int
DECLARE @locationIdStr varchar(50)
DECLARE @desc varchar(50)
DECLARE @pricingStartDate datetime
DECLARE @pricingEndDate datetime
DECLARE @pricekWh float



SELECT @counter = 1
WHILE (@counter < @NumRows)
BEGIN

SET @locationId = 10000 + @counter;
SET @locationIdStr = CAST(@locationId as varchar);
SET @desc =  @locationIdStr + '-' + CAST(@counter as nvarchar)+'-description';
SET @pricingStartDate = DATEADD(m, 2, getdate());
SET @pricingEndDate = DATEADD(m, 3, getdate());
SET @pricekWh = CAST(@counter as float)* 0.00063;


   EXEC InsertPricingLocations @locationId, @desc;
   EXEC InsertPricingCalendar_kWh @pricingStartDate, @pricingEndDate, @pricekWh, @locationId;
  

   SELECT @counter = @counter + 1;
   
END
GO
CREATE PROCEDURE DROPSAMPLEDATA
AS
BEGIN
 
 DELETE FROM PricingCalendar_kWh;
 DELETE FROM PricingLocations;

END
