﻿CREATE TABLE [dbo].[cms_AppSetting] (
    [SiteId] [nvarchar](50) NOT NULL,
    [Category] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Caption] [nvarchar](50),
    [Value] [nvarchar](2000),
    [ShowOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.cms_AppSetting] PRIMARY KEY ([SiteId], [Category], [Name])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_AppSetting]([SiteId])
CREATE TABLE [dbo].[cms_SubSite] (
    [SiteId] [nvarchar](50) NOT NULL,
    [SiteName] [nvarchar](50) NOT NULL,
    [SubSiteName] [nvarchar](50) NOT NULL,
    [Language] [nvarchar](50),
    [DateTimeFormat] [nvarchar](50),
    [DateFormat] [nvarchar](50),
    [TimeFormat] [nvarchar](50),
    [CurrencyFormat] [nvarchar](50),
    [NumberFormat] [nvarchar](50),
    [TimeZone] [float] NOT NULL,
    [DefaultPageTitle] [nvarchar](50),
    [DefaultMetaKeywords] [nvarchar](500),
    [DefaultMetaDescription] [nvarchar](500),
    CONSTRAINT [PK_dbo.cms_SubSite] PRIMARY KEY ([SiteId])
)
CREATE INDEX [IX_SiteName] ON [dbo].[cms_SubSite]([SiteName])
CREATE TABLE [dbo].[cms_WebSite] (
    [SiteName] [nvarchar](50) NOT NULL,
    [UrlStartWith] [nvarchar](200),
    CONSTRAINT [PK_dbo.cms_WebSite] PRIMARY KEY ([SiteName])
)
CREATE TABLE [dbo].[cms_BasicData] (
    [ItemId] [nvarchar](50) NOT NULL,
    [SiteId] [nvarchar](50) NOT NULL,
    [Category] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Caption] [nvarchar](50),
    [Value] [nvarchar](50),
    [GroupName] [nvarchar](50),
    [ShowOrder] [int] NOT NULL,
    [IsValid] [bit] NOT NULL,
    [ParentId] [nvarchar](50),
    CONSTRAINT [PK_dbo.cms_BasicData] PRIMARY KEY ([ItemId])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_BasicData]([SiteId])
CREATE INDEX [IX_ParentId] ON [dbo].[cms_BasicData]([ParentId])
CREATE TABLE [dbo].[cms_Category] (
    [CategoryId] [nvarchar](50) NOT NULL,
    [SiteId] [nvarchar](50) NOT NULL,
    [Code] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50),
    [CategoryType] [int] NOT NULL,
    [GroupName] [nvarchar](50),
    [ShowOrder] [int] NOT NULL,
    [Status] [int] NOT NULL,
    [ParentId] [nvarchar](50),
    CONSTRAINT [PK_dbo.cms_Category] PRIMARY KEY ([CategoryId])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_Category]([SiteId])
CREATE INDEX [IX_ParentId] ON [dbo].[cms_Category]([ParentId])
CREATE TABLE [dbo].[cms_CategoryContent] (
    [SiteId] [nvarchar](50) NOT NULL,
    [CategoryId] [nvarchar](50) NOT NULL,
    [ContentId] [nvarchar](50) NOT NULL,
    [TopIndex] [int],
    [HotIndex] [int],
    [PublishTime] [datetime] NOT NULL,
    [OnTime] [datetime],
    [OffTime] [datetime],
    CONSTRAINT [PK_dbo.cms_CategoryContent] PRIMARY KEY ([SiteId], [CategoryId], [ContentId])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_CategoryContent]([SiteId])
CREATE INDEX [IX_CategoryId] ON [dbo].[cms_CategoryContent]([CategoryId])
CREATE INDEX [IX_SiteId_ContentId] ON [dbo].[cms_CategoryContent]([SiteId], [ContentId])
CREATE TABLE [dbo].[cms_Content] (
    [SiteId] [nvarchar](50) NOT NULL,
    [ContentId] [nvarchar](50) NOT NULL,
    [ContentType] [int] NOT NULL,
    [Title] [nvarchar](100),
    [Keywords] [nvarchar](200),
    [Description] [nvarchar](500),
    [ImageUrl] [nvarchar](200),
    [ContentUrl] [nvarchar](200),
    [LinkUrl] [nvarchar](200),
    [ShortUrl] [nvarchar](200),
    [PlainBody] [nvarchar](max),
    [HtmlBody] [nvarchar](max),
    [Properties] [nvarchar](max),
    [ContentSize] [float],
    [Published] [bit] NOT NULL,
    [ReadCount] [int] NOT NULL,
    [CreateAsRelated] [bit] NOT NULL,
    [CreatorId] [nvarchar](50),
    [CreatorName] [nvarchar](50),
    [CreateTime] [datetime],
    [ModifierId] [nvarchar](50),
    [ModifierName] [nvarchar](50),
    [ModifiedTime] [datetime],
    [PublisherId] [nvarchar](50),
    [PublisherName] [nvarchar](50),
    [PublishTime] [datetime],
    [ShowOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.cms_Content] PRIMARY KEY ([SiteId], [ContentId])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_Content]([SiteId])
