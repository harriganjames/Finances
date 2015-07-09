--DROP TABLE [dbo].[CashflowBankAccount]
IF OBJECT_ID('dbo.CashflowBankAccount') IS NULL
BEGIN
PRINT '	Creating [dbo].[CashflowBankAccount] ...'
CREATE TABLE [dbo].[CashflowBankAccount] 
(
     CashflowBankAccountId	INT IDENTITY (1, 1) NOT NULL
    ,CashflowId				INT NOT NULL
    ,BankAccountId			INT NOT NULL

    ,RecordCreatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    --,RecordUpdatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    
    ,PRIMARY KEY CLUSTERED (CashflowBankAccountId ASC)
    ,CONSTRAINT UK_CashflowBankAccount UNIQUE (CashflowId,BankAccountId)

	,CONSTRAINT FK_CashflowBankAccount_CashflowId FOREIGN KEY (CashflowId) REFERENCES dbo.Cashflow(CashflowId) ON DELETE CASCADE
	,CONSTRAINT FK_CashflowBankAccount_BankAccountId FOREIGN KEY (BankAccountId) REFERENCES dbo.BankAccount(BankaccountId)

);
PRINT '	Done.'
END

