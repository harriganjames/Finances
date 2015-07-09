
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
