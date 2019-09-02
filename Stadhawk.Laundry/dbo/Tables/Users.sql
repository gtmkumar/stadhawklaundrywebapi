CREATE TABLE [dbo].[Users] (
    [UserId]       UNIQUEIDENTIFIER NOT NULL,
    [UserName]     NVARCHAR (200)   NULL,
    [FullName]     NVARCHAR (200)   NULL,
    [Email]        NVARCHAR (200)   NULL,
    [Contactno]    NVARCHAR (20)    NULL,
    [Password]     NVARCHAR (200)   NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    [Status]       BIT              NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

