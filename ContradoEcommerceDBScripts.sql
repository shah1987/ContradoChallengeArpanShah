IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_TYPE = 'PROCEDURE'
			AND ROUTINE_NAME = 'uspAddEditProduct'
			AND SPECIFIC_SCHEMA = 'dbo'
		)
BEGIN
	DROP PROCEDURE [dbo].[uspAddEditProduct]
END
GO

-- =============================================
-- Author:		Arpan Shah
-- Create date: 29 Jan 2022
-- Description:	Add/Edit Product
-- EXEC [dbo].[uspAddEditProduct] 1
-- =============================================
CREATE PROCEDURE [dbo].[uspAddEditProduct] @ProductId INT
	,@ProdCatId INT
	,@AttributeId INT
	,@ProdName VARCHAR(250)
	,@ProdDescription VARCHAR(MAX)
	,@AttibuteValue VARCHAR(250)
AS
BEGIN
	BEGIN TRAN

	BEGIN TRY
		IF (@ProductID = 0)
		BEGIN
			INSERT INTO dbo.Product (
				ProdCatId
				,ProdName
				,ProdDescription
				)
			VALUES (
				@ProdCatId
				,@ProdName
				,@ProdDescription
				)

			SET @ProductId = @@IDENTITY

			INSERT INTO dbo.ProductAttribute (
				ProductId
				,AttributeId
				,AttributeValue
				)
			VALUES (
				@ProductId
				,@AttributeId
				,@AttibuteValue
				)
		END
		ELSE
		BEGIN
			UPDATE dbo.Product
			SET ProdCatId = @ProdCatId
				,ProdName = @ProdName
				,ProdDescription = @ProdDescription
			WHERE ProductId = @ProductId

			IF EXISTS (
					SELECT TOP 1 1
					FROM ProductAttribute
					WHERE ProductId = @ProductId
					)
			BEGIN
				UPDATE dbo.ProductAttribute
				SET AttributeId = @AttributeId
					,AttributeValue = @AttibuteValue
				WHERE ProductId = @ProductId
			END
			ELSE
			BEGIN
				INSERT INTO dbo.ProductAttribute (
					ProductId
					,AttributeId
					,AttributeValue
					)
				VALUES (
					@ProductId
					,@AttributeId
					,@AttibuteValue
					)
			END
		END

		COMMIT TRAN

		SELECT @ProductId
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN
		END

		SELECT @ProductID
	END CATCH
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_TYPE = 'PROCEDURE'
			AND ROUTINE_NAME = 'uspDeleteProduct'
			AND SPECIFIC_SCHEMA = 'dbo'
		)
BEGIN
	DROP PROCEDURE [dbo].[uspDeleteProduct]
END
GO

-- =============================================
-- Author:		Arpan Shah
-- Create date: 29 Jan 2022
-- Description:	Delete Product
-- EXEC [dbo].[uspDeleteProduct] 1
-- =============================================
CREATE PROCEDURE [dbo].[uspDeleteProduct] @ProductId INT
AS
BEGIN
	BEGIN TRAN

	BEGIN TRY
		DELETE
		FROM dbo.ProductAttribute
		WHERE ProductId = @ProductId

		DELETE
		FROM dbo.Product
		WHERE ProductId = @ProductId

		COMMIT TRAN

		SELECT @ProductId
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN
		END

		SELECT @ProductID
	END CATCH
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_TYPE = 'PROCEDURE'
			AND ROUTINE_NAME = 'uspGetAllProducts'
			AND SPECIFIC_SCHEMA = 'dbo'
		)
BEGIN
	DROP PROCEDURE [dbo].[uspGetAllProducts]
END
GO

-- =============================================
-- Author:		Arpan Shah
-- Create date: 29 Jan 2022
-- Description:	Get Products with Paging
-- EXEC [dbo].[uspGetAllProducts] 1, 10
-- =============================================
CREATE PROCEDURE [dbo].[uspGetAllProducts] @PageIndex INT
	,@PageSize INT
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT P.ProductId
		,P.ProdCatId
		,PA.AttributeId
		,P.ProdName
		,P.ProdDescription
		,PC.CategoryName
		,ISNULL(PAL.AttributeName, '') AS AttributeName
		,COUNT(P.ProductId) OVER () TotalRecords
	FROM dbo.Product AS P
	INNER JOIN dbo.ProductCategory AS PC ON PC.ProdCatId = P.ProdCatId
	LEFT JOIN dbo.ProductAttribute AS PA ON PA.ProductId = P.ProductId
	LEFT JOIN dbo.ProductAttributeLookup AS PAL ON PAL.AttributeId = PA.AttributeId
		AND PAL.ProdCatId = PC.ProdCatId
	ORDER BY P.ProdName OFFSET(@PageIndex - 1) * @PageSize ROWS

	FETCH NEXT @PageSize ROWS ONLY
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_TYPE = 'PROCEDURE'
			AND ROUTINE_NAME = 'uspGetProductAttribute'
			AND SPECIFIC_SCHEMA = 'dbo'
		)
BEGIN
	DROP PROCEDURE [dbo].[uspGetProductAttribute]
END
GO

-- =============================================
-- Author:		Arpan Shah
-- Create date: 29 Jan 2022
-- Description:	Get Products Attribute
-- EXEC [dbo].[uspGetProductAttribute] 1
-- =============================================
CREATE PROCEDURE [dbo].[uspGetProductAttribute] @ProductCatID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT PAL.AttributeId
		,PAL.ProdCatId
		,PAL.AttributeName
	FROM dbo.ProductAttributeLookup AS PAL
	WHERE PAL.ProdCatId = @ProductCatID
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_TYPE = 'PROCEDURE'
			AND ROUTINE_NAME = 'uspProductByID'
			AND SPECIFIC_SCHEMA = 'dbo'
		)
BEGIN
	DROP PROCEDURE [dbo].[uspProductByID]
END
GO

-- =============================================  
-- Author:  Arpan Shah  
-- Create date: 29 Jan 2022  
-- Description: Get Products By ID  
-- EXEC [dbo].[uspProductByID] 1  
-- =============================================  
CREATE PROCEDURE [dbo].[uspProductByID] @ProductID BIGINT
AS
BEGIN
	SELECT P.ProductId
		,P.ProdCatId
		,PA.AttributeId
		,P.ProdName
		,P.ProdDescription
		,PC.CategoryName
		,ISNULL(PAL.AttributeName, '') AS AttributeName
	FROM dbo.Product AS P
	INNER JOIN dbo.ProductCategory AS PC ON PC.ProdCatId = P.ProdCatId
	LEFT JOIN dbo.ProductAttribute AS PA ON PA.ProductId = P.ProductId
	LEFT JOIN dbo.ProductAttributeLookup AS PAL ON PAL.AttributeId = PA.AttributeId
		AND PAL.ProdCatId = PC.ProdCatId
	WHERE P.ProductID = @ProductID
END
GO

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.ROUTINES
		WHERE ROUTINE_TYPE = 'PROCEDURE'
			AND ROUTINE_NAME = 'uspProductCategory'
			AND SPECIFIC_SCHEMA = 'dbo'
		)
BEGIN
	DROP PROCEDURE [dbo].[uspProductCategory]
END
GO

-- =============================================
-- Author:		Arpan Shah
-- Create date: 29 Jan 2022
-- Description:	Get Products Category
-- EXEC [dbo].[uspProductCategory]
-- =============================================
CREATE PROCEDURE [dbo].[uspProductCategory]
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT PC.ProdCatId
		,PC.CategoryName
	FROM dbo.ProductCategory AS PC
END
