CREATE TABLE [dbo].[tblStore] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Name]         VARCHAR (200)    NULL,
    [LocationName] VARCHAR (200)    NULL,
    [Latitude]     DECIMAL (30, 20) NULL,
    [Longitude]    DECIMAL (30, 20) NULL,
    [CityId]       INT              NULL,
    [CityName]     VARCHAR (200)    NULL,
    [PinCode]      VARCHAR (10)     NULL,
    [CreatedDate]  DATETIME         NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME         NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    [Image]        NVARCHAR (255)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

