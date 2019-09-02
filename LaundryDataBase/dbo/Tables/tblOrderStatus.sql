CREATE TABLE [dbo].[tblOrderStatus] (
    [Id]           INT              IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)     NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

