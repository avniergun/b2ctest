	/*M��teri Tablosu*/
	CREATE TABLE dbo.Customer(
		Id				INT IDENTITY(1,1) NOT NULL,
		[Name]			NVARCHAR(50),
		Surname			NVARCHAR(50),
		[Address]		NVARCHAR(300), /*Bu adres sipari� tablosunda tutaca��m�z adres i�inde kullan�labilir. M��terinin birden fazla adresi olursa farkl� tabloda tutulabilir. */
		
		CONSTRAINT PK_Customer PRIMARY KEY (Id)
	);

	/*�r�n Tablosu*/
	CREATE TABLE dbo.Product(
		Id				INT IDENTITY(1,1) NOT NULL,
		Barcode			NVARCHAR(30),
		Quantity		DECIMAL(18,2) NOT NULL,
		Price			DECIMAL(18,2) NOT NULL,
		[Description]	NVARCHAR(500),
		
		CONSTRAINT PK_Product PRIMARY KEY (Id)
	);

	/*Sipari� Tablosu*/
	CREATE TABLE dbo.CustomerOrder(
		Id				INT IDENTITY(1,1) NOT NULL,
		CustomerId		INT NOT NULL,
		OrderCode		NVARCHAR(20),
		DeliveryAddress	NVARCHAR(500), /*M��teri tablosundaki adres bilgisi de�i�ti�inde �nceki sipari�lerde bilgi tutarl�l���n� sa�lamak i�in adresi a��k olarak kaydediyoruz.*/
		OrderAmount		DECIMAL(18,2) NOT NULL,
		OrderDate		DATETIME NOT NULL,
		CreateDate		DATETIME NOT NULL,
		UpdateDate		DATETIME,
		/*Burada �deme, fatura bilgileri, kdv, taksit, ara toplam vb. detaylar tan�mlanabilir. */	
		
		CONSTRAINT PK_CustomerOrder PRIMARY KEY (Id),
		CONSTRAINT FK_CustomerOrder_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
	);

	/*Sipari� Detay- �r�n Bilgileri Tablosu*/
	CREATE TABLE dbo.CustomerOrderDetail(
		Id				INT IDENTITY(1,1) NOT NULL,
		CustomerOrderId	INT NOT NULL,
		ProductId		INT NOT NULL,		
		Quantity		DECIMAL(18,2) NOT NULL,
		Price			DECIMAL(18,2) NOT NULL,	
		TotalAmount		DECIMAL(18,2) NOT NULL,	
		
		CONSTRAINT PK_CustomerOrderDetail PRIMARY KEY (Id),
		CONSTRAINT FK_CustomerOrderDetail_CustomerOrderId FOREIGN KEY (CustomerOrderId) REFERENCES CustomerOrder(Id),
		CONSTRAINT FK_CustomerOrderDetail_ProductId FOREIGN KEY (ProductId) REFERENCES Product(Id)
	);