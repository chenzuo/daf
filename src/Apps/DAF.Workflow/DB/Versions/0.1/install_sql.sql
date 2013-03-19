﻿CREATE TABLE [dbo].[wf_BizFlow] (
    [FlowId] [nvarchar](50) NOT NULL,
    [Name] [nvarchar](50),
    [Code] [nvarchar](50),
    [TargetType] [nvarchar](50),
    [ClientId] [nvarchar](50),
    [BizGroup] [nvarchar](50),
    [Owner] [nvarchar](50),
    [StartUrl] [nvarchar](200),
    [DetailUrl] [nvarchar](200),
    [Guide] [nvarchar](500),
    [StopWhenIncomeRequired] [bit] NOT NULL,
    [StopWhenOutcomeRequired] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.wf_BizFlow] PRIMARY KEY ([FlowId])
)
CREATE TABLE [dbo].[wf_FlowState] (
    [StateId] [nvarchar](50) NOT NULL,
    [FlowId] [nvarchar](50),
    [Code] [nvarchar](50),
    [Name] [nvarchar](50),
    [Guide] [nvarchar](200),
    [IntervalType] [int],
    [ResponseIntervalValue] [int],
    [TreatIntervalValue] [int],
    [StateType] [int] NOT NULL,
    [Result] [int],
    [AllParallelStateShouldBeEnd] [bit],
    CONSTRAINT [PK_dbo.wf_FlowState] PRIMARY KEY ([StateId])
)
CREATE INDEX [IX_FlowId] ON [dbo].[wf_FlowState]([FlowId])
CREATE TABLE [dbo].[wf_FlowStateOperation] (
    [StateId] [nvarchar](50) NOT NULL,
    [OperationId] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_dbo.wf_FlowStateOperation] PRIMARY KEY ([StateId], [OperationId])
)
CREATE INDEX [IX_StateId] ON [dbo].[wf_FlowStateOperation]([StateId])
CREATE INDEX [IX_OperationId] ON [dbo].[wf_FlowStateOperation]([OperationId])
CREATE TABLE [dbo].[wf_FlowOperation] (
    [OperationId] [nvarchar](50) NOT NULL,
    [FlowId] [nvarchar](50),
    [Code] [nvarchar](50),
    [Name] [nvarchar](50),
    [Guide] [nvarchar](200),
    [OperationUrl] [nvarchar](200),
    [OperationArgs] [nvarchar](200),
    [PermissionUri] [nvarchar](200),
    [DefaultNextStateId] [nvarchar](50),
    [CanPlanned] [bit] NOT NULL,
    [CanCancelled] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.wf_FlowOperation] PRIMARY KEY ([OperationId])
)
CREATE INDEX [IX_FlowId] ON [dbo].[wf_FlowOperation]([FlowId])
CREATE TABLE [dbo].[wf_FlowStateIncome] (
    [StateId] [nvarchar](50) NOT NULL,
    [IncomeId] [nvarchar](50) NOT NULL,
    [IsRequired] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.wf_FlowStateIncome] PRIMARY KEY ([StateId], [IncomeId])
)
CREATE INDEX [IX_StateId] ON [dbo].[wf_FlowStateIncome]([StateId])
CREATE INDEX [IX_IncomeId] ON [dbo].[wf_FlowStateIncome]([IncomeId])
CREATE TABLE [dbo].[wf_FlowIncome] (
    [IncomeId] [nvarchar](50) NOT NULL,
    [FlowId] [nvarchar](50),
    [Code] [nvarchar](50),
    [Name] [nvarchar](50),
    [Description] [nvarchar](200),
    [FileType] [nvarchar](50),
    [SampleFileUrl] [nvarchar](200),
    [UploadUrl] [nvarchar](200),
    CONSTRAINT [PK_dbo.wf_FlowIncome] PRIMARY KEY ([IncomeId])
)
CREATE INDEX [IX_FlowId] ON [dbo].[wf_FlowIncome]([FlowId])
CREATE TABLE [dbo].[wf_FlowStateOutcome] (
    [StateId] [nvarchar](50) NOT NULL,
    [OutcomeId] [nvarchar](50) NOT NULL,
    [IsRequired] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.wf_FlowStateOutcome] PRIMARY KEY ([StateId], [OutcomeId])
)
CREATE INDEX [IX_StateId] ON [dbo].[wf_FlowStateOutcome]([StateId])
CREATE INDEX [IX_OutcomeId] ON [dbo].[wf_FlowStateOutcome]([OutcomeId])
CREATE TABLE [dbo].[wf_FlowOutcome] (
    [OutcomeId] [nvarchar](50) NOT NULL,
    [FlowId] [nvarchar](50),
    [Code] [nvarchar](50),
    [Name] [nvarchar](50),
    [Description] [nvarchar](200),
    [FileType] [nvarchar](50),
    [SampleFileUrl] [nvarchar](200),
    [UploadUrl] [nvarchar](200),
    CONSTRAINT [PK_dbo.wf_FlowOutcome] PRIMARY KEY ([OutcomeId])
)
CREATE INDEX [IX_FlowId] ON [dbo].[wf_FlowOutcome]([FlowId])
CREATE TABLE [dbo].[wf_NextBizFlow] (
    [FlowId] [nvarchar](50) NOT NULL,
    [NextFlowId] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_dbo.wf_NextBizFlow] PRIMARY KEY ([FlowId], [NextFlowId])
)
CREATE INDEX [IX_FlowId] ON [dbo].[wf_NextBizFlow]([FlowId])
CREATE INDEX [IX_NextFlowId] ON [dbo].[wf_NextBizFlow]([NextFlowId])
CREATE TABLE [dbo].[wf_TargetFlow] (
    [TargetFlowId] [nvarchar](50) NOT NULL,
    [FlowId] [nvarchar](50) NOT NULL,
    [TargetId] [nvarchar](50),
    [FlowCode] [nvarchar](50),
    [Title] [nvarchar](50),
    [Message] [nvarchar](50),
    [HasStarted] [bit] NOT NULL,
    [HasCompleted] [bit] NOT NULL,
    [LastTargetFlowId] [nvarchar](50),
    [CreatorId] [nvarchar](50),
    [CreatorName] [nvarchar](50),
    [CreateTime] [datetime] NOT NULL,
    [Result] [int],
    CONSTRAINT [PK_dbo.wf_TargetFlow] PRIMARY KEY ([TargetFlowId])
)
CREATE INDEX [IX_FlowId] ON [dbo].[wf_TargetFlow]([FlowId])
CREATE INDEX [IX_LastTargetFlowId] ON [dbo].[wf_TargetFlow]([LastTargetFlowId])
CREATE TABLE [dbo].[wf_TargetState] (
    [TargetStateId] [nvarchar](50) NOT NULL,
    [TargetFlowId] [nvarchar](50),
    [StateId] [nvarchar](50),
    [OperationId] [nvarchar](50),
    [Title] [nvarchar](50),
    [Message] [nvarchar](50),
    [ResponseExpiryTime] [datetime],
    [TreatExpiryTime] [datetime],
    [ResponsorId] [nvarchar](50),
    [ResponsorName] [nvarchar](50),
    [ResponseTime] [datetime],
    [PlannerId] [nvarchar](50),
    [PlannerName] [nvarchar](50),
    [PlanTreatTime] [datetime],
    [TreaterId] [nvarchar](50),
    [TreaterName] [nvarchar](50),
    [TreatTime] [datetime],
    [OperatorId] [nvarchar](50),
    [OperatorName] [nvarchar](50),
    [OperateTime] [datetime],
    [StateStatus] [int] NOT NULL,
    CONSTRAINT [PK_dbo.wf_TargetState] PRIMARY KEY ([TargetStateId])
)
CREATE INDEX [IX_TargetFlowId] ON [dbo].[wf_TargetState]([TargetFlowId])
CREATE INDEX [IX_OperationId] ON [dbo].[wf_TargetState]([OperationId])
CREATE INDEX [IX_StateId] ON [dbo].[wf_TargetState]([StateId])
CREATE TABLE [dbo].[wf_TargetIncome] (
    [TargetIncomeId] [nvarchar](50) NOT NULL,
    [TargetStateId] [nvarchar](50),
    [IncomeId] [nvarchar](50),
    [Name] [nvarchar](50),
    [Remark] [nvarchar](200),
    [UploaderId] [nvarchar](50),
    [UploaderName] [nvarchar](50),
    [UploadTime] [datetime],
    [Verified] [bit],
    [VerifierId] [nvarchar](50),
    [VerifierName] [nvarchar](50),
    [VerifierTime] [datetime],
    [FileType] [nvarchar](50),
    [FileUrl] [nvarchar](200),
    [FileStatus] [int] NOT NULL,
    CONSTRAINT [PK_dbo.wf_TargetIncome] PRIMARY KEY ([TargetIncomeId])
)
CREATE INDEX [IX_TargetStateId] ON [dbo].[wf_TargetIncome]([TargetStateId])
CREATE INDEX [IX_IncomeId] ON [dbo].[wf_TargetIncome]([IncomeId])
CREATE TABLE [dbo].[wf_TargetOutcome] (
    [TargetOutcomeId] [nvarchar](50) NOT NULL,
    [TargetStateId] [nvarchar](50),
    [OutcomeId] [nvarchar](50),
    [Name] [nvarchar](50),
    [Remark] [nvarchar](200),
    [UploaderId] [nvarchar](50),
    [UploaderName] [nvarchar](50),
    [UploadTime] [datetime],
    [Verified] [bit],
    [VerifierId] [nvarchar](50),
    [VerifierName] [nvarchar](50),
    [VerifierTime] [datetime],
    [FileType] [nvarchar](50),
    [FileUrl] [nvarchar](200),
    [FileStatus] [int] NOT NULL,
    CONSTRAINT [PK_dbo.wf_TargetOutcome] PRIMARY KEY ([TargetOutcomeId])
)
CREATE INDEX [IX_TargetStateId] ON [dbo].[wf_TargetOutcome]([TargetStateId])
CREATE INDEX [IX_OutcomeId] ON [dbo].[wf_TargetOutcome]([OutcomeId])
CREATE TABLE [dbo].[wf_NextTargetState] (
    [TargetStateId] [nvarchar](50) NOT NULL,
    [NextTargetStateId] [nvarchar](50) NOT NULL,
    [ParallelTargetStateId] [nvarchar](50),
    CONSTRAINT [PK_dbo.wf_NextTargetState] PRIMARY KEY ([TargetStateId], [NextTargetStateId])
)
CREATE INDEX [IX_TargetStateId] ON [dbo].[wf_NextTargetState]([TargetStateId])
CREATE INDEX [IX_NextTargetStateId] ON [dbo].[wf_NextTargetState]([NextTargetStateId])
ALTER TABLE [dbo].[wf_FlowState] ADD CONSTRAINT [FK_dbo.wf_FlowState_dbo.wf_BizFlow_FlowId] FOREIGN KEY ([FlowId]) REFERENCES [dbo].[wf_BizFlow] ([FlowId])
ALTER TABLE [dbo].[wf_FlowStateOperation] ADD CONSTRAINT [FK_dbo.wf_FlowStateOperation_dbo.wf_FlowState_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[wf_FlowState] ([StateId]) ON DELETE CASCADE
ALTER TABLE [dbo].[wf_FlowStateOperation] ADD CONSTRAINT [FK_dbo.wf_FlowStateOperation_dbo.wf_FlowOperation_OperationId] FOREIGN KEY ([OperationId]) REFERENCES [dbo].[wf_FlowOperation] ([OperationId]) ON DELETE CASCADE
ALTER TABLE [dbo].[wf_FlowOperation] ADD CONSTRAINT [FK_dbo.wf_FlowOperation_dbo.wf_BizFlow_FlowId] FOREIGN KEY ([FlowId]) REFERENCES [dbo].[wf_BizFlow] ([FlowId])
ALTER TABLE [dbo].[wf_FlowStateIncome] ADD CONSTRAINT [FK_dbo.wf_FlowStateIncome_dbo.wf_FlowState_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[wf_FlowState] ([StateId]) ON DELETE CASCADE
ALTER TABLE [dbo].[wf_FlowStateIncome] ADD CONSTRAINT [FK_dbo.wf_FlowStateIncome_dbo.wf_FlowIncome_IncomeId] FOREIGN KEY ([IncomeId]) REFERENCES [dbo].[wf_FlowIncome] ([IncomeId]) ON DELETE CASCADE
ALTER TABLE [dbo].[wf_FlowIncome] ADD CONSTRAINT [FK_dbo.wf_FlowIncome_dbo.wf_BizFlow_FlowId] FOREIGN KEY ([FlowId]) REFERENCES [dbo].[wf_BizFlow] ([FlowId])
ALTER TABLE [dbo].[wf_FlowStateOutcome] ADD CONSTRAINT [FK_dbo.wf_FlowStateOutcome_dbo.wf_FlowState_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[wf_FlowState] ([StateId]) ON DELETE CASCADE
ALTER TABLE [dbo].[wf_FlowStateOutcome] ADD CONSTRAINT [FK_dbo.wf_FlowStateOutcome_dbo.wf_FlowOutcome_OutcomeId] FOREIGN KEY ([OutcomeId]) REFERENCES [dbo].[wf_FlowOutcome] ([OutcomeId]) ON DELETE CASCADE
ALTER TABLE [dbo].[wf_FlowOutcome] ADD CONSTRAINT [FK_dbo.wf_FlowOutcome_dbo.wf_BizFlow_FlowId] FOREIGN KEY ([FlowId]) REFERENCES [dbo].[wf_BizFlow] ([FlowId])
ALTER TABLE [dbo].[wf_NextBizFlow] ADD CONSTRAINT [FK_dbo.wf_NextBizFlow_dbo.wf_BizFlow_FlowId] FOREIGN KEY ([FlowId]) REFERENCES [dbo].[wf_BizFlow] ([FlowId])
ALTER TABLE [dbo].[wf_NextBizFlow] ADD CONSTRAINT [FK_dbo.wf_NextBizFlow_dbo.wf_BizFlow_NextFlowId] FOREIGN KEY ([NextFlowId]) REFERENCES [dbo].[wf_BizFlow] ([FlowId])
ALTER TABLE [dbo].[wf_TargetFlow] ADD CONSTRAINT [FK_dbo.wf_TargetFlow_dbo.wf_BizFlow_FlowId] FOREIGN KEY ([FlowId]) REFERENCES [dbo].[wf_BizFlow] ([FlowId]) ON DELETE CASCADE
ALTER TABLE [dbo].[wf_TargetFlow] ADD CONSTRAINT [FK_dbo.wf_TargetFlow_dbo.wf_TargetFlow_LastTargetFlowId] FOREIGN KEY ([LastTargetFlowId]) REFERENCES [dbo].[wf_TargetFlow] ([TargetFlowId])
ALTER TABLE [dbo].[wf_TargetState] ADD CONSTRAINT [FK_dbo.wf_TargetState_dbo.wf_TargetFlow_TargetFlowId] FOREIGN KEY ([TargetFlowId]) REFERENCES [dbo].[wf_TargetFlow] ([TargetFlowId])
ALTER TABLE [dbo].[wf_TargetState] ADD CONSTRAINT [FK_dbo.wf_TargetState_dbo.wf_FlowOperation_OperationId] FOREIGN KEY ([OperationId]) REFERENCES [dbo].[wf_FlowOperation] ([OperationId])
ALTER TABLE [dbo].[wf_TargetState] ADD CONSTRAINT [FK_dbo.wf_TargetState_dbo.wf_FlowState_StateId] FOREIGN KEY ([StateId]) REFERENCES [dbo].[wf_FlowState] ([StateId])
ALTER TABLE [dbo].[wf_TargetIncome] ADD CONSTRAINT [FK_dbo.wf_TargetIncome_dbo.wf_TargetState_TargetStateId] FOREIGN KEY ([TargetStateId]) REFERENCES [dbo].[wf_TargetState] ([TargetStateId])
ALTER TABLE [dbo].[wf_TargetIncome] ADD CONSTRAINT [FK_dbo.wf_TargetIncome_dbo.wf_FlowIncome_IncomeId] FOREIGN KEY ([IncomeId]) REFERENCES [dbo].[wf_FlowIncome] ([IncomeId])
ALTER TABLE [dbo].[wf_TargetOutcome] ADD CONSTRAINT [FK_dbo.wf_TargetOutcome_dbo.wf_TargetState_TargetStateId] FOREIGN KEY ([TargetStateId]) REFERENCES [dbo].[wf_TargetState] ([TargetStateId])
ALTER TABLE [dbo].[wf_TargetOutcome] ADD CONSTRAINT [FK_dbo.wf_TargetOutcome_dbo.wf_FlowOutcome_OutcomeId] FOREIGN KEY ([OutcomeId]) REFERENCES [dbo].[wf_FlowOutcome] ([OutcomeId])
ALTER TABLE [dbo].[wf_NextTargetState] ADD CONSTRAINT [FK_dbo.wf_NextTargetState_dbo.wf_TargetState_TargetStateId] FOREIGN KEY ([TargetStateId]) REFERENCES [dbo].[wf_TargetState] ([TargetStateId])
ALTER TABLE [dbo].[wf_NextTargetState] ADD CONSTRAINT [FK_dbo.wf_NextTargetState_dbo.wf_TargetState_NextTargetStateId] FOREIGN KEY ([NextTargetStateId]) REFERENCES [dbo].[wf_TargetState] ([TargetStateId])
ALTER TABLE [dbo].[wf_NextTargetState] ADD CONSTRAINT [FK_dbo.wf_NextTargetState_dbo.wf_TargetState_ParallelTargetStateId] FOREIGN KEY ([ParallelTargetStateId]) REFERENCES [dbo].[wf_TargetState] ([TargetStateId])
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
INSERT INTO [__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES ('201302030217203_Init', 0x1F8B0800000000000400ED5D5B6FDC38B27E3FC0F90F8D7E5C60DD4E66E69CC9C0DE85E34B26D8719213E7827D0A946EDA16A2967A2475C63E7F6D1FF6279DBF70A83B2F55BC48A4D4ED692048D222592C163F168B6415F97FFFFAF7C9DF1FD6D1EC3B49B330894FE7CF8E8EE733122F935518DF9DCEB7F9ED5F7F9EFFFD6FFFF91F2797ABF5C3EC5393EF87221F2D1967A7F3FB3CDFFCB25864CB7BB20EB2A375B84C932CB9CD8F96C97A11AC92C5F3E3E3178B67CF16849298535AB3D9C9FB6D9C876B52FEA03FCF93784936F93688AE931589B2FA3B4DB929A9CEDE046B926D8225399D5F9C5D1D7D4ED26FB751F2C7D1C5CBA3CBABF9EC2C0A03CAC90D896EE7B3CD8FBF7CCCC84D9E26F1DDCD26C8C320FAF0B82134FD36883252F3FDCBE64753D68F9F17AC2F82384E724A2E897B357DDE368A36EB92363F7F2CD82A9B763A7F19FEEF156D109B8966FB0779E43ED04FEFD26443D2FCF13DB9AD8B16E55EAFE6B3055F7621166E8B0AE50A2EA8ECF29476F97C76153E90D56F24BECBEF5B815D070FCD979F68BF7F8C430A105A264FB734F5CD368A82AF1169B32F94D5167FBBACB4FAADAEF39C961CBBCE0F417A47F2A2CED15B1B8524CEDD76AD49BD14C3AFD264BB19BBDEB77FC4241DBBD29B3C48F38F6964592F5508032BBE2079104653D4FC6A1BF61846C3059D6C3EDF93F8359D95D6E43DF97D1BA6A4C5F6CB248948105B6BA186EADB6D3E88EC9BE07B78574E0A323E7292CD67EF49542667F7E1A69AA18E0AC55B267F2955FEEC2A4DD6EF93A856C95DCA974A85509E1230F926D9A64B0B962A09E23C55E920534C12C8159B6ECB56DD05385F75069031360DE48CCB60CD1AFD51718433D76481D9E3526106F92CB62CBE210F796D3CC04CD6895FF88C1D9B507ACB45C32898A9690DCBEAC9A2336D94064F8BE53E264F59B08FCDD3161CD9E8716F6BEDAAD9338579D767621A3E1DBE8E73927EAF16176DE5C588BBA018FB405737EFA885604BF43D5DE6D0E14B1AE29F8268DB52A71F7F786E6D87A624C8DD912B4790D464E6ABE5C8A00DDE463947AC182CCD673BE6CEA2880A3D882212951CDDDC27DB68F5925CC6AA991DA48C2ADC4ACD0F99D645E58ACCFA0E27A9927A37D1547A176250C8835B206246D7B648A5A82BAB026797CD80F3CAE5726E9D54E2A8AD0C8564D91C0AB972D92066ED67D8B6AF1C4EB560DEB6A27D9A9A39A67DD7AD5E2EB81FBFA8AAC106BAB5D231E59929A0E49B614CCF7BF7BFC1B6E8A0413208F8A302F060973E79BBB4C5D3147B446DE567E95D367AEDEF48BA0EB3AC6C7B38C1CEDC6D408DD6628DEC6132331A5841FC2E0AE278F8FE18A544FF2C0935A51D6E8A296D67AB9D14687240365B86194F95F1E8DB72AA6AD927B3A9E378EC8A334F9BB52ED722A8ED02AE58EC164D467C365971461B4E349CD6FF0C1E4CFDC751FFB13119460F6695CF3A2F48B64CC34DB59818798EBF0A237EF76BACE3C660BD894851FB1456DDC74D9404AB516AEE673B181E5D41CA0E3ADA1AB8DF52EDE178DF6DA9AAD927A38161F9CF6135D8EE0BE27B1EE0F6A1E526A619AF6D6605B72D3B3A7E9B7F87EFCFF41F540306CA74803D98100713E260428C6842187B99805B0F901B4A2F5DC77858B8730605B31635ED9BEF28C3F264473505349A1E82C0C4F4E0172E6F8729248B042D2C9FEDFCFF21316598C909B3DB665032DBE51A3418AAC27DC74257BA0FC4F9D27B3FFB1B555BB5797CB3A368EE243EDA611E8D5EE935C9B2E06EF46A7F0DB2D26779F8D900A5749E14B684035ABF0559EE6BA41919BC857754924E56F124D11045D5A5875A53F345FB7B54EF2D7B43AD838A64A70949D2F424A6DBCEA13C5275DC89B9413EF94C2A8E859CD6F37FD9E32B854F7A4591DD5D80F91673489E4C48B6419E4C0CCDFE76406F9F61A1F82453F234CA71A2536C4F0E310783007635BE7CD884E9A36E3A3076357647AE66708AD9B9AD7A8AF9B9E9181732ACDC412690605DF114F22BAA2EA1E80CD35308B0AE780A013A135EA5C9A718C04DCD5388AFAADBC9F0ADA218E85FDB8C33B2B9EFBD77B28CCCD91E36216CC8A2A6A31DBBAAC80136C717D66A947896F320962C90D13684A02AAB0C24E0B2A839073221AC43396D792F6A67CAEB170F7201182E623E2568A4CCF61BA0568D10B323884F8C1B206475EB6DCF5604BAD9831994FCAA1DEBFB1D8A733084B1CD242AD9C34FC12DD794FDBDD2D8F2FD57959379A8795BD49A454EBA6FF5AE9E34BF27EB20FD36D171EB14766B53F314C2AEEA7661787D2269781B2AF7B72DC84CD00D4DCD53744353B78B8E98CA5B622A3F89A25EC0E0673F0FB4F77533744FF3199EAF1576B603AF77AE0ED9E51D4856B3E9C2D99DB3BDFB5B16039CD624027F26DBC24BB30FC6C5C1B838181707E3E2605C0C352E7A6F71C1F3B66A33CC85733C5F0BE0190F66D030EBC427BEF0B6F3792E8EBA8CEEF5693AD080913968EE271AD74832DDDF45FD4315BBB1A2A7A8222BE833AACA3F689FD7A8314209BC295C466D43F8DCB6CD004062D418B01CDE2420BBB66150193B45B65D336A8CBF2BED7576150577DD4DCD1F636AEB458F744C54E3A3BAB18C17E235597F25694DEE9F2448E7B3F296B3D3F93349E05CDEEB24CEEFDBCCCFD5992F82C736EB0FEAACBF5209B6797FD4F0B08DC3A28FEADC3FA973DF906552DC6456E7FE2F75EECF847C6BF3FEB73AEFFF6C0BCFD08EED9FE5CEACBA4DD19525162A6B6A703FBE247761DC7273AC114B9E6C4C3BFD1DB51E93154F5ED39F5511AE124DAF56252E99AED2746C33A878BE34FDDBDD74C770A6E9E7A60CCBDBCFEA226CD6172F7AE08275081D0C8C37494C4C71F1CFE2E8CE0C166F124C119843BFB1A0476D63EBD26D08FFE6761833ADD77822AD4CC74AED687A99A609A387356DA80B7D0ED20EFBCF340DA9CBBC49D2751075A55EA84B5D857198DD93A18066964B833BFB220D6E73D3EEBBD97E5D87796EDE81DD4E8359FFBD8E69BED0563E6759962CC3D23861C67C6DD1C9C13B54A3CCD41763F2DEE4B51D734D3548B889C225B52A4EE77F911A82926DBD233AB26D90134FF4F8E848EE01BAFC2129898B7724CEE960C8D3208C7379AD14C6CB7013444A168452E01A0B8DE05BB475882917644328EEE25C29D66195B77508EB3D9D744E160C3A0C41C3F828006B5BA09F85024A20311E123D1025D604600B85EC33B10B4EDEC617A408A3999D2DF3F25D95F3205B062B79FD4687DDCA252A9166984004DF2FE80150A4E306F23112563BE6B54A8ECF8AE1B3373405F253293C988D11951E2CE67D547C1D142C5412E0DBE55101CA3E62A670DE49452835C704365CA88A5B852875A6037EC6C43277619E1656F0ED79007A9B53FE1ED0856F0BDFAF891B6AC3E8B336D459FB3165B3F784A9F0025E1AC623A51F10C18774469EA6011E469CA301D1EECF04CDBB1A996A1CD1F5C88B5A137C978CD0BA7B8A8D6F8509303017EC41AA8DEFB1A16C8C6A427237CEE9CD39F8FA39C86C6C0EBFFB188DF0E317FB35F5828D187FC50C75D87E4CBEDC0D5BCAE52C78DD96B0B8E80747F8BDB8B1D7C9001363AE9201F1EECF142C3AE3182B1FC939C7939213DD7BCC80BB838A4E6888D1FA13F35A1EA6EC84AE1BCCC90878C56E81C360A4BD12AE43117B45A005507597C9E9B5E0F418D5B4612425AAE9AB9DD7A5F0857F26B8016EFF738A4BF9DEC07D43A5D482093029F592090FF875A023A1127CA816038DFAD5DA7EF69C863A804873C45BC8A1AAA7522EAA7129E4835ADF65B1199022E17D1A8A08EF230D42A44B767E4260F8162EAE33C008768B9D431822D7DF1955E16EE9A665C8A49FBB52EEB006F7800937F2F59B13E28FDD5831009F981D479EB5031152C1A4B8C3B899007498E45DB0321ADA803067351C5431CF2220ECB7F3B5E1D266A8768D3A9C21F3CE76B1FFA9EB0227DC8C8D3DCD311290D71BE2FA9C1E79C2DA4447400A690FE561345C41D195EAEE57865A8A18EBB13B8AD732AD5E53703485625374C37E6936D3DD7930B747E4F5DB94F785BAC976D69562DF976D75769088B1A8AA4D4645396C3BB3E7FAC134ACD54CF74DBFAB62D89E7135A76197EE8BFE5407269B220D8952F6826A38C6795F310DB6665A44839D69BA97BF8B9886E2D34DD1A60C56F7826F55A8FBD856AB056FD36256D14D268C21D7634CBE2B295D6B6CB27588DF71DC1F3D86350138B51B197D776F136B2961D728BB96117207F3D812D247F680B93DED6EF70FE2F1B3BF3D69148E52F00E1819596799ECFF30393D21CCDEE7D70FB22671DB45853C9001E748AA2EEBA165725AA2BB422649BFDDD26EBA7829C1A8C87E4372DE77633EEB2EFD113D0524B0F014183448349834532A8CDAC2C931990CE8EA48DA51E3A23E70169B1C061495C42CE8F08EB00AE935594C64A724674A8973B7912871A91A4AD5B84408B189467430DC72A94694D03EE4938D68E13217D20DA4CE1975B0E8558D65940E80B5FA294726930835C0118A9B6FF88CED44D6B684532F9236D55E1CC2D0C100B6E09B68D37CF1B60B9520943763C04DC1EEC6808483AA3033D280BC70D5DD4F62C29D0B88AC543733484D41EE66101A612E1AE43286B140C4306A0424643DA2EB717961E21450F2BAC4B43F8648918B5957890F0F6E871B0686B743028335BC01D111C61E1B3C8D08078DAF965A0045580BBC1BCA020AA9F63BD8840707F438811C3D947D2A787B38428AE0E46124EE414A898B4555AA233C6A15D11460DC2AA8826033C784EC18F3191B1389CD6668DCA43CF7409193A2E6341408182AE97916132FDA36800CE869A0EE5DD1DDC0196C442F0333C9F79018165D0608CC28104D3A075185A231AD52ACB98C487A85151CE6A41111120F85B6468E881A281E3904CA8B70C0681B4036FAA81CAE25CAB81C7D43F4B400A1A816FEF682E956FFA85216B3E04D10724292C0771B9494BC8283A94C8807514B43153C82B506091F192421245EC488666F71B136835A56524E5DA3C402B894F47B1C08C571640438DDA352D239E803AD52B8E84BED32B09F51A2A8B05C99865CCDF83A03CA66D81C7C85D14F3C23AD2DB8AD4B331C699DA28146A9DCA2255199988A38D991B064605AC3194DDBA430AAFB8A6954735AE14E81D98CAA226A634F5112B323F92D788D31A9A2EF1D709A2756F4B254B83E2A5B0A3B3FBA9223ECEE389A14C1B75DF4B2D4BADC29DBAC72BA732557959B9D67A5080E107019A4CC6F666CA1DE60038D38D4F7CBA283861ABD898DFC543E62B8A90A7B890D3580619FB0F124A73CAD81339AB54C793ED34754A39EC870FA466DDEB199CC5AA231E7ECC4E274A7B87934ABF5C369D34E1637CB7BB20EEA0F270B9A654936F93688AE931589B226E13AD86CC2F82EEB4AD65F66379B6049993FFFEBCD7CF6B08EE2EC747E9FE79B5F168BAC249D1DADC3659A64C96D7E44EDAA45B04A16CF8F8F5F2C9E3D5BAC2B1A8B25375C45AFA1B6A63C49833B22A4D2AA29A757619AE517411E7C0D8AF752CE576B299BDEEBA8A988773E923BAC715368F217FFAF9F5D39BB3A6AEA39BA78797479758490E92478451BB52E7CBB8AF61160E3452A49CBDE2C832848C557F7A22AFCFD3C89B6EB587D01084EA5F89BA7517D31A75074084FA1FA624EA1027EF5EA174B87FD6EC14F1452F98A92E9BE9A53A25DF22A4DB61B9E52F7D59CD2DB3FE2E26134964CFDC99C06D503695E3EF8CA92E9BE9A53BA207494441229E6B339AD57DB50ECFEFA934DCB92CDE77B12570BFCF7E4F76D989295D84E388F7D2DF58A4F5D8D9449AEE764210C6951752C24DD21A87051151929AA6E4A18A8AA504206CA4A515601DFCA1155402FEC9D8AD371A3F6862BADE18AD3C5D0791DE724FD1E44B2F2E453CC2936CFB535E5CB47BC78D248160B855F3CBCA6A8004AB75294CD1B9212D67A48A37C7350687EF9CD9CCA5914750F2D522E6EEE936DB47A494794006465C6DD5341AAF5443F5D8452B4514A0A22BEB51317BCC14DF7AAA88E497BD269270EEDBF1E5DD757E48709C64ABA92B9C8A7F4A07896DE6508C92AC99CE63B92AEC32C2B9909799A42928D957C1B503D5FECDD80DA014AB7C04710B7EFA8722861BE5B51A37F96844E1C323D2665A7140FEB81E86AFE80C9D94C1E1805DF334777F7136FC56137A5282865F0DA86FDBE534070878141DD6FDBF3EE7AECE94C3917245BA6E1A6DAC1E5B5259360219930024CF9EEABC5AA20586F22529494F750F824739A1F375112AC247ACCE79D1A679C4FAA338B1DA66765AF6324BC5BEBDD45549C1982DF4F85D3DA3BADEB1008C33060DDFD2EBBEDA07A0FAAB74BF732D858D7E281834D41CA60B0294B7B3E66621E86E040AE783062B20E533AD55AF5174EC9A0BB5485D5476990A4F994B11564EDE609F264CF8FAC6EBBAF163C8579246E3C579FCC695C932C0BEE042AED47733ABF06597992275A0FEC772B6AE749A158217A4C8A3945F98E7F96AAFE05009CF279B1D79FA4D2796DF7D99A963C97720996F4C8871024577FF7759630A9EE7372C0A82065ACFD2C0F199942B0AEE9B14870AF52A73E6CD80785D89C315E3E6CC2F4511E8250BAE50124465A4AB4E65A56675C420F7AB24A1392EC258BCBD4B6D5D586B5D466E6B3352DB9BD5C821DBDB243E5E60A4996E891DBCB7CB6A625B7974BB0A48760DAB69D951A91C1CC7EB7A72637954FB1A50800994BB0D4CBC55FDB0CD0CD4DC28E4DD56E36CF55B48C276BDB0D74B614B234E8B199EEC30670B7D53F7C9FE73D5907E937517357DF6CF74F6425C67EB7A726B78E4FB1A5280F6DF6BB39B54F240D6F437131D47DB5A624C98DFD6E4F4D961B9F624F51961C9F32C53E20B803D863EFAF2802A969F6FB8E6969479BED4A62C67ADA7AC39D2B062BD55E9BEF3E54B5CBC38183B2B6A17850D60765BDE7CA5A0AD473705E336C1B4C4B614CDD0A3C09221EE6F4A68D3CDAC02DD8CDDE75F08F1E3EF60E0C2413EE4432091A138A589CDE15618572EC127A87922C2FE3939FF6FE2D64D3B37C58A7A1D393CB3A0E7200978A134567A793CA969E27F12A2CE0317B9DBDD946D1E9FC36883262234B31C2D31A88EAE874D39D7BA0A8A58A02BADD24A2DDB0EBCCD4DC178DCAB084AD49E4BC53FE6DC13C4C490F87B6B1E89DA25CBC41C018E36241B708876F1DF0886FE9E18B410087AF3738C0DB48F27A7C4BE1FA6296D698A9BFB4BFDB70FD3A549E8BE12FC55744E49762CBEAB07D3176BECA329F51697C0F5745DCFCCD639693F55191E1E8E6F7A80A9BEE325C0771784BB2FC43F28DC4A7F3E7C7C73FCF6767511864D57D0AF6B70290D57A9165AB08B813A018BFBCA120C4F2FF8348FDD7F46BF7020A36BD8A854F10C3A27AA220FE1EA4CBFB209DCFAE8387DF487C97DF9FCE7F3A9ECF0A90045F8B1B1D6AA02C9454AB25A231CD3C95631F459295DF8B53926CE4BD5B5EDB307CA764BB987CA764EB187DA734BB807D05593A386CE932D1FB6E09D721634A21F490021CCE5FD5F235CCAD07161AB96F4A925D142A751112FC6EA68D503B48A78EDA826EF591B5969B482379D09B7A60F7182D7CD87D453B8C735B3248887D6F7A50447D6F624C243D48C308788DE75B6F2E94B1F2E0A097A9DA8FF9D6B5CAE1E007F32A7CB82653161C4F43485BC97D90C80789D1597B0F7AD7B7DE6DBBCA8305D4D2AEC2D2DD127FC7C7A7BB360AE51875B78863C2D5FBDB6E7C90BA3F830D725072AFB931B7A0C9D476C79063BA99672BBB7F7FF5EF035FC2FA132B7C2E00D1AD82EB4EB6DD2ED2F9B044B73C33318A4308F7B05D21BF1F0F962BE66A339DDDDA71B4671A7040970DE8066FF23A28C183129C4609A2DE13430E11C0ACB8D3C064670E0C476E298B2E0C03F067DC935858B45947AA62F6F4FDC3979E58371A51ED42AC9DEA862EDEDAEDB953156EE894661B7CE8942A1B98DD7FE1CB8763F7A7230760BB9D61BB606C1F747D9C893211DA15E115FD9D97BFFD6E545BEAB1DE07491AB71A334DE6C9DEF608453FDB58FD367A9F96468322AAF1A1637CE8E48E1C1743EDA3E95E34111F543D4C024C44B5531EB9D86AE79499D8630778F2D07C2ED4DA3D65174DAF149407E837843D34BE22ED04F85C00B6E999B3E54CDC7FB3591DAB6C3A17FBDA78EE3BD39B793B58F33CD1F64B1374E76317C3834AE223F13C9076312ABB903C43770B94847B01F221795E48BB10A1A7AD3B4F9B766CCC9E272D3C60C35B13886CAA87BD6D7EFB54C47D983E68E283263E68E2832606CF4A7CEE4EA1C726BBBCA705F0E7B60224BAD9ED7A148FCB1BA79E675ECE8898182BD1E7E10B7050D43DC2DA9C08D64C952FC2B6DFAEB7511E6EA270496B3C9D1F1F1DC9D15D1D25E6A5568616F395A7F61789141D092425B47941749EC4599E06A11CB7FE2E0DE365B80922817F219FDD11E6A2A52AA65C900D8957C5B50462238755D8D21506B84E065C349D2100DAAD5DE879DFFE1DF84C0A027D1B5F90E2206776B6ACDE173E0FB265205F495C861F6AB968D986D96192BD00CBAEA715A196F6D8EA9A36B0EA695086B02FF431DEBD68CFFE091067D7F5DCA1CDF8C8D3543F12FA3AE08D38D1F540EFFE4E7876A8D88D49AFDA97DDAF19AFDE7F077969D29EDA5C875EB8BAB3135D8D2CF0B084EF54A43FE1AE7CD2E0B2E865FC4A59CFF052553C12BE6A688D388F5941747F67300B18ECC6F455EF66EFD7FCD59C5CC0D67393F8D46630FC2EDA9D9DC21A74C1474D82A58B742AD29F4F1C63369DADB870D737CE94558FB52EAB4136E6AACC0EABFB3B9FD96061D2098D093A29EF098403507AE1C193A661C364582EB8EF4F0147EA97147718471F92038AF61645EA0B6547401278D5B0FFE9E90008A44AFD7DCA2380A20BD77066AE78522C4CEC1FCB04FBF929A048F96CEC8ECD4D0C78F828341446D69DA8512E5DC1EA06D2318161D953EA17237B21A46CB349DDFA475A47C40BEB1A320158E46D23EEFB13848AF9F6D08E61C4DB79FACE0265BA93705BA84C7F04CE22C5E96EF14EA363DCBD5E5B544CBBCDCB06537D41591FD69546E0000E97F8048FB38CADB67707138BB3266DD563A3C5F5A1F60EC3648A63696B804C7C26CD85034DAA4AA09D7D21E5492A139B8DFEDDD1261ECF17771A2DD39C10DA2365EAE3412192447C41CAA576F1B8BF8F7123A53D31CDA47FD271377593883AEE59A703E69E1CE68C1EC29A00774098DB88F6D4014576B51B3EB93AFA5E8FF42AE50142BB0A2145CCE9E430121E7E3C8068EF4004BE75E91C4597FC138D3553D2738A1280EA1744F9C3FEF9AC8B62164FD1AB07194FE7ABAF09C5441507FDC7ED97365D02145F03B39D2DD5C1A421B530394CEB614E57F00A994CBA9A99AC062CE86A37ABD8AE4E2E64046F739343D7E0269F41CDCA4A0DEAB3A88AF72756F46C9345DBAF4D46935E55D66B52A5696D9CFB91541B978AD4C6E5D1D4C69E594B95B189485D6C16A3AA304DC0A52A2B33D306FC7E35529D06A07C26A30A719C08E9CA2A6DB0A216AB944381199578F91783E10B2D664C267150024E5C9CED2120BE7CE11AC1B0B45FC9B5BD2C894F1AC2D3C7368D142F6D50355779C1C39006C065A5398727824F264EC4C190371209EA4780FB4F08AD421BB413E211C2DF11A1A882E49D0D8D1E221C82092EC45B05063C16DCFDE0E0E7009E02A2DB878BA021AC97017CF6881CF10A6D80D99F580C6CC82CD27E34AAD619F0AD0436480D7261A14A058807907A9811783B4450788879E1400C2D69034120E763D869A4A8C0E0464C2E0E2EC80ED3FD68209E3BCD6F27B61E8DC502C980361BC59C39693AB07A2ACBAAD644C39ADEC53E691A8E0449ED51B3C1401DA0D5FA809E3D6AB4188802B45719ABE2A4A9F232BD2CAA587D0F6AA81034A16EB22AC262582390D25544855F31B0BBBA6A194839FD0A40B60C54CBF661CD572EEC0CFCE19DAFE8C617016AD6699CBC9D59746336197050465BAE73661EDA08A03C60D32B77E8068A015FC2691D741DAEDFC66D3AE4588A0A40EB85EA01039059ABDE331D2A0AC58AC6C0B5D2E972663C11E8DC011153D7D883D02932905DF0D61674AB2AD53E6B06825138B93D21B1402E5506C2D17A62EDB18894BE421A1343ED5BF444842278BEE84C6E859FCC1E08A4B937BE75E068D34E16D5215DFD81FECC9334B823D7C98A4459F9F564F17E1B172F4A54BF2E4816DE75244E28CD982C39879136CFEBF836697C59048E9A2CD2D38E79B00AF2E02CCDC3DB6099D3E425C9B230BE9BCF3E05D19666B95C7F25ABD7319D8036DB9C3699ACBF468FAC300AFF1755FD270B89E793B7E5C3E4998B265036C3E2118EB7F1CB6D18AD5ABEAF80172910128563CD2B12D7CB949BBC7846EEEEB1A5F426890D09D5E26BFD813E90F5262A20FC36BE09BE139C37BD0C79899D5C84C15D1AACB39A46579EFEA4F05BAD1FFEF6FFA762E1B32A9B0100, '5.0.0.net45')