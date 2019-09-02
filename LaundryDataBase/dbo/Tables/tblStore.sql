CREATE TABLE [dbo].[tblStore] (
    [Id]           UNIQUEIDENTIFIER  NOT NULL,
    [Name]         NVARCHAR (200)    NULL,
    [LocationName] NVARCHAR (200)    NULL,
    [Latitude]     [sys].[geography] NULL,
    [Longitude]    [sys].[geography] NULL,
    [CityId]       INT               NULL,
    [CityName]     VARCHAR (200)     NULL,
    [PinCode]      VARCHAR (10)      NULL,
    [Image]        NVARCHAR (255)    NULL,
    [CreatedDate]  DATETIME          NULL,
    [CreatedBy]    UNIQUEIDENTIFIER  NULL,
    [ModifiedDate] DATETIME          NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER  NULL,
    [IsDeleted]    BIT               NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

