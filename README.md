-- Creat Tabels

CREATE TABLE Users (
    id INT PRIMARY KEY IDENTITY,
    userName VARCHAR(255) UNIQUE,
	pasWord VARCHAR(255),
    phoneNumber VARCHAR(20),
    email VARCHAR(255),
    userAddress VARCHAR(255)
);

CREATE TABLE Users_permission (
    id INT PRIMARY KEY IDENTITY,
    userId INT,
    permissionType VARCHAR(50),
    FOREIGN KEY (userId) REFERENCES Users(id)
);

CREATE TABLE ref_data (
    refName VARCHAR(255),
	refValue VARCHAR(255),
	PRIMARY KEY (refName, refValue)
);

CREATE TABLE Products (
    id INT PRIMARY KEY IDENTITY ,
    category VARCHAR(255),
    imageUrl VARCHAR(255),
    stockQuantity INT,
    price DECIMAL(10,2),
    stockLocation VARCHAR(255),
	brand VARCHAR(255),
    FOREIGN KEY (category) REFERENCES Product_categories(categoryName)
);

CREATE TABLE Product_category_attributes (
    categoryName VARCHAR(255),
    attributeName VARCHAR(255),
    PRIMARY KEY (categoryName, attributeName), -- Composite primary key
    FOREIGN KEY (categoryName) REFERENCES Product_categories(categoryName)
);

CREATE TABLE Product_categories (
    categoryName VARCHAR(255)PRIMARY KEY,  
	-- monitors/ PcCasess/ Proccesor/ Mouse/ KeyBoard/ GraphicCard 
);

CREATE TABLE Product_attributes (
    id INT PRIMARY KEY IDENTITY ,
    productId INT,
    attributeName VARCHAR(255),  -- size
    attributeValue VARCHAR(255), -- 50 inch
    FOREIGN KEY (productId) REFERENCES Products(id)
);

CREATE TABLE Orders (
    id INT PRIMARY KEY IDENTITY,
    orderStatus VARCHAR(255),
    userId INT,
	isPickupOrder BIT NULL,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME,
    FOREIGN KEY (userId) REFERENCES Users(id)
);

CREATE TABLE Order_products (
    orderId INT,
    productId INT,
	quanity INT,
	actaulPrice  DECIMAL(10,2), 
	FOREIGN KEY (orderId) REFERENCES Orders(id),
	FOREIGN KEY (productId) REFERENCES Products(id),
	PRIMARY KEY (orderId, productId)
);

CREATE TABLE Discounts (
    id INT PRIMARY KEY IDENTITY,
    productId INT NULL,
	userId INT NULL,
    percentage int,
    startDate DATETIME NULL,
    endDate DATETIME NULL
);

CREATE TABLE Cart(
    userId INT,
    productId INT,
	quantity INT,
	actualPrice  DECIMAL(10,2), 
	FOREIGN KEY (userId) REFERENCES Users(id),
	FOREIGN KEY (productId) REFERENCES Products(id),
	PRIMARY KEY (userId, productId)
);

CREATE TABLE Payments (
id INT PRIMARY KEY IDENTITY,
  orderId INT,
   userId INT,
   ammount  DECIMAL(10,2),
   --encrypted CC todo
   FOREIGN KEY (orderId) REFERENCES Orders(id),
	FOREIGN KEY (userId) REFERENCES Users(id),
);

CREATE TABLE Shipment_adresses (
id INT PRIMARY KEY IDENTITY,
  orderId INT,
   userId INT,
   --
   --add more shipment fileds
   FOREIGN KEY (orderId) REFERENCES Orders(id),
	FOREIGN KEY (userId) REFERENCES Users(id),
);






















































