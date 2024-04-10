-- Drop table Order_products
DROP TABLE IF EXISTS Order_products;

-- Drop table Cart
DROP TABLE IF EXISTS Cart;

-- Drop table Orders
DROP TABLE IF EXISTS Orders;

-- Drop table Discounts
DROP TABLE IF EXISTS Discounts;

-- Drop table Product_attributes
DROP TABLE IF EXISTS Product_attributes;

-- Drop table Products
DROP TABLE IF EXISTS Products;

-- Drop table Users_permission
DROP TABLE IF EXISTS Users_permission;

-- Drop table Users
DROP TABLE IF EXISTS Users;

-- Drop table Product_category_attributes
DROP TABLE IF EXISTS Product_category_attributes;

-- Drop table Product_categories
DROP TABLE IF EXISTS Product_categories;


-- Product categories table
CREATE TABLE Product_categories (
    categoryName VARCHAR(255) PRIMARY KEY
);

-- Product category attributes table
CREATE TABLE Product_category_attributes (
    categoryName VARCHAR(255),
    attributeName VARCHAR(255),
    PRIMARY KEY (categoryName, attributeName),
    FOREIGN KEY (categoryName) REFERENCES Product_categories(categoryName)
);

-- Users table
CREATE TABLE Users (
    id INT PRIMARY KEY IDENTITY,
    userName VARCHAR(255) UNIQUE,
    pasWord VARCHAR(255),
    phoneNumber VARCHAR(20),
    email VARCHAR(255),
    userAddress VARCHAR(255)
);

-- Users permission table
CREATE TABLE Users_permission (
    id INT PRIMARY KEY IDENTITY,
    userId INT,
    permissionType VARCHAR(50),
    FOREIGN KEY (userId) REFERENCES Users(id)
);

-- Products table
CREATE TABLE Products (
    id INT PRIMARY KEY IDENTITY,
    category VARCHAR(255),
    imageUrl VARCHAR(255),
    stockQuantity INT,
    price DECIMAL(10,2),
    stockLocation VARCHAR(255),
    brand VARCHAR(255),
    FOREIGN KEY (category) REFERENCES Product_categories(categoryName)
);

-- Product attributes table
CREATE TABLE Product_attributes (
    id INT PRIMARY KEY IDENTITY,
    productId INT,
    attributeName VARCHAR(255),
    attributeValue VARCHAR(255),
    FOREIGN KEY (productId) REFERENCES Products(id)
);

-- Discounts table
CREATE TABLE Discounts (
    id INT PRIMARY KEY IDENTITY,
    productId INT NULL,
    userId INT NULL,
    percentage INT,
    startDate DATETIME NULL,
    endDate DATETIME NULL
);

-- Orders table
CREATE TABLE Orders (
    id INT PRIMARY KEY IDENTITY,
    orderStatus VARCHAR(255),
    userId INT,
    isPickupOrder BIT NULL,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME,
    FOREIGN KEY (userId) REFERENCES Users(id)
);

-- Cart table
CREATE TABLE Cart(
    userId INT,
    productId INT,
    quantity INT,
    actualPrice DECIMAL(10,2), 
    FOREIGN KEY (userId) REFERENCES Users(id),
    FOREIGN KEY (productId) REFERENCES Products(id),
    PRIMARY KEY (userId, productId)
);

-- Order products table
CREATE TABLE Order_products (
    orderId INT,
    productId INT,
    quantity INT,
    actualPrice DECIMAL(10,2), 
    FOREIGN KEY (orderId) REFERENCES Orders(id),
    FOREIGN KEY (productId) REFERENCES Products(id),
    PRIMARY KEY (orderId, productId)
);



INSERT INTO Product_categories (categoryName)
VALUES 
    ('Graphic Card'),
    ('Keyboard'),
    ('Monitor'),
    ('Mouse'),
    ('PC-Case'),
    ('Processor'),
    ('Storage capacity');

INSERT INTO Product_category_attributes (categoryName, attributeName)
VALUES 
    ('Graphic Card', 'Generation'),
    ('Keyboard', 'Color'),
    ('Monitor', 'Hz'),
    ('Monitor', 'Resolution'),
    ('Monitor', 'Size'),
    ('Mouse', 'Buttons''s number'),
    ('Mouse', 'Color'),
    ('PC-Case', 'Color'),
    ('PC-Case', 'Material'),
    ('Processor', 'Core-CPU'),
    ('Processor', 'RAM'),
    ('Processor', 'Speed'),
    ('Storage capacity', 'GB');


