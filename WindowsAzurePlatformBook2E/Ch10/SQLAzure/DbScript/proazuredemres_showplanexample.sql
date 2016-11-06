CREATE INDEX INDEX_PricingCalendar_kWh_LocationId ON PricingCalendar_kWh(LocationId);
SELECT * FROM PricingCalendar_kWh;
SET SHOWPLAN_ALL ON
GO
SELECT PricePerkWh FROM PricingCalendar_kWh WHERE LocationId = 95148; 
GO
SET SHOWPLAN_ALL OFF

EXEC AddPricingCalendar_kWhData 5000
