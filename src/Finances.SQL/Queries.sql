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

select * from dbo.Bank

delete	dbo.BankAccount
where	AccountNumber='00000000'

go
declare @qty int = 100
declare @loop int = 1
declare @name varchar(100)

while(@loop<=@qty)
begin
	select @name='Testing Account '+convert(varchar,@loop)
	print @name

	INSERT	dbo.BankAccount (Name,BankId,SortCode,AccountNumber,AccountOwner,OpenedDate,PaysTaxableInterest)
	SELECT	@name,1,'000000','00000000','Mike','2014-01-01',1

	set @loop+=1
end



select	c.CashflowId, c.Name, count(*)
from	dbo.Cashflow c
inner join dbo.CashflowBankAccount cba
	on	c.CashflowId=cba.CashflowId
group by c.CashflowId, c.Name

select	*
from	dbo.BankAccount


insert	dbo.BalanceDate(DateOfBalance) values ('2016-08-10')

insert	dbo.BalanceDateBankAccount(BalanceDateId,BankAccountId,BalanceAmount) values (1,2,200)
insert	dbo.BalanceDateBankAccount(BalanceDateId,BankAccountId,BalanceAmount) values (1,3,300)

select	*
from	dbo.BalanceDate

select	*
from	dbo.BalanceDateBankAccount

