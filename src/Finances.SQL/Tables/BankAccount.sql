﻿IF OBJECT_ID('dbo.BankAccount') IS NULL
BEGIN
PRINT '	Creating [dbo].[BankAccount] ...'
CREATE TABLE [dbo].[BankAccount] 
(
     BankAccountId  INT IDENTITY (1, 1) NOT NULL
    ,Name           VARCHAR (100) NOT NULL
    ,BankId         INT NOT NULL
    ,SortCode       VARCHAR(6) NULL
    ,AccountNumber  VARCHAR(50) NULL
    ,AccountOwner   VARCHAR(20) NOT NULL  -- Mike,Tina,Joint
    ,PaysTaxableInterest  BIT NOT NULL
    ,LoginURL		VARCHAR(200) NULL
    ,LoginID		VARCHAR(100) NULL
	,PasswordHint	VARCHAR(500) NULL
    ,OpenedDate     DATE NOT NULL
    ,ClosedDate     DATE NULL
	,InitialRate	NUMERIC(5,4) NULL
	,MilestoneDate	DATE NULL
	,MilestoneNotes	VARCHAR(500) NULL
    ,Notes          VARCHAR(1000) NULL

    ,RecordCreatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    ,RecordUpdatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    
    ,PRIMARY KEY CLUSTERED (BankAccountId ASC)
    ,CONSTRAINT UK_BankAccount_BankId_Name UNIQUE (BankId,Name)
    ,CONSTRAINT FK_BankAccount_BankId FOREIGN KEY (BankId) REFERENCES dbo.Bank(BankId)
);
PRINT '	Done.'
END


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TR_BankAccount_UPDATE]'))
DROP TRIGGER [dbo].[TR_BankAccount_UPDATE]
GO
PRINT '	Creating trigger dbo.TR_BankAccount_UPDATE ...'
GO
CREATE TRIGGER dbo.TR_BankAccount_UPDATE
ON dbo.BankAccount
FOR UPDATE
AS
BEGIN
	SET NOCOUNT ON
	UPDATE	a
	SET		RecordUpdatedDateTime=GETDATE()
	FROM	dbo.BankAccount a
		INNER JOIN inserted i
			ON	a.BankAccountId=i.BankAccountId
END
GO
