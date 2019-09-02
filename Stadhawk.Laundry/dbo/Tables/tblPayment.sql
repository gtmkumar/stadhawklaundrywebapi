CREATE TABLE [dbo].[tblPayment] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [OrderId]         UNIQUEIDENTIFIER NULL,
    [PaymentMode]     INT              NULL,
    [DeuAmount]       DECIMAL (18, 3)  NULL,
    [PaymentStatus]   INT              NULL,
    [PaymentDate]     DATETIME         NULL,
    [PaidAmmount]     DECIMAL (18, 3)  NULL,
    [PaymentedAmount] DECIMAL (18, 3)  NULL,
    [CreatedDate]     DATETIME         NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [ModifiedDate]    DATETIME         NULL,
    [ModifiedBy]      UNIQUEIDENTIFIER NULL,
    [IsDeleted]       BIT              NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