CREATE TABLE [dbo].[cms_ContentRelation] (
    [RelationId] [nvarchar](50) NOT NULL,
    [SiteId] [nvarchar](50) NOT NULL,
    [ContentId] [nvarchar](50) NOT NULL,
    [RelatedContentId] [nvarchar](50) NOT NULL,
    [RelationType] [int] NOT NULL,
    CONSTRAINT [PK_dbo.cms_ContentRelation] PRIMARY KEY ([RelationId])
)
CREATE INDEX [IX_SiteId_ContentId] ON [dbo].[cms_ContentRelation]([SiteId], [ContentId])
CREATE INDEX [IX_SiteId_RelatedContentId] ON [dbo].[cms_ContentRelation]([SiteId], [RelatedContentId])
CREATE TABLE [dbo].[cms_CategoryUserGroup] (
    [CategoryId] [nvarchar](50) NOT NULL,
    [UserGroupId] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_dbo.cms_CategoryUserGroup] PRIMARY KEY ([CategoryId], [UserGroupId])
)
CREATE INDEX [IX_CategoryId] ON [dbo].[cms_CategoryUserGroup]([CategoryId])
CREATE INDEX [IX_UserGroupId] ON [dbo].[cms_CategoryUserGroup]([UserGroupId])
CREATE TABLE [dbo].[cms_UserGroup] (
    [UserGroupId] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [SiteId] [nvarchar](50) NOT NULL,
    [ShowOrder] [int] NOT NULL,
    [ParentId] [nvarchar](50),
    CONSTRAINT [PK_dbo.cms_UserGroup] PRIMARY KEY ([UserGroupId])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_UserGroup]([SiteId])
CREATE TABLE [dbo].[cms_UserGroupUser] (
    [UserGroupId] [nvarchar](50) NOT NULL,
    [UserId] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_dbo.cms_UserGroupUser] PRIMARY KEY ([UserGroupId], [UserId])
)
CREATE INDEX [IX_UserGroupId] ON [dbo].[cms_UserGroupUser]([UserGroupId])
CREATE TABLE [dbo].[cms_PageTemplate] (
    [SiteId] [nvarchar](50) NOT NULL,
    [TemplateName] [nvarchar](50) NOT NULL,
    [TemplatePath] [nvarchar](200),
    [AllowContentTypes] [nvarchar](500),
    CONSTRAINT [PK_dbo.cms_PageTemplate] PRIMARY KEY ([SiteId], [TemplateName])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_PageTemplate]([SiteId])
CREATE TABLE [dbo].[cms_PageTemplateControl] (
    [TemplateControlId] [nvarchar](50) NOT NULL,
    [SiteId] [nvarchar](50) NOT NULL,
    [TemplateName] [nvarchar](50) NOT NULL,
    [Section] [nvarchar](50) NOT NULL,
    [ControlPath] [nvarchar](200) NOT NULL,
    [ControlParas] [nvarchar](max),
    [Container] [nvarchar](50),
    [ShowOrder] [int] NOT NULL,
    [Cached] [bit] NOT NULL,
    [CacheMunites] [int],
    CONSTRAINT [PK_dbo.cms_PageTemplateControl] PRIMARY KEY ([TemplateControlId])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_PageTemplateControl]([SiteId])
CREATE INDEX [IX_SiteId_TemplateName] ON [dbo].[cms_PageTemplateControl]([SiteId], [TemplateName])
CREATE TABLE [dbo].[cms_WebPage] (
    [PageId] [nvarchar](50) NOT NULL,
    [SiteId] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [TemplateName] [nvarchar](50) NOT NULL,
    [CategoryId] [nvarchar](50),
    [ShortUrl] [nvarchar](200),
    [HtmlUrl] [nvarchar](200),
    [MetaKeywords] [nvarchar](200),
    [MetaDescription] [nvarchar](200),
    [PageTitle] [nvarchar](50),
    [HeaderTitle] [nvarchar](50),
    [Status] [int] NOT NULL,
    [ParentPageId] [nvarchar](50),
    CONSTRAINT [PK_dbo.cms_WebPage] PRIMARY KEY ([PageId])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_WebPage]([SiteId])
CREATE INDEX [IX_SiteId_TemplateName] ON [dbo].[cms_WebPage]([SiteId], [TemplateName])
CREATE TABLE [dbo].[cms_WebPageControl] (
    [ControlId] [nvarchar](50) NOT NULL,
    [PageId] [nvarchar](50) NOT NULL,
    [Section] [nvarchar](50) NOT NULL,
    [ControlPath] [nvarchar](200) NOT NULL,
    [ControlParas] [nvarchar](max),
    [Container] [nvarchar](50),
    [ShowOrder] [int] NOT NULL,
    [Cached] [bit] NOT NULL,
    [CacheMunites] [int],
    CONSTRAINT [PK_dbo.cms_WebPageControl] PRIMARY KEY ([ControlId])
)
CREATE INDEX [IX_PageId] ON [dbo].[cms_WebPageControl]([PageId])
CREATE TABLE [dbo].[cms_MenuGroup] (
    [SiteId] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Caption] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_dbo.cms_MenuGroup] PRIMARY KEY ([SiteId], [Name])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_MenuGroup]([SiteId])
