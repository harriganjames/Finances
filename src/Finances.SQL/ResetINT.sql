use FinanceINT

DELETE dbo.CashflowBankAccount
DELETE dbo.Cashflow

DELETE dbo.Transfer

DELETE	dbo.BankAccount

DELETE	dbo.Bank

SET IDENTITY_INSERT dbo.Bank ON
INSERT	dbo.Bank (BankId,Name,Logo)
VALUES	
 (1,'first direct',0x010203)
,(2,'hsbc',0x01020304)
SET IDENTITY_INSERT dbo.Bank OFF

SET IDENTITY_INSERT dbo.BankAccount ON
INSERT	dbo.BankAccount (BankAccountId,Name,BankId,SortCode,AccountNumber,AccountOwner,OpenedDate,PaysTaxableInterest)
SELECT	1,'Test Account 1',1,'012345','02468804','Mike','2014-01-01',1
UNION
SELECT	2,'Test Account 2',2,'012345','02468804','Mike','2014-01-01',1
UNION
SELECT	3,'Test Account 3',2,'222222','33333333','Mike','2014-02-01',1
SET IDENTITY_INSERT dbo.BankAccount OFF

SET IDENTITY_INSERT dbo.Transfer ON
INSERT	dbo.Transfer (TransferId,Name,FromBankAccountId,ToBankAccountId,Amount,AmountTolerence,Frequency,StartDate,IsEnabled)
SELECT	1,'Test Transfer 1',1,2,100.1,0.5,'Monthly','2014-01-01',1
SET IDENTITY_INSERT dbo.Transfer OFF


select	*
from	dbo.Bank

select	*
from	dbo.BankAccount

select	*
from	dbo.Transfer
