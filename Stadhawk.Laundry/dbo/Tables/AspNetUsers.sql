﻿CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (450)     NOT NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,
    [FirstName]            NVARCHAR (50)      NULL,
    [MiddleName]           NVARCHAR (50)      NULL,
    [LastName]             NVARCHAR (50)      NULL,
    [DOB]                  DATETIME2 (7)      NULL,
    [FCMToken]             NVARCHAR (MAX)     NULL,
    [DeviceType]           NVARCHAR (1)       NULL,
    [DeviceId]             NVARCHAR (MAX)     NULL,
    [IsGuestUser]          BIT                NULL,
    [CustomerImage]        NVARCHAR (MAX)     NULL,
    [CreatedDate]          DATETIME2 (7)      NULL,
    [CreatedBy]            NVARCHAR (100)     NULL,
    [ModifiedDate]         DATETIME2 (7)      NULL,
    [ModifiedBy]           NVARCHAR (100)     NULL,
    [IsDeleted]            BIT                NULL,
    [Status]               BIT                NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);

