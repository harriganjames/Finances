use Finance

select	*
from	dbo.bank
order by BankId 

SELECT	*
FROM	dbo.BankAccount
order by BankAccountId desc


--update dbo.bank set Name='first direct' where BankId=1

SELECT	*
FROM	dbo.BankAccount

delete dbo.BankAccount

select	*
from	dbo.Transfer

delete	dbo.Transfer
where	Name like 'xfer%'


insert	dbo.Transfer (Name,FromBankAccountId,ToBankAccountId,Amount,Frequency,AmountTolerence,IsEnabled,StartDate,TransferCategoryId)
values	('Test Transfer',1,2,100,'Monthly',0,1,getdate(),1)

insert	dbo.Transfer (Name,FromBankAccountId,ToBankAccountId,Amount,Frequency,AmountTolerence,IsEnabled,StartDate,TransferCategoryId)
values	('Test Transfer 2',1,2,100,'Monthly',0,1,getdate(),-1)

insert dbo.Cashflow (Name,OpeningBalance,StartDate)
values ('Cashflow 1',100,'2015-02-12')

insert	dbo.CashflowBankAccount(CashflowId,BankAccountId)
values (1,2)




exec GetDataTest

select	*
from	dbo.Cashflow
order by CashflowId desc
select	*
from	dbo.CashflowBankAccount
order by CashflowBankAccountId desc

ALTER TABLE [dbo].[BankAccount] 
ALTER COLUMN AccountOwner   VARCHAR(20) NULL  

select	*
from	dbo.Transfer


exec sp_rename 'dbo.Transfer.FrequencyDays','FrequencyEvery', 'column'




