﻿CREATE TABLE [dbo].[UsersInRoles] (
    [UserRolesId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId]      UNIQUEIDENTIFIER NULL,
    [UserId]      UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([UserRolesId] ASC)
);

