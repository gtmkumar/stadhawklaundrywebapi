﻿CREATE TABLE [dbo].[tblOrder] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [StoreId]          UNIQUEIDENTIFIER NULL,
    [UserId]           UNIQUEIDENTIFIER NULL,
    [OrderStatusId]    INT              NULL,
    [CreatedDate]      DATETIME         NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [ModifiedDate]     DATETIME         NULL,
    [ModifiedBy]       UNIQUEIDENTIFIER NULL,
    [IsDeleted]        BIT              NULL,
    [TotalAmount]      DECIMAL (18, 3)  NULL,
    [OrderShipName]    VARCHAR (200)    NULL,
    [OrdrShipAddress]  VARCHAR (MAX)    NULL,
    [OrdrShipAddress2] VARCHAR (MAX)    NULL,
    [OrderCity]        VARCHAR (200)    NULL,
    [OrderState]       VARCHAR (200)    NULL,
    [OrderZIP]         VARCHAR (20)     NULL,
    [OrderPhone]       VARCHAR (20)     NULL,
    [OrderSiping]      DECIMAL (18, 3)  NULL,
    [OrdrGST]          DECIMAL (2, 2)   NULL,
    [OrderEmail]       NVARCHAR (200)   NULL,
    [OrderDate]        DATETIME         NULL,
    [OrderTrakingNo]   NVARCHAR (200)   NULL,
    [OrderStatus]      INT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