-- Insert data into Products table
INSERT INTO Products (category, imageUrl, stockQuantity, price, stockLocation, brand)
VALUES
    ('Monitor', 'https://www.ivory.co.il/files/catalog/reg/1684749389k89Kr.jpg', 13, 9000.00, 'musherfa', 'LG'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1699536738u38Pe.webp', 9, 1230.00, 'Tell-Aviv', 'Intel'),
    ('Keyboard', 'https://www.ivory.co.il/files/catalog/reg/1699779818f18Jf.webp', 29, 159.00, 'Haifa', 'Ivory Gaming'),
    ('Monitor', 'https://www.ivory.co.il/files/catalog/reg/1712477856j56UE.webp', 12, 1469.00, 'BerSheva', 'Hp'),
    ('Mouse', 'https://www.ivory.co.il/files/catalog/reg/1707386791d91QP.webp', 60, 59.00, 'TellAviv', 'Asus'),
    ('PC-Case', 'https://www.ivory.co.il/files/catalog/reg/1698223944l44Rw.webp', 5, 860.00, 'BeerSheva', 'MSI'),
    ('Processor', 'https://www.ivory.co.il/files/catalog/reg/1697634906k06Js.webp', 130, 2555.00, 'Haifa', 'Intel'),
    ('Storage capacity', 'https://www.ivory.co.il/files/catalog/reg/1707896110t10PH.webp', 0, 155.00, 'Haifa', 'Team Group'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1709115262g62Rz.webp', 3, 3490.00, 'Hifa', 'Zotac'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1698571257y57Tu.webp', 1, 1620.00, 'BeerSheva', 'Zotac'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1686659104o04Hl.jpg', 15, 265.00, 'TellAviv', 'Zotac'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1704371954t54HG.webp', 6, 3220.00, 'BeerSheva', 'MSI'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1702302452s52FB.webp', 9, 9450.00, 'TellAviv', 'MSI'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1668072573p73En.jpg', 0, 9890.00, 'Haifa', 'Asus'),
    ('Graphic Card', 'https://www.ivory.co.il/files/catalog/reg/1699536152g52Wl.webp', 50, 479.00, 'Hifa', 'Asus'),
    ('Keyboard', 'https://www.ivory.co.il/files/catalog/reg/1629636050v50NA.jpg', 8, 225.00, 'BeerSheva', 'Logitech G'),
    ('Keyboard', 'https://www.ivory.co.il/files/catalog/reg/1639394468i68Jf.jpg', 20, 69.00, 'Haifa', 'Lenovo'),
    ('Keyboard', 'https://www.ivory.co.il/files/catalog/reg/1618385028C28OK.jpg', 20, 544.00, 'Haifa', 'Razer'),
    ('Monitor', 'https://www.ivory.co.il/files/catalog/reg/1707654439v39HI.webp', 6, 936.00, 'Haifa', 'HP'),
    ('Monitor', 'https://www.ivory.co.il/files/catalog/reg/1712477856j56UE.webp', 1, 1469.00, 'BeerSheva', 'HP'),
    ('Monitor', 'https://www.ivory.co.il/files/catalog/reg/1711884296A96Sv.webp', 0, 879.00, 'Haifa', 'Samsung'),
    ('Monitor', 'https://www.ivory.co.il/files/catalog/reg/1710674981b81MD.webp', 30, 840.00, 'TellAviv', 'Samsung'),
    ('Monitor', 'https://www.ivory.co.il/files/catalog/reg/1684757538j38DJ.jpg', 4, 1785.00, 'TellAviv', 'LG'),
    ('Mouse', 'https://www.ivory.co.il/files/catalog/reg/1643109560r60Ih.jpg', 16, 107.00, 'TellAviv', 'Logitech G'),
    ('Mouse', 'https://www.ivory.co.il/files/catalog/reg/1623757805k05Ps.jpg', 14, 179.00, 'BeerSheva', 'Logitech'),
    ('Mouse', 'https://www.ivory.co.il/files/catalog/reg/1632314182h82Tu.jpg', 18, 325.00, 'BeerSheva', 'Apple'),
    ('PC-Case', 'https://www.ivory.co.il/files/catalog/reg/1703588262a62FL.webp', 13, 1990.00, 'TellAviv', 'Asus'),
    ('PC-Case', 'https://www.ivory.co.il/files/catalog/reg/1703587909w09KJ.webp', 8, 2190.00, 'TellAviv', 'Asus'),
    ('PC-Case', 'https://www.ivory.co.il/files/catalog/reg/1620563748v48RA.jpg', 0, 652.00, 'Haifa', 'corsair'),
    ('Processor', 'https://www.ivory.co.il/files/catalog/reg/1646053748t48Nt.jpg', 0, 1199.00, 'TellAviv', 'Intel'),
    ('Processor', 'https://www.ivory.co.il/files/catalog/reg/1655361950j50Sp.jpg', 19, 599.00, 'BeerSheva', 'Intel'),
    ('Processor', 'https://www.ivory.co.il/files/catalog/reg/1648984171w71FK.jpg', 8, 480.00, 'BeerSheva', 'Intel'),
    ('Storage capacity', 'https://www.ivory.co.il/files/catalog/reg/1612871620g20KL.jpg', 0, 180.00, 'BeerSheva', 'Samsung'),
    ('Storage capacity', 'https://www.ivory.co.il/files/catalog/reg/1612872986y86Il.jpg', 19, 245.00, 'Haifa', 'Samsung'),
    ('Storage capacity', 'https://www.ivory.co.il/files/catalog/reg/1657546828y28IL.jpg', 16, 325.00, 'Haifa', 'SanDisk'),
    ('Storage capacity', 'https://www.ivory.co.il/files/catalog/reg/1595318137w37Ig.jpg', 6, 179.00, 'Haifa', 'SanDisk');



-- Insert data into Product_attributes table
INSERT INTO Product_attributes (productId, attributeName, attributeValue) VALUES
    (1, 1, 'Hz', '120'),
    (2, 1, 'Resolution', '4K'),
    (3, 1, 'Size', '75inch'),
    (4, 2, 'Generation', 'Intel Arc A750'),
    (5, 3, 'Color', 'Black'),
    (6, 4, 'Hz', '165'),
    (7, 4, 'Resolution', '2560x1440 2k WQHD'),
    (8, 4, 'Size', '31.5"'),
    (9, 5, 'Buttons''s number', '2'),
    (10, 5, 'Color', 'Black'),
    (11, 6, 'Color', 'Black'),
    (12, 6, 'Material', '38 c"m'),
    (13, 7, 'Core-CPU', 'i9-14900KF'),
    (14, 7, 'RAM', '36'),
    (15, 7, 'Speed', '24 Core'),
    (16, 8, 'GB', '512'),
    (17, 9, 'Generation', 'Nvidia GeForce RTX 4070 SUPER'),
    (18, 10, 'Generation', 'Nvidia GeForce RTX 4060'),
    (19, 11, 'Generation', 'Nvidia GeForce GT 710'),
    (20, 12, 'Generation', 'Nvidia GeForce RTX 4070'),
    (21, 13, 'Generation', 'Nvidia GeForce RTX 4090'),
    (22, 14, 'Generation', 'Nvidia GeForce RTX 4090'),
    (23, 15, 'Generation', 'Nvidia GeForce GT 1030'),
    (24, 16, 'Color', 'Black'),
    (25, 17, 'Color', 'Black'),
    (26, 18, 'Color', 'White'),
    (27, 19, 'Hz', '75'),
    (28, 19, 'Resolution', '1920x1080 Full HD'),
    (29, 19, 'Size', '31.5"'),
    (30, 20, 'Hz', '165'),
    (31, 20, 'Resolution', '2560x1440 2k WQHD'),
    (32, 20, 'Size', '31.5"'),
    (33, 21, 'Hz', '165'),
    (34, 21, 'Resolution', '1920x1080 Full HD'),
    (35, 21, 'Size', '23.8"'),
    (36, 22, 'Hz', '100'),
    (37, 22, 'Resolution', '1920x1080 Full HD'),
    (38, 22, 'Size', '27'),
    (39, 23, 'Hz', '165'),
    (40, 23, 'Resolution', '2560x1440 2K WQHD'),
    (41, 23, 'Size', '32"'),
    (42, 24, 'Buttons''s number', '4'),
    (43, 24, 'Color', 'Black'),
    (44, 25, 'Buttons''s number', '2'),
    (45, 25, 'Color', 'White'),
    (46, 26, 'Buttons''s number', '2'),
    (47, 26, 'Color', 'White'),
    (48, 27, 'Color', 'Black'),
    (49, 27, 'Material', 'With Window'),
    (50, 28, 'Color', 'White'),
    (51, 28, 'Material', 'With Window'),
    (52, 29, 'Color', 'Black'),
    (53, 29, 'Material', 'With Window'),
    (54, 30, 'Core-CPU', 'Intel Core i7'),
    (55, 30, 'RAM', '25 MB'),
    (56, 30, 'Speed', '12 Core'),
    (57, 31, 'Core-CPU', 'Intel Core i5'),
    (58, 31, 'RAM', '18 MB'),
    (59, 31, 'Speed', '6 Core'),
    (60, 32, 'Core-CPU', 'Intel Core i3'),
    (61, 32, 'RAM', '12 MB'),
    (62, 32, 'Speed', '4 Core'),
    (63, 33, 'GB', '250'),
    (64, 34, 'GB', '500'),
    (65, 35, 'GB', '1 TB'),
    (66, 36, 'GB', '480');

INSERT INTO Users (userName, pasWord, phoneNumber, email, userAddress) VALUES
('admin', 'admin', '123213123', 'asdsa@asdsad.com', '123213123'),
('customer', 'customer', '123213123', 'asdsa@asdsad.com', '123213123'),
('Guest', 'Guest', '00000000000', 'Guest@Guest.com', 'Guest');

INSERT INTO Users_permission (userId, permissionType) VALUES
(2, 'Customer'),
(1, 'Admin'),
(3, 'Guest');

INSERT INTO Discounts (productId, userId, percentage, startDate, endDate) VALUES
(7, NULL, 10, NULL, NULL),
(8, NULL, 50, NULL, NULL),
(9, NULL, 30, NULL, NULL),
(1, NULL, 10, NULL, NULL);
