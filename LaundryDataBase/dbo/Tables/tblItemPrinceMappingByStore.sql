CREATE TABLE [dbo].[tblItemPrinceMappingByStore] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [ItemId]       UNIQUEIDENTIFIER NULL,
    [StoreId]      UNIQUEIDENTIFIER NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ItemId]) REFERENCES [dbo].[tblItem] ([Id]),
    FOREIGN KEY ([StoreId]) REFERENCES [dbo].[tblStore] ([Id])
);

