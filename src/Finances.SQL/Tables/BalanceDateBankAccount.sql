--DROP TABLE [dbo].[BalanceDateBankAccount]
IF OBJECT_ID('dbo.BalanceDateBankAccount') IS NULL
BEGIN
PRINT '	Creating [dbo].[BalanceDateBankAccount] ...'
CREATE TABLE [dbo].[BalanceDateBankAccount] 
(
     BalanceDateBankAccountId	INT IDENTITY (1, 1) NOT NULL
    
	,BalanceDateId				INT NOT NULL
    ,BankAccountId				INT NOT NULL
	,BalanceAmount				MONEY NOT NULL

    ,RecordCreatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    --,RecordUpdatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    
    ,PRIMARY KEY CLUSTERED (BalanceDateBankAccountId ASC)
    ,CONSTRAINT UK_BalanceDateBankAccount UNIQUE (BalanceDateId,BankAccountId)

	,CONSTRAINT FK_BalanceDateBankAccount_BalanceDateId FOREIGN KEY (BalanceDateId) REFERENCES dbo.BalanceDate(BalanceDateId) ON DELETE CASCADE
	,CONSTRAINT FK_BalanceDateBankAccount_BankAccountId FOREIGN KEY (BankAccountId) REFERENCES dbo.BankAccount(BankAccountId) ON DELETE CASCADE

);
PRINT '	Done.'
END

