﻿CREATE TABLE [dbo].[tl_TimelineItem] (
    [ItemId] [nvarchar](50) NOT NULL,
    [ClientId] [nvarchar](50) NOT NULL,
    [EventType] [nvarchar](50) NOT NULL,
    [EventName] [nvarchar](50),
    [UserId] [nvarchar](50) NOT NULL,
    [UserType] [nvarchar](10),
    [UserName] [nvarchar](50),
    [Title] [nvarchar](50),
    [Decription] [nvarchar](200),
    [ImageUrl] [nvarchar](200),
    [DetailUrl] [nvarchar](200),
    [LinkUrl] [nvarchar](200),
    [UserUrl] [nvarchar](200),
    [SiteName] [nvarchar](50),
    [SiteUrl] [nvarchar](200),
    [ActionTime] [datetime] NOT NULL,
    [Keywords] [nvarchar](max),
    CONSTRAINT [PK_dbo.tl_TimelineItem] PRIMARY KEY ([ItemId])
)
CREATE TABLE [dbo].[tl_TimelineItemHistory] (
    [ItemId] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_dbo.tl_TimelineItemHistory] PRIMARY KEY ([ItemId])
)
CREATE INDEX [IX_ItemId] ON [dbo].[tl_TimelineItemHistory]([ItemId])
ALTER TABLE [dbo].[tl_TimelineItemHistory] ADD CONSTRAINT [FK_dbo.tl_TimelineItemHistory_dbo.tl_TimelineItem_ItemId] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[tl_TimelineItem] ([ItemId])
CREATE TABLE [dbo].[__MigrationHistory] (
    [MigrationId] [nvarchar](255) NOT NULL,
    [Model] [varbinary](max) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
    CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId])
)
BEGIN TRY
    EXEC sp_MS_marksystemobject 'dbo.__MigrationHistory'
END TRY
BEGIN CATCH
END CATCH
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201301021401032_Init', 0x1F8B0800000000000400D55AC172DB3610BD77A6FFC0E1293984B4D37426F5C8C9D896D57A1AD91D53CED50393908C0908B004E44ADFD6433F29BF9085488220204AA4CD46ED8D5C02EFED2E81C5E34A5FFFFE67F4719552EF09E7827076EA1F0747BE8759CC13C216A7FE52CEDFBCF73F7EF8F187D16592AEBCCFD5B89FD43898C9C4A9FF2865761286227EC42912414AE29C0B3E9741CCD310253C7C7B74F44B787C1C6280F001CBF346B74B26498A3737707BC1598C33B94474CA134C45698727D106D5BB462916198AF1A93F3E9B0433984B09C34144240EC6E7C1E5C4F7CE2841E04E84E9DCF7B2772777024732E76C116548124467EB0CC3F339A20297CE9F64EFBAFA7FF456F91F22C6B80438CE9E15BFAF2383D82E210772ADDCDAC477EA57615D49DC1809637FC7EB86014C7FE43CC3B95CDFE279395FCDBB4A7C2F6CCE0DEDC97AAA354FB902099439BC7CDF9B90154E3E61B6908F3A6B53B4AA2C3FC30AB86304960ACC91F9129E5E2F29450F14EBE1E14EDA0B4A30930720BE7C025EC57918667539247371BF9B1836437E804C2BDA6724FA7888700F91E61991F4BB938E719C93ACA849BD98A130BD90FA2A450B7C97D3EF4E3CC612117A08E64F847D3904AF5AD287E05547EC21B692E23D44BC67B1DA49EA28AEA8C748E2E2BE67F58373F72F9E27A2670C70D92B8651582B89CEFAE2372224CFD7BE778E042EDD03E51434344893A6C003AD065B8FE1DC021D9F5B92A5181E61B9851D52527BB79F39B4A86B7D181602B11292618B921C4D519641EE0D65595ABCA89095176FA2FE8A2E2D30C2586C1176DA5BCD04098772693D056AF07442722161A1A107A456C345923AC3F627BA22DA966F5B3DD6E9AF66A9EBB2AA0B757D337FD5A2B51BAFEAB50D5DE77602E1A62078369163EDE62EA15B024431A2286F91A9179C2E53B64BF2EE42A955A789535BBB231932D28432CC3DB18A32EB6015E6EE5895D633812A5B3F1437B8DADA0FC90DADB676472AE59509539ABA63986AC90432EDDDD16A01D45895DADAC72FAD689A6E6973772CAD514C246DECF7E61C1C6DEC8E536B0813A8B6F643723CD2C6EE38E6296F4299F6EE68F5416F62D5561769145A35D2AECFA153A0FB54F02E85BB5200FDCBB7A4F79D808629E42F4D55F5BC3E2077490B7B8866D712C39212A3F258DFDFB972CEF96288EF417A9E48A2CEF8682D200F811A10447FD2E254AA074C1123732CE48C7FC16C2376DF5B7DAF67F4A4422112FA7F684CB12794C78F2877BE335ED8771A0AD7692B0D0A6C7E837500EEDF141ACA5BBBE7B315F7F859CEFE0B3968746C06C2741B325B8137DFAA2FEBB70C85EBB4538602B6BA2543C15ACD90A160ED5EC740EBC16A650CE5ADDBA94890C472884E45E5E2AB14AD5E0FD47DE8A21AFEFBC7495BB86742F0986C7E196AEFB6DCAB59203584CC416CC8FB49CED34652EE67FCBE539E2E59E2DD726A9FCD55A3C9EDA74C9754928C92187C87F2EB24AF066CE137A1DB8634498E82C0E581B7887338CE08A27522DC574E584C3244B7C5E80AC4AE2B44BD3D0D6D3F19E30CB344C9DD9D697819BB26B156EEBEAC8C42637DEDEFC9B91DA5EEADB9BD9DB942B542B179E0B028CAB2D04CD696D5D5246C5B400E75DBC00E4EE8B1962F4626B7676080CDEA19245B92387439D85110DABA6EEEE2DC5F01F67F023A25B999ECE7F574DD0F2CD830C61F0860EB0AB2A821D4DF09188E1B5B458FB962735EED58CBA36A88756A4C411BC1C18ACE7249E62896F038C6426C3AF99F115D2A819E3EE0E48ADD2C65B69410324E1F6823196AE7EFE2DF34AE9B3E8F6E360A520C1102B8499436B861E74B4213EDF764CBF9D602A14ACAAF18ECC5BB84152BF162AD91AE39EB0854A64F57C2194E330A60E28645E809B7FBB63F87CD8C8DC6042D72948A12A39E0FB7B0FC9274F5E11B5307BAFC02230000, '5.0.0.net45')
