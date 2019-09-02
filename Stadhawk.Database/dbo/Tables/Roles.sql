CREATE TABLE [dbo].[Roles] (
    [RoleId]       UNIQUEIDENTIFIER NOT NULL,
    [RoleName]     NVARCHAR (200)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    [Status]       BIT              NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