CREATE TABLE [dbo].[cms_MenuItem] (
    [SiteId] [nvarchar](50) NOT NULL,
    [MenuGroupName] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Caption] [nvarchar](50) NOT NULL,
    [Icon] [nvarchar](200),
    [Shortcut] [nvarchar](50),
    [Tooltip] [nvarchar](50),
    [LinkUrl] [nvarchar](200),
    [ProtectedUri] [nvarchar](200),
    [Target] [nvarchar](50),
    [ItemType] [int] NOT NULL,
    [ParentName] [nvarchar](50),
    [ShowOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.cms_MenuItem] PRIMARY KEY ([SiteId], [MenuGroupName], [Name])
)
CREATE INDEX [IX_SiteId] ON [dbo].[cms_MenuItem]([SiteId])
CREATE INDEX [IX_SiteId_MenuGroupName] ON [dbo].[cms_MenuItem]([SiteId], [MenuGroupName])
CREATE INDEX [IX_SiteId_MenuGroupName_ParentName] ON [dbo].[cms_MenuItem]([SiteId], [MenuGroupName], [ParentName])
ALTER TABLE [dbo].[cms_AppSetting] ADD CONSTRAINT [FK_dbo.cms_AppSetting_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_SubSite] ADD CONSTRAINT [FK_dbo.cms_SubSite_dbo.cms_WebSite_SiteName] FOREIGN KEY ([SiteName]) REFERENCES [dbo].[cms_WebSite] ([SiteName]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_BasicData] ADD CONSTRAINT [FK_dbo.cms_BasicData_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_BasicData] ADD CONSTRAINT [FK_dbo.cms_BasicData_dbo.cms_BasicData_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[cms_BasicData] ([ItemId])
ALTER TABLE [dbo].[cms_Category] ADD CONSTRAINT [FK_dbo.cms_Category_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_Category] ADD CONSTRAINT [FK_dbo.cms_Category_dbo.cms_Category_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[cms_Category] ([CategoryId])
ALTER TABLE [dbo].[cms_CategoryContent] ADD CONSTRAINT [FK_dbo.cms_CategoryContent_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_CategoryContent] ADD CONSTRAINT [FK_dbo.cms_CategoryContent_dbo.cms_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[cms_Category] ([CategoryId])
ALTER TABLE [dbo].[cms_CategoryContent] ADD CONSTRAINT [FK_dbo.cms_CategoryContent_dbo.cms_Content_SiteId_ContentId] FOREIGN KEY ([SiteId], [ContentId]) REFERENCES [dbo].[cms_Content] ([SiteId], [ContentId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_Content] ADD CONSTRAINT [FK_dbo.cms_Content_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId])
ALTER TABLE [dbo].[cms_ContentRelation] ADD CONSTRAINT [FK_dbo.cms_ContentRelation_dbo.cms_Content_SiteId_ContentId] FOREIGN KEY ([SiteId], [ContentId]) REFERENCES [dbo].[cms_Content] ([SiteId], [ContentId])
ALTER TABLE [dbo].[cms_ContentRelation] ADD CONSTRAINT [FK_dbo.cms_ContentRelation_dbo.cms_Content_SiteId_RelatedContentId] FOREIGN KEY ([SiteId], [RelatedContentId]) REFERENCES [dbo].[cms_Content] ([SiteId], [ContentId])
ALTER TABLE [dbo].[cms_CategoryUserGroup] ADD CONSTRAINT [FK_dbo.cms_CategoryUserGroup_dbo.cms_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[cms_Category] ([CategoryId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_CategoryUserGroup] ADD CONSTRAINT [FK_dbo.cms_CategoryUserGroup_dbo.cms_UserGroup_UserGroupId] FOREIGN KEY ([UserGroupId]) REFERENCES [dbo].[cms_UserGroup] ([UserGroupId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_UserGroup] ADD CONSTRAINT [FK_dbo.cms_UserGroup_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId])
ALTER TABLE [dbo].[cms_UserGroupUser] ADD CONSTRAINT [FK_dbo.cms_UserGroupUser_dbo.cms_UserGroup_UserGroupId] FOREIGN KEY ([UserGroupId]) REFERENCES [dbo].[cms_UserGroup] ([UserGroupId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_PageTemplate] ADD CONSTRAINT [FK_dbo.cms_PageTemplate_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_PageTemplateControl] ADD CONSTRAINT [FK_dbo.cms_PageTemplateControl_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_PageTemplateControl] ADD CONSTRAINT [FK_dbo.cms_PageTemplateControl_dbo.cms_PageTemplate_SiteId_TemplateName] FOREIGN KEY ([SiteId], [TemplateName]) REFERENCES [dbo].[cms_PageTemplate] ([SiteId], [TemplateName])
ALTER TABLE [dbo].[cms_WebPage] ADD CONSTRAINT [FK_dbo.cms_WebPage_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_WebPage] ADD CONSTRAINT [FK_dbo.cms_WebPage_dbo.cms_PageTemplate_SiteId_TemplateName] FOREIGN KEY ([SiteId], [TemplateName]) REFERENCES [dbo].[cms_PageTemplate] ([SiteId], [TemplateName])
ALTER TABLE [dbo].[cms_WebPageControl] ADD CONSTRAINT [FK_dbo.cms_WebPageControl_dbo.cms_WebPage_PageId] FOREIGN KEY ([PageId]) REFERENCES [dbo].[cms_WebPage] ([PageId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_MenuGroup] ADD CONSTRAINT [FK_dbo.cms_MenuGroup_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_MenuItem] ADD CONSTRAINT [FK_dbo.cms_MenuItem_dbo.cms_SubSite_SiteId] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[cms_SubSite] ([SiteId]) ON DELETE CASCADE
ALTER TABLE [dbo].[cms_MenuItem] ADD CONSTRAINT [FK_dbo.cms_MenuItem_dbo.cms_MenuGroup_SiteId_MenuGroupName] FOREIGN KEY ([SiteId], [MenuGroupName]) REFERENCES [dbo].[cms_MenuGroup] ([SiteId], [Name])
ALTER TABLE [dbo].[cms_MenuItem] ADD CONSTRAINT [FK_dbo.cms_MenuItem_dbo.cms_MenuItem_SiteId_MenuGroupName_ParentName] FOREIGN KEY ([SiteId], [MenuGroupName], [ParentName]) REFERENCES [dbo].[cms_MenuItem] ([SiteId], [MenuGroupName], [Name])
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
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201301291437463_Init', 0x1F8B0800000000000400ED3DCB6EE43892F705E61F1279DA19609C7675F762BA61CFC06597A78D2E970B9555DDC05C0A72266D0BAD9472256557797E6D0FFB49FB0BABB7F888E04BD4CBE58BE114C9604430180C9211C1FFFB9FFF3DFDC7D75DB0F883C4891F8567CB93A3E3E582849B68EB870F67CB437AFFD7BF2DFFF1F73FFDC7E99BEDEEEBE2D7BADE7779BDAC65989C2D1FD374FFD36A956C1EC9CE4B8E76FE268E92E83E3DDA44BB95B78D56AF8E8F7F5C9D9CAC48066299C15A2C4E3F1CC2D4DF91E247F6F3220A37649F1EBCE026DA9220A9BE6725EB02EAE29DB723C9DEDB90B3E5E5F9D5D1C5CDFAE8F2F5D19BABE5E23CF0BD0C893509EE978BFDF73F7D4AC83A8DA3F061BDF752DF0B3E3EED49567EEF0509A950FE69FFBD2ED6C7AF72AC575E184669062E0AADA85E36F46414BDC9284F9F72B40AAACE96E7FBFD9AA469C670BA5E56F317F2C47CC83EBD8FA33D89D3A70FE4BE6ABDF65372BD5D2E56EAAA175E4A1EA2F849AB72FE97AF78BAE2516A1A72D8E4E465BFD238A76A71E57F25DBB7247C481F9B91B8F1BED65F7EC864E953E8674297B549E34356FAEE1004DE5D409AEA2B69B72D6503775C7269706AF7A528BAEBB7FC2DEFF6572F3898129B4D82AEDDAE1FA32FB7F196C475D7D761FADD2B35D3DE797FF80FC59C056474B9F84082A23079F4F7A5FA385A1FEEF2B2CFF99F6A4626198571B4FB10E5FD40E59F3F7AF1034933DC2249A5757488371C8AA7AB561348F54305D19D7298EC34CEBB1D6546552C1EA5EFB75EF870F01E9C76AC33AF2E339DF9315B83AFA278E7A563F43E4ECFE3D17C7188E3CCB67A1AA7F77787DD1D89C7E3F9BFA2B091F1CBE890B5359E2A97E4DE3B04E9FB6CB67CF4D360F82953F67F43522FD3A05FA2789B18A3E010874B926C62DFCE163046035D4E6FBF842456AEA9542D71416D0A9B85925F4DDB1AF57A6BB2DAB78BB9AB551FC313340D74517DED25FE26538CDE754A7630B24C95CF3C3FC552C13C01AA40C6890CCDCAD8F6098C626D8B0BD8310502626CA9314E5198923045302A0B4584A8EF223E74A1293A858622BB7D860AC225BA86809850286027D6B0411146ED377297170A58D1DF058498C24EE66E06A98BB96BBF6F1DC5FAFB1407EBD48BD3DFFCF4D17C6F65A4C4B54780D11136E390B7B3D976D4ED46D876BC1C5ABC1C5A38EBF49F7174D8BBE6729F6725FC3C4C32B6F9CD8C781D652ADE0B8DC1BCF7B2ED4EEA766699D9A6A8596A6C47F1069FC4D4D25F8173F668A05757C4102CCB152856954C91BC78F4836DD6D21D9A728B94A3C56AFD6A14ABC5D255B7B559BEE8B6DFC81296359DFDF2A57574530D6DDE6BD337BDDB290B0C2999FB3A9159AEE92161F8914FE4FAF3F3592E34F7B4BC068677BC0ED68706B0A873B9221CA73E160403BCD0FDBF6405B03F01A880D79BFAD6F41611E4EBA0980A154D51FE9490B8500172A49B6A52B4C55A28E2405527EB6DC590816EB175AB9748CDE8166C44338262D6C03D7F8CF6D7E1967C4517249D65EDE728ED0EE4FDE12EF093C7FCCAA2B9AEA8AECA8CA9BA0D55607430BABDBFB70263BFCEA127A64039BAC280C7AB8687CC4FEED5B90A5D417D1AAE40D22368FAEC5C3C886E4BD1E368AA4A379D3D8CAE9E9FF21D4D03563D8B3B0CFABBA956B5B8253DE97C456979376A7A9E0DDD8D0E7821CA9F99EDBC07F2290E0627BA928F31BA7EEB87BF8FD16FB67D8D4721F87DE0F9E1EB686B7A669FFDDBB1E79FD35D304AC7D5CF62C51AB8EB4AB2D7FEBF5BDB47701C3130E948E713ED0FC4DB5E448730452D4C3D551F936C193F4F0A43A13B5605B8281EE0E404EE789403B982852EACEB9B68EBDFFB6404F6D53D8FC1BFAAEFAD0B0ED6D36B0416365D8FC143CD9DE2B49C9CB59C61843D9283AD1CEA2F64BA35C29003764FBA28568A58CB8B48A82B22CB554131E6EB41689BEEE86ACC6D7676755B9BFD1ADDF65BB9FC1A6BABC8CACD580864838D6D56D9726B25A671A252977C6EEA0AD391AF824D47A15E372DA28535DF04479EADA9A481ABDE4DB3F0F7066E2FD6C1EA4D5F33BB8867F0EEBBEF6EA7A7D6F74AD809AAE40ACAF84A0CC45CE8803316A072E12415ACD4E92CB5D3BCE824E8834ADB14BC08C70A4C73E32F31610F877656F01B03B64498FC5CB143278716B2E84DC09749D0EAC3CFC108355C0159BA3AE400E03D4A033AFFE733A54C01F4D82A38965C3DC7D122562A1D1F6CC51ED05CA5E7FF3856EB68FDB9AD0035CEA399399AD682F96440050C9934563246C7F0F47C0F5B77631F1C33864B0883F4489DBFF77A8FCB113B3F0F82E80B75EF3B4094A9B9C56018C1C64F293CC6CDE474208E02757C5D55F17333D91044F97AD2A83BA172A72D0400D9462570206C161400C4B761E38FAA6ED664E33A3A4AFB0C331B65E75ACEACF3D81BE772D9F3C3764B3733DFFB0B6FE3E042BB80727308FDB45D64F4BC26BB2D18B5F694AD1B741DE9F2C154345D45DA45A1AF5544077561C9B15A45AA206C9B95236F67B35CD4EDBE8D356254537494CEFB394A9FB49357EE6A3546BF1DD2CAB8E9DCDE7DD28167DD58397D7E265EB6168FD2751FE173EEF5B1A3D55F2BAB08BF548229471C1C2DD770C5D35BB60445A88F43656DA4B0DC2B96C7C9520BA8860D593D7C19CA2DA97563BDC5AFA0D7265461F988D8D1C518E7983A5D93D75000ADEEC9EDB7EEE36DD9C7B2025F76CD2FBBE6E7B16B2EB5971B2D87A86150135A69B97C39BE21E1C1DAE9C1E0EA62565716236DD19C675672E56CCC088A60FA89A5625E45B18AA90D9137C7132AD61D14C9675A9906B0646B88799FE16ADD323F53307B9E660DC6D07C9BFFC4E4C87B510BFDF57BBD19E1F8A0382CDA1C864F6C9C5910A9BF1FBADBB1022FB39F69667393EDA7D81FBCF346E30ECAEA5CF70A6EEEB552B60ACA2EB7E9AE75C2B4A28C98D5105BF7F1EC7A620D9B551F770DB25EF5A5888AC68183A32AA607F16808289623D9C7C995058E52F3C92A09E06107842C9413F43AB90ABC87A49E249FC26C0A044FD994A36701CB881B9267CBAFA0FDEAC7F9DB44CB45914DF46C7922F08DA97E1105875DD8D47E25AF9D53DDD4FD5EA4B6A44B422B7D66DC505A4D646352DFE5EF0368539AC743DF86C1932EAD6FA3CDEFF9F6BBAAFD9DBCF62509484A55FFF1470BEE30E9333AB327BF1B6AF03996A35F6484D0E5E4951F106D2E66ABBF2E0F3F92AF29265D7CDDF3C3D68F9ACA3FC82BFFEA6F495BF9BFE4956FE38796110AB6BD277112B5B3E74439D9C2D4DBB4349EBCB21712366CADB3B09CA719668FBB421BC252A0851C6B6C74C68AD1370AE6BE3EA469A4ADC94CE472BD0F328D1F9B29BEF32489367E31443518E17909B6CF37E176A17C6BA2B5EBEAA796163787DCAC0FFC4DB6D49C2DFF22D02283CBD9A9D5211E04F784C536837B1B964A6F715E9C6CE70B59B2F1B6C0463BC380FD92EDCC49BE6CFA5E90897392C69E1FA6E236DE0F37FEDE0B54D8730DD1E305E4B860D574C4975C923DC944364C55E3D21583A6238E712A3E9DAE2831D3933EE6E91095A0C0EF882865D0A9AC28B00224987AA45039393A4922C41E5D5100CFBACC4511E245371406904520A33936E6B2F4E6ED88B3AF40186844D92B34C38AB99520E2E80F2486F8F0CC4C08AB9DA09E9CF07B57E782C86F3C353B383E3A128D240782C5A2A333B2D7E0AB2A1D448B65B90E0A6D00EB28E2C5A6E1C6861DC9C94D25A968C2DDF5650979BC6A06FA0CC47C2055060EC5E4B5189FC55C2913B8EEEA246AB8C6C2C0BA535608123A6327CDF461213BB355534CAA2DD568C3F9B7C4216F52C5980B14FCDEDD8C541844C0C09A0C1AA6D92834215FB6AEC488A95F7A114CF1E502B5CA9B9E74F2548CA033B171EB8ECA10D2AAA53655EAD2421A67AB1E47548BB3548762C649954C40C94794C2D6D3919DECB5800E6AB993E889FCE92205B05EC2DE12B012589179C3223C9C980B1912157285A74B44D33C5A685934D9E2C0D3AA8BD463344C5BF4B1F17DF6F2CFE5DAD415502CF1662FB30149DB39C74901933293B9010FBA73E4C544BEA31A447C6A6B950582E6B9568A2B7850A5EE009F0F9A93CF62D30CA44955ED67653953450B8DCA4C66BE75963DFB37A7CD334EC708DB677CFC66B181E6727962228425F66CC5C64A329184A073D848C3A80FB495868763F29B692115AA5A2EF0EB1254E014EB059E735503B6A31B130C079D1194670BB59121F34B13050E438A1297B05339EA58F64E60F08B7CAE36DA0C4B97AB256093516C3015A388283C683393549D3344790AE3A1E5478197C47834B0063A2A50BBD344F74AD4EE90707CF11453B3623220C9D3DA0A0093A8D84077E2295EE7600CA2D80F640FA243337993104DF4A8232870D6475818EB343B963209E68D9C9B6842448C20A1D0B0CD5250918CEC7209123360F52AB062826A3D653D4DC9E5A9717ECA2ACB7EDF59E0F9A11F1CFB01E60993BB0F135438911F130355262FD2977D30FDDF1C143484F8404A191A86C92B622EF5A14A1EF0032444C814C747587E45255C47474770FF3AA306A76DB612181B375BACF3014546B95EE3592A9DE8A6F9AEC51805D35D7FB1A17CC66B2E93F74F218B70124041CA2D8C50692E550D2D39115187F01F56CB42433479552B26E1C3839C85AA6088335DCB28D85E043F27B310477F20E3101F9EC99B88424A28A50C4AA39BE94A361238CFD86614FB81E56F7E91CD0CE658EE5B444C80F4618E0511484BAAA96D2726930221CE4DBBEE094BA443EB1C5F5562D6C1855FB14707EAF626F6F87E5D06DE5D90AC0499A1E5600059373F20E89D403A95E60033E34D917AF0A2CE815FFBFBDDAC2F5F0BF321AFB926699D0AAC499F932C176F9A0C8640721D41F259406FA38D17909CB110A0C6F05040A9F25141209A54550A104C2A090810975543018E7EFD5900D5BAAFEA41A9839E64B01AC7641548092833104DE24C1C54EBC5AC4968E36B20239572DC50809582B30053BD548E832ADDB114E0E86334081A7BCC6600ECA2795E460AB3393051CFAABC1932ABCA93113D1012C4F8431C05C046AB82EA82B50C354061939D5D6F394094366DA18999FB16543D569F61E9FD58CB004DF0D710C2A94F61C59001AB8D0C0A58AB4579CB98A5D8801BF99F76A1C019C2D45393415777C116061EC01966B9EBCC1C20B519C01A5502348610490A348A0C7E8D93704692F44C8FCF9DF952E75D567106B2DF25A47026BC0BEE7066BB36480B16B119A300DE48524A3114C049A528D4690346C209388F546F22C2A732927140291858BEA30E5CC0450187D4810D4C6A09092FF01414201960320A9116CA2CD5E00D988EA27741E13387E870098E8B931225C4C5B9E2961005D79B48A94449538454A2A3C5846145454C9F21A11F73900709001CE46D7901F8B41BC8983D5B84740B386FE4991920AAD0DC0C2297A8DDA69A5B6836060DEE3B60151799AFC131592CBF9440249ADF15FF90F8FD5ED928846D4B66A334C21B9C47588CB7EDBCC442BA0D46A0C3F20604734B163855E837B8124982BF0105449FB8682C739270EFBE163A2E9A1860972CDE98A1028938A630D764071263DCDB922704B94AB9A034A4D150D82E9CC04D691C54175670419A328EC8E239616A90884E88A8EA8C518747480C67EFAC52984AEA684264C8E5E692B104E9994C4E592446AA01FC5184B331D4E0016D1429DC81B2843978085B6FBA068D9E5270068FB442290263AD103EB547D29AEC02A3AB06E55A7B27A0C739D8955849A0E058EC9A83828BB1AE285BF09109F500F8868782306480C120ECD179755522E10518FED19B0471310812DA95EB3B12A7604B3FBEB263603A902F9B34729F7B1077D9E43062C2089380F1BDC6D981BB684384804EDA2253B49485CC2DBB1F29015C81A15B2AB196E44E49A80CB185BEC7945D5089C07A571CA267AA84253A9753B8032BC710F5C50BEEAE3A0C3BA8DB65054F109F4A9C1AD1ABB21B77441F4A2DF1EBCA217CB9517ADDE1B4E00B8F0563F0E5070526614AFD806BE345D5949DAED69B47B2F3AA0FA7ABACCA86ECF397576FA22D0992BAE0C6DBEFF3ABE5B665F565B1DE7B9BFCB4E4AFEBE5E2EB2E0893B3E5639AEE7F5AAD92027472B4F337719444F7E9D126DAADBC6DB47A757CFCE3EAE464B52B61AC36CCDE8BF7F96A7A4AA338D3A95C697E76B525577E9CA4F92DE69D97BFD078B1DD89D5643E63751FA2EB983862B51F46DD26FFBF6C77797E7594757374F9FAE8CD15ED62C60169D977955194BF56591047A8719635CE9AAF33F5E1C5C0D3D0B9EF5FF918AECA1F1083D29ED2D170307F3019A4D249908682F919E2B8EC4BDF2C16953DE4B0258353BC78C942A93EE9C3A05EF26618DC7E06FC2757DC38F3D2B412C4899BD4BC846AC92FBDBAD8CB6FEDD9682EBC68CB7E25376F21CA5CFBD500524900008C2ED087F7D60B1F0E85494B036BBFEA43CA741CF9E8EFC855FE6874CAC2E3CBCCA06210CDA161F8D9E1767188B3C570F30441E4CB0C74D3217F191782C9969851FDAF282422CDE55783D120F7DE21488B2D969F061C44B1D418F20D49BD5FC8D39728DE262070B68215FC4B926C621F50DF589DC968CFC6C7D05E75D61EDDE6AA136DD9BFD2FB1407EBD48BD3DFFCF49185C6964C66A038D735FBE162FDE6CD074DD11E63F875F5BA24CDEAFA9BD9AAF762F5F565F551E131341C69D48C4B0B12959D2423C5E785A7FEA80FA77DB78F0684BFE637DA6497DED3E94E74CCA740638EE34D55538A67AE2C59B94478DC4CF368CB4972F965D8E95DD29F0F12CC99B264FE93345B34D3036761D5DF9EEF147D92B83A994E54C4DBCC60BEA21086594FDD4CFE8BF6B51276EEA28F98E0B03E46FBEB704BBE72FB94E6AB3EA49FA31480D47E3510F1C35DE0278FF966899372BA401FDE6D2882AABF1940B9BF07C0D41FA733ED1C4C37FB6936D2F4723821AA46C0724417981C04087B76E38D3ABC3BB7DB92A3FB70E9E61B353C77DE03C936849CE5D97C35E6BB008BFE6E70C4E687BF0BA09A8F46EB7F2CE2D47E35506A81E787AFA32DB7A7A23E1BA8DA741788A0DAAF0658953F0BBF38062DEABBF118AEFD7FC373A72C305E08086FECB49FF5617D20DEF6223A84DCF91EF5D980CE98640BF77952B95273B4F2858670A358D063ED676358A21DCC1498D22CAE80F4777D6837D1D6BFF789402AFDDD1C9A482C5B620C712BD2CB9698CBB240325360014F249A2B1ACFF09AD1259910F2D0D97C6A5249589B5138045CC9952D7819A3BF0F7FC8E0CE38139F6B14A8943EE6A8E69C68F7B125D31158D11BBCFB061B0D1130D8624B600CB33D669E7F612E2E64EFC28C36904E06B0CBC0590C982D8BFB3B4674E433E0F0B86F3607755C208F03212C920A751044B8FD50C2983783404D6AD0588F6BFB31635237990F99BC79BF9395CD11CE1CB048B387AB21BEF7F83B6FB6441FE27910445FA893236E9B0D144F52C21A6778378256E7CEEA266F2814D5F0560D7909048A875E7FDC8BF49A6CC4C3BEE6A399159F31459C194C8105BCD84B1080458919C4C2B95704577D1EC70EB8F036C20956FDCD10CACD212C630404584DC964D44713F362AF32EA9C7DE66A026D891B6B0FC2F4C552FE4BC4C68912E86E0CBB57238EBD179C9DAAE767DE02A0E6A3C1791FEA0369EBFC28F57AD4707794492A70A764E500FA33F132650640630AC6F1708067245D323555E7C048E2728B5A2B3E63D3083189AC4C2147BAF4C57079315CC698CD54A4A1FD446613FB9ACF6345FBA91B20A63EBB230E75576F7126F1B2FD409BF98ABB19672EE13F6BA028DE02988EEC6070AE373C90F28BA199BA39A480995A7C35F1B28BF207307827BBEAE3F06E2DD9CF345B4AC9F653EC0B8E1F548901855570324360F5CD60CCB25920DEC0B55F4D8D48511A650F564825617217D96C8037789B2D262654DC598B0D0CAFA6F31076F8FE17CB6328F24D5BE3D15957910D81C58DF467AA9887583CB95443B3271D7DA2C6D9A5BE14D10C8FAD9FB75C5C27EF0E4170B6BCF7820476D942996B0605E5299FA8401475215F015FA59968D597E67793AFA0CA15C0243128B89DA72428B89C54790BF8E4016595E52263E51FFE364F1CB07E4A3275749457385AFF777011F8B9C36B53E1C60BFD7B92A41FA3DF4978B67C757CFCB7E5E23CF0BDA44C2A619E16816C77AB24D9329B39CA3EA92F1A900C01A7BF1061FC6BB9D078D94DAC8A078169BE7F74BAE2513A45E67AF9BC45F887176F1EBD4CC7DE785FDF92F0217D3C5BFE70BC5CE442E7DDE5D9322AC15B49A1B688BB855BD2E81AD7CADCD1069BC662241B0FB50A7793C0CC64CE182AB5089690FD7C3E4809A6973EA95483A903BA88F458C2D746E03A864B6720700BBACD47E05408F9B404CE81F702B8378CF99C054E81B3C90B9C33A4CC645082BD0F224F35ED81F112B217B81507287F81B4872E5D3007FA5D7AD1568F607A007DF568BF3AF7A16FD8B402F245CA11FF2491FA7A5C84E3F3D53CACDBB95F645EECA631EC267398D4E19A53B816D618209E75E68012C49D6F0EA2F573EC409EF64486C3F1F5E6307E85AD9EC774DB59CCE522BC7FD479AC65143191FFF6723CE949565F8CDB4318678E8131BEBD1D33E856C7CF97C63D6DE8630E37A4BA05DC06FD8322A93331DA687F6B104CA85909659BF1322D7E1B525407F9E36074306A62FC4DC0E84FAB61A6D3E4E6475F52CC84E9DBEB56F576F4C47CB3A8B509556E6FA04DA8A39D27600A36A1FC6E51A6C3FADD426EAE43DD826DBD13DDC2A51200B080FF73E77DFDB3B1026E7200380056FD2C92003800C72401404E8D0C5608D2697F42C5FDDB2B0921D4DF1E1F2AC6DFAD294D47FBBB874C5C2CB074D0BF5314D9E8FF3E406F5DD0CF6400708A259709A00FD82E18D0E71592D46141CFCEC22FDAD5D613DD761E27043DD96562707E0FF09B30FD0E3A9DF37B70AB3345778821B7EE48A477D77332B0BA24F876CC733506AD2EA0B599DF89E99DB8E88CD6610EEF7BBA7977715637EC499B240EBEB3D4A0F5272660354A834C513C8ADDF9418C2C006FACB31816A77E609761366EB7CE40F4FC20D7EECA20743DA951C69BABE5010031079DDCA7C0352162EEEDE2265A4C5F8CCD601781638E4E5AAAD0B1E9DD7BD521641DCE4A98C031CD9B0613A71A31765C6F46C3518EEA695CB79BC3DCED7391E8C725C4622B31E6817013BDEE16ACB6139C256CDD3B089B23F29EDC03992877B7B2E1E86EDF583174D77B1D0C9A0E864C6F064C4FAAF5C5C8783132BA4C36497CB7F32DE794B69AFDACF0A64E9F9D4252D0806DE703A708C29EF85073D87F237224822DA3CE7B70462862D09DAADE2622DD29D49E1C32DE3391EA6E61D791EB4EF9D086B177350B7BB8CD7578054B05F4D6C0CB28B5E24E0D7E4A9B7EFABC8AF9ABF0285F62AFBFDD1C72010DFC4DD6EFD9F244083FBF0D2F494052B238DF940FB95F78C9C613DF342BE2A3310CA8585A1A09FA338BC75F04F0994A26F948F95E90D949491A6796929000E07DEC871B7FEF051CE55C3DB36564D540E54B2EC99E84DB3C09894066B71E1BC01C8B554C6062BFF524E8F64B667002314FEDE0D53151F4C835DF06111F0301762438F803B3F03022F684BEE8984A2AD6E10092C3C4787D968ACEF89A870D48A3F1E04A9E83FE51BDB33B3115C40A52B910A3A2643990C7474727C2582250CB6C12038B88F998614F18DB0A4A41B64EC778A6FD41C4A53E779EBACAE1C33B0B14DA8FCF41D148DEF99D988E698446A15E8C064DA154EA66823EE959088C4645EA7C662E09735322B453E20C74491DD304495353F69C340BFE62EA44154CE3880A074E5B6A9A672B5663EA2A13D992773D847CB172E5F3F9FFE9518546131EC5E72B570EF4062C08B21C86A3A83773340714D7A9AFABFA5365A6EBE86CD64F369AE83388F8B4941C17FF0460D2967D934A4EF95CEBF350728DCCB291582FA2CB0CDE8BE80A918FD21775075DA6F9ECD40E44577538F22282EE4550236F37DE687459E46329E7B5876EA3112164A8D267B78F963D913CC19DB4205FB2BD343CAAE8683E6B19331B6779A864DF52A6E87D50319BF81ED850C4E7B90F36939E5177C24C90F2672CC87D6A2A8A0DAD0631294B9E916A523DCB3E45B54487D84E5D3331D1E3341A6CC173D04F8AD7E227A6A28038ED39C9521D8985895453FEDC240B0C419B8B802179243AAA8B672F6ECE340B5859FEECF588626A85ED00925D05834E5D5DD6B1FA9CB377F9ED39A845F421FBA9A9C25A6066A5FE86179F99A83917623771D556AFD740AE0F2B01E95744A1C5992FEA2BB4445B12B047DECD05CF6469C57B1D222E898EE39EFA6AC9069D3378B025CF61E554BDC03EB1F5930E2B9F8B1C09B12D6CC17392A2798428314284A597E8AC0F9EA35439531660E5EEF196AEC410ACACCA3331B8E82AA25FAC2445E1E841371522610694C05146B87FA1D50EB8E99D2C3A83C20092FE867DBBBC7166E1DE1917643D6FB52669559F4E3DF0A649BA7026E62528DF2B3F5B6EEFA24C82CBD40D9B5DF299AE23CC01B6AFC696103A6A4AB05E9A0A8A2E9A787CA18BA604EBA2A9A0E8820B86153AE2CAB1EE9A6ACA0E5B7F1EA1AFB608EB067B541DEDA5F166433B6B6AA8FA6C2AAABAC6BB547665D645EB518775D5D65074D956D4642C75218DB296AAA3622E55558180AC638D0E2D3A2A2FBAF1CECA726587653545A7ECE19ED0275B8C75C9D632E8B139229176DCD4D2E9BFA9AC5675E56911A4EACA1289AA2B2BE8758113C9575074A84B1AB74910970BB61CEB95AAA2D921A2D2D9625977650DAE376A011716433673D182AACA2E8D920C47F01EBE2191FA2658256D3BD10E289A4A56F7154B9601C96DAA1D09BD483E1EFE0853201659C66D99644126901706A053953DC6C9A882B64AD19A2B714C74B5CD52920D6DC75C1080C32877603D3282CDCE01704092BEC3C998F3E662D11035033B10880FB234D9843DB2604B61407B209509E694D08B077D3A1D59CE5E66A8C60CE2EEC4B77C553300F3C6773AF203B1418C3B8718A0884E6709801087119E00E1A8D0F72CECFA4CB2274E88B6C5E99407E6BA195F78934AB747779DDD59C0056F6A704216EE395F8608B18012D997860DCE910578089A44EBAB02D61CEB7DE13C8301811F5798B3038C940218A18EA86208810940119F0C13B0954016E9E3642D3064581722B9981319B5B2F01467E30D9EA9B1EDC16332731688811100F18AE80927A30D9DE8158DA5A774DD08669CF81574E30EFFCEC9E74EDE042E60076A6E98D1F25A8F2198FB61D7B19D007B18AF65801DB857B31399E0CE7AEB832EF010D79E38D978CBBD701D8FF120E4326E9A38C5B837A72DCA503B48921587E616E7AFA24B217400AB703C7422D0E0097FD99A2D7146B4F43056E123E79464E124932D704B30C54905D5A89352F7511B9F0DF851A5D23BA83BF22804E1E8D29205F573198D2F485376BA2A2FABAA0FD9CF348A339D72136D4990145F4F571F0E59EB5DF9D0C6E92549FC8716C46906332C9F246B81D675AEC3FBA87683E130AAAB00CFF96DBDD43B8F53FFDEDBA459F186244971BDF4AB171CB22A6F7677647B1DDE1ED2FD21CD4826BBBB80D917E6AE34B2FE4F5702CEA7B7C5FB3B890B123234FD8C04721BBE3EF8C1B6C1FB0A78C6040191FBE8FC9364DFCBB14CE37CD7F6D4407A17859A802AF635AE45F5E29ADC866BEF0F82E3A6E621CBB1D34BDF7B88BD5D52C168DB673F33F1DBEEBEFEFDFF0172DE73BA72C00100, '5.0.0.net45')