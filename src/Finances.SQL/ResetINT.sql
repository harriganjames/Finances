use FinanceINT

DELETE dbo.BalanceDateBankAccount
DELETE dbo.BalanceDate

DELETE dbo.CashflowBankAccount
DELETE dbo.Cashflow

DELETE dbo.Transfer

DELETE	dbo.BankAccount

DELETE	dbo.Bank


MERGE dbo.TransferCategory AS TARGET
USING (	SELECT	*
		FROM	(VALUES	
					('NONE','<None>',1)
					,('BILL','Bills',1)
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

declare @NoneTransferCategory int = (select TransferCategoryId from dbo.TransferCategory where Code='NONE')

SET IDENTITY_INSERT dbo.Transfer ON
INSERT	dbo.Transfer (TransferId,Name,FromBankAccountId,ToBankAccountId,Amount,AmountTolerence,ScheduleFrequency,ScheduleFrequencyEvery,ScheduleStartDate,IsEnabled,TransferCategoryId)
SELECT	1,'Test Transfer 1',1,2,100.1,0.5,'Monthly',1,'2014-01-01',1,@NoneTransferCategory
SET IDENTITY_INSERT dbo.Transfer OFF


select	*
from	dbo.Bank

select	*
from	dbo.BankAccount

select	*
from	dbo.Transfer

select	*
from	dbo.BalanceDate
select	*
from	dbo.BalanceDateBankAccount

begin tran
delete dbo.BalanceDate where BalanceDateId=10
select	*
from	dbo.BalanceDate
select	*
from	dbo.BalanceDateBankAccount
rollback
