CREATE TABLE [dbo].[MenuMaster] (
    [MenuIdentity]  INT           IDENTITY (1, 1) NOT NULL,
    [MenuID]        VARCHAR (30)  NOT NULL,
    [MenuName]      VARCHAR (30)  NOT NULL,
    [Parent_MenuID] VARCHAR (30)  NOT NULL,
    [User_Roll]     VARCHAR (256) NOT NULL,
    [MenuFileName]  VARCHAR (100) NOT NULL,
    [MenuURL]       VARCHAR (500) NOT NULL,
    [USE_YN]        CHAR (1)      DEFAULT ('Y') NULL,
    [CreatedDate]   DATETIME      NULL,
    CONSTRAINT [PK_MenuMaster] PRIMARY KEY CLUSTERED ([MenuIdentity] ASC, [MenuID] ASC, [MenuName] ASC)
);

