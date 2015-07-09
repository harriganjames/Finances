
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

