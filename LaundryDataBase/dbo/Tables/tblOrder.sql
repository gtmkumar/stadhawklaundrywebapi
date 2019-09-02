CREATE TABLE [dbo].[tblOrder] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [ItemId]        UNIQUEIDENTIFIER NULL,
    [StoreId]       UNIQUEIDENTIFIER NULL,
    [UserId]        UNIQUEIDENTIFIER NULL,
    [NoOfItem]      INT              NULL,
    [OrderStatusId] INT              NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [ModifiedDate]  DATETIME         NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER NULL,
    [IsDeleted]     BIT              NULL,
    [ServiceId]     UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

