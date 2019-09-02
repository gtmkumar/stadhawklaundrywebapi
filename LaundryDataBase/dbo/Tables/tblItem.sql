CREATE TABLE [dbo].[tblItem] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [ServiceId]    UNIQUEIDENTIFIER NOT NULL,
    [Name]         NVARCHAR (200)   NULL,
    [Image]        NVARCHAR (255)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[tblService] ([Id])
);

