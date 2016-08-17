--DROP TABLE [dbo].[BalanceDate]
IF OBJECT_ID('dbo.BalanceDate') IS NULL
BEGIN
PRINT '	Creating [dbo].[BalanceDate] ...'
CREATE TABLE [dbo].[BalanceDate] 
(
     BalanceDateId		INT IDENTITY (1, 1) NOT NULL

    ,DateOfBalance		DATE NOT NULL

    ,RecordCreatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    --,RecordUpdatedDateTime DATETIME NOT NULL DEFAULT(GETDATE())
    
    ,PRIMARY KEY CLUSTERED (BalanceDateId ASC)
    ,CONSTRAINT UK_BalanceDate UNIQUE (DateOfBalance)

);
PRINT '	Done.'
END

