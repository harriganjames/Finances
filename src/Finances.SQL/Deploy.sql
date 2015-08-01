
IF OBJECT_ID('dbo.Bank') IS NULL
BEGIN
	CREATE TABLE dbo.Bank
	(
		BankId  INT IDENTITY(1,1) NOT NULL
		,Name   VARCHAR(100)
		,Logo   VARBINARY(MAX)
		,CONSTRAINT PK_Bank PRIMARY KEY CLUSTERED (BankId ASC)
		,CONSTRAINT UK_Bank_Name UNIQUE (Name)
	)
END

IF (SELECT COUNT(*) FROM dbo.Bank)=0
BEGIN
	INSERT	dbo.Bank (Name)
	VALUES	('first direct')
		,('HSBC')
		,('Amex')
		,('Capital One')
		,('Derbyshire')
END



--dbo.AccountType
--Current
--Savings
--ISA
--CreditCard
--Pension?


IF OBJECT_ID('dbo.BankAccount') IS NULL
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


GO
IF (SELECT COUNT(*) FROM dbo.BankAccount where BankAccountId=1)=0
BEGIN
	SET IDENTITY_INSERT dbo.BankAccount ON
	INSERT	dbo.BankAccount (BankAccountId,Name,BankId,SortCode,AccountNumber,AccountOwner,OpenedDate,PaysTaxableInterest)
	SELECT	1,'Test Account 1',MIN(BankId),'012345','02468804','Mike','2014-01-01',1
	FROM	dbo.Bank
	SET IDENTITY_INSERT dbo.BankAccount OFF
END



IF OBJECT_ID('dbo.Cashflow') IS NULL
BEGIN
PRINT '	Creating [dbo].[Cashflow] ...'
CREATE TABLE [dbo].[Cashflow] 
(
     CashflowId			INT IDENTITY (1, 1) NOT NULL
    ,Name				VARCHAR(100) NOT NULL
	,OpeningBalance		MONEY NOT NULL
	,StartDate			DATE NOT NULL

    ,RecordCreatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    ,RecordUpdatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    
    ,PRIMARY KEY CLUSTERED (CashflowId ASC)
    ,CONSTRAINT UK_Cashflow_Name UNIQUE (Name)
);
PRINT '	Done.'
END


IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TR_Cashflow_UPDATE]'))
DROP TRIGGER [dbo].[TR_Cashflow_UPDATE]
GO
PRINT '	Creating trigger dbo.TR_Cashflow_UPDATE ...'
GO
CREATE TRIGGER dbo.TR_Cashflow_UPDATE
ON dbo.Cashflow
FOR UPDATE
AS
BEGIN
	SET NOCOUNT ON
	UPDATE	a
	SET		RecordUpdatedDateTime=GETDATE()
	FROM	dbo.Cashflow a
		INNER JOIN inserted i
			ON	a.CashflowId=i.CashflowId
END
GO

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


--
alter TABLE [dbo].[Transfer] add CONSTRAINT FK_Transfer_TransferCategory FOREIGN KEY (TransferCategoryId) REFERENCES dbo.TransferCategory(TransferCategoryId)

-- DROP TABLE [dbo].[Transfer] 

IF OBJECT_ID('dbo.Transfer') IS NULL
BEGIN
PRINT '	Creating [dbo].[Transfer] ...'
CREATE TABLE [dbo].[Transfer] 
(
     TransferId			INT IDENTITY (1, 1) NOT NULL
    ,Name				VARCHAR(100) NOT NULL
	,FromBankAccountId	INT NULL
	,ToBankAccountId	INT NULL
	,TransferCategoryId	INT NOT NULL
	,Amount				MONEY NOT NULL
	,AmountTolerence	NUMERIC(3,2) NOT NULL	-- %
	,StartDate			DATE NOT NULL
	,EndDate			DATE NULL
	,Frequency			VARCHAR(100) NOT NULL
	,IsEnabled			BIT NOT NULL

    ,RecordCreatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    ,RecordUpdatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    
    ,PRIMARY KEY CLUSTERED (TransferId ASC)
    ,CONSTRAINT UK_Transfer_Name UNIQUE (Name)
    ,CONSTRAINT FK_Transfer_FromBankAccountId FOREIGN KEY (FromBankAccountId) REFERENCES dbo.BankAccount(BankAccountId)
    ,CONSTRAINT FK_Transfer_ToBankAccountId FOREIGN KEY (ToBankAccountId) REFERENCES dbo.BankAccount(BankAccountId)
	,CONSTRAINT FK_Transfer_TransferCategory FOREIGN KEY (TransferCategoryId) REFERENCES dbo.TransferCategory(TransferCategoryId)
);
PRINT '	Done.'
END
go

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TR_Transfer_UPDATE]'))
DROP TRIGGER [dbo].[TR_Transfer_UPDATE]
GO
PRINT '	Creating trigger dbo.TR_Transfer_UPDATE ...'
GO
CREATE TRIGGER dbo.TR_Transfer_UPDATE
ON dbo.Transfer
FOR UPDATE
AS
BEGIN
	SET NOCOUNT ON
	UPDATE	a
	SET		RecordUpdatedDateTime=GETDATE()
	FROM	dbo.Transfer a
		INNER JOIN inserted i
			ON	a.TransferId=i.TransferId
END
GO


-- DROP TABLE [dbo].[TransferCategory]

IF OBJECT_ID('dbo.TransferCategory') IS NULL
BEGIN
PRINT '	Creating [dbo].[TransferCategory] ...'
CREATE TABLE [dbo].[TransferCategory] 
(
     TransferCategoryId	INT IDENTITY (1, 1) NOT NULL
    ,Code				VARCHAR(20) NOT NULL
    ,Name				VARCHAR(100) NOT NULL
	,DisplayOrder		INT NOT NULL

    ,PRIMARY KEY CLUSTERED (TransferCategoryId ASC)
    ,CONSTRAINT UK_TransferCategory_Name UNIQUE (Name)
);
PRINT '	Done.'
END
go


-- DELETE dbo.TransferCategory WHERE TransferCategoryId=-1

IF NOT EXISTS(SELECT * FROM dbo.TransferCategory WHERE TransferCategoryId=-1)
BEGIN
	SET IDENTITY_INSERT dbo.TransferCategory ON
	INSERT dbo.TransferCategory (TransferCategoryId,Code,Name,DisplayOrder)
	VALUES (-1,'NONE','<None>',0)
	SET IDENTITY_INSERT dbo.TransferCategory OFF
END

MERGE dbo.TransferCategory AS TARGET
USING (	SELECT	*
		FROM	(VALUES	
					('BILL','Bills',1)
					,('SAVE','Savings',2)
					,('INCOME','Income',2)
					,('CC','Credit Card',2)
					) t(Code,Name,DisplayOrder)
	) AS SOURCE
ON	SOURCE.Code=TARGET.Code
WHEN NOT MATCHED THEN
INSERT (Code,Name,DisplayOrder)
VALUES (Code,Name,DisplayOrder)
WHEN MATCHED THEN
UPDATE SET
	Name=SOURCE.Name
	,DisplayOrder=SOURCE.DisplayOrder
	;

SELECT	*
FROM	dbo.TransferCategory
