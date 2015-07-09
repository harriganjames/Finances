
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

