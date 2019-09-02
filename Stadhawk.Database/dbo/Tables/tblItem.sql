CREATE TABLE [dbo].[tblItem] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [SubcategoryId] UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (200)   NULL,
    [Price]         DECIMAL (18, 6)  NULL,
    [CreatedDate]   DATETIME         NULL,
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    [ModifiedDate]  DATETIME         NULL,
    [ModifiedBy]    UNIQUEIDENTIFIER NULL,
    [IsDeleted]     BIT              NULL,
    [Image]         NVARCHAR (255)   NULL,
    CONSTRAINT [PK_tblItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([SubcategoryId]) REFERENCES [dbo].[tblSubcategory] ([Id])
);







