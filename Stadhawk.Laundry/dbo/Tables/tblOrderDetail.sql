CREATE TABLE [dbo].[tblOrderDetail] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [OrderId]      UNIQUEIDENTIFIER NULL,
    [ItemId]       UNIQUEIDENTIFIER NULL,
    [NoOfItem]     INT              NULL,
    [Name]         VARCHAR (200)    NULL,
    [Price]        DECIMAL (18, 3)  NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

