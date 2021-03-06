CREATE TRIGGER Orders_UPDATE
ON dbo.Orders
AFTER UPDATE
AS
DECLARE @SUM MONEY;
DECLARE @AMOUNT MONEY;
SET @SUM = (SELECT toPayment FROM deleted) - (SELECT toPayment FROM inserted)
SET @AMOUNT = (SELECT amount FROM dbo.Money WHERE moneyId = (SELECT max(moneyId) FROM dbo.Money)) - @SUM
INSERT INTO dbo.Money(date, sum, amount)
VALUES (GETDATE(), -@SUM, @AMOUNT)
INSERT INTO dbo.Payment (orderId, moneyId, sum)
VALUES ((SELECT orderId FROM inserted) ,(SELECT max(moneyId) FROM dbo.Money),@SUM)