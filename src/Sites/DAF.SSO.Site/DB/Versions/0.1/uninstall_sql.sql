﻿DROP INDEX [IX_RoleId] ON [dbo].[sso_RolePermission]
DROP INDEX [IX_ParentRoleId] ON [dbo].[sso_Role]
DROP INDEX [IX_RoleId] ON [dbo].[sso_UserRole]
DROP INDEX [IX_UserId] ON [dbo].[sso_UserRole]
ALTER TABLE [dbo].[sso_RolePermission] DROP CONSTRAINT [FK_dbo.sso_RolePermission_dbo.sso_Role_RoleId]
ALTER TABLE [dbo].[sso_Role] DROP CONSTRAINT [FK_dbo.sso_Role_dbo.sso_Role_ParentRoleId]
ALTER TABLE [dbo].[sso_UserRole] DROP CONSTRAINT [FK_dbo.sso_UserRole_dbo.sso_Role_RoleId]
ALTER TABLE [dbo].[sso_UserRole] DROP CONSTRAINT [FK_dbo.sso_UserRole_dbo.sso_User_UserId]
DROP TABLE [dbo].[sso_ServerSession]
DROP TABLE [dbo].[sso_Permission]
DROP TABLE [dbo].[sso_RolePermission]
DROP TABLE [dbo].[sso_Role]
DROP TABLE [dbo].[sso_UserRole]
DROP TABLE [dbo].[sso_User]
DROP TABLE [dbo].[sso_Consumer]
DROP TABLE [dbo].[sso_Client]
DELETE FROM [__MigrationHistory] WHERE [MigrationId] = '201210140241330_Init'
DROP TABLE [dbo].[__MigrationHistory]