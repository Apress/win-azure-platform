CREATE TABLE [dbo].[PricingLocations](
	[LocationId] [varchar](50) NOT NULL PRIMARY KEY CLUSTERED,
	[Description] [varchar](100) NOT NULL);
	
	GO

CREATE TABLE [dbo].[PricingCalendar_kWh](
	[PricingId] [int] IDENTITY(1,1) NOT NULL  PRIMARY KEY CLUSTERED,
	[PricingStartDate] [datetime] NOT NULL,
	[PricingEndDate] [datetime] NOT NULL,
	[PricePerkWh] [float] NOT NULL,
	[LocationId] [varchar](50) NOT NULL);
GO

CREATE TABLE [dbo].[Gateways](
	[GatewayNumber] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED ,
	[GatewayId] [varchar](50) NOT NULL,
	[LastCommunication] [datetime] NULL,
	[LocationId] [varchar](50) NOT NULL,
	[WebAddress] [varchar](100) NOT NULL);
 
GO
CREATE TABLE [dbo].[EnergyMeterValues](
	[RecordId] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
	[GatewayNumber] [int] NOT NULL,
	[GatewayId] [varchar](50) NOT NULL,
	[kWhValue] [float] NOT NULL,
	[kWhFieldRecordedTime] [datetime] NOT NULL,
	[kWhServerTime] [datetime] NOT NULL,
	[Cost] [money] NOT NULL);
	
	GO
/****** Object:  ForeignKey [FK_EnergyMeterValues_Gateways]    Script Date: 08/26/2009 13:17:35 ******/
ALTER TABLE [dbo].[EnergyMeterValues]  WITH CHECK ADD  CONSTRAINT [FK_EnergyMeterValues_Gateways] FOREIGN KEY([GatewayNumber])
REFERENCES [dbo].[Gateways] ([GatewayNumber])
GO
ALTER TABLE [dbo].[EnergyMeterValues] CHECK CONSTRAINT [FK_EnergyMeterValues_Gateways]
GO
/****** Object:  ForeignKey [FK_Gateways_PricingLocations]    Script Date: 08/26/2009 13:17:35 ******/
ALTER TABLE [dbo].[Gateways]  WITH CHECK ADD  CONSTRAINT [FK_Gateways_PricingLocations] FOREIGN KEY([LocationId])
REFERENCES [dbo].[PricingLocations] ([LocationId])
GO
ALTER TABLE [dbo].[Gateways] CHECK CONSTRAINT [FK_Gateways_PricingLocations]
GO
/****** Object:  ForeignKey [FK_PricingCalendar_kWh_PricingLocations]    Script Date: 08/26/2009 13:17:35 ******/
ALTER TABLE [dbo].[PricingCalendar_kWh]  WITH CHECK ADD  CONSTRAINT [FK_PricingCalendar_kWh_PricingLocations] FOREIGN KEY([LocationId])
REFERENCES [dbo].[PricingLocations] ([LocationId])
GO
ALTER TABLE [dbo].[PricingCalendar_kWh] CHECK CONSTRAINT [FK_PricingCalendar_kWh_PricingLocations]
GO
	
	
	
