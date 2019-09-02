CREATE TABLE [dbo].[tblService] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Name]         NVARCHAR (200)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

