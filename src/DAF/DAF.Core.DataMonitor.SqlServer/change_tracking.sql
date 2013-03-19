﻿ALTER DATABASE database_name SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;

ALTER DATABASE database_name SET CHANGE_TRACKING = ON
	(CHANGE_RETENTION = 2 DAYS, AUTO_CLEANUP = ON);

ALTER TABLE dbo.table_name ENABLE CHANGE_TRACKING
	WITH (TRACK_COLUMNS_UPDATED = OFF);

SELECT CHANGE_TRACKING_CURRENT_VERSION() AS current_version;

SELECT * FROM CHANGETABLE(VERSION table_name, (key_columns), (1)) CT

SELECT * FROM CHANGETABLE(CHANGES table_name, 0) CT;

SELECT CHANGE_TRACKING_MIN_VALID_VERSION(OBJECT_ID('dbo.table_name'));

ALTER TABLE dbo.table_name DISABLE CHANGE_TRACKING;

ALTER DATABASE database_name SET CHANGE_TRACKING = OFF;