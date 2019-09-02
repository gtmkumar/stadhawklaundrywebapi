CREATE TABLE [dbo].[tblSubcategory] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [CategoryId]   UNIQUEIDENTIFIER NOT NULL,
    [Name]         NVARCHAR (200)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    [Price]        DECIMAL (18, 9)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[tblCategory] ([Id])
);



