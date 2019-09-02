CREATE TABLE [dbo].[tblImage] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (200)   NULL,
    [CategoryId]    UNIQUEIDENTIFIER NULL,
    [SubcategoryId] UNIQUEIDENTIFIER NULL,
    [ItemId]        UNIQUEIDENTIFIER NULL,
    [ServiceId]     UNIQUEIDENTIFIER NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [ModifiedDate]  DATETIME         NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER NULL,
    [IsDeleted]     BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

