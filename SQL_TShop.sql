IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TShop')
BEGIN
  CREATE DATABASE TShop;
END;
GO

USE [TShop]
GO


-- Create table Category
CREATE TABLE Category (
    IdCategory INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);
GO

-- Create table Brands
CREATE TABLE Brands (
   IdBrands INT IDENTITY(1,1) PRIMARY KEY,
   Name NVARCHAR(50) NOT NULL,
   ImageBrands NVARCHAR(50) NULL,
);
GO

-- Create table Product
CREATE TABLE Product (
    IdProduct INT IDENTITY(1,1) PRIMARY KEY,
    IdBrands INT NOT NULL,
    IdCategory INT NOT NULL,
    NameProduct NVARCHAR(50) NOT NULL,
    Description NVARCHAR(max) NOT NULL,   
    Price DECIMAL(18,2) NULL,
    Image NVARCHAR(50) NULL,
    ImageDetailOne NVARCHAR(50) NULL,
    ImageDetailTwo NVARCHAR(50) NULL,
    FOREIGN KEY (IdBrands) REFERENCES Brands(IdBrands),
    FOREIGN KEY (IdCategory) REFERENCES Category(IdCategory),
);
GO

-- Create table Invoice
CREATE TABLE Invoice (
    IdInvoice INT IDENTITY(1,1) PRIMARY KEY,
    IdCustomer NVARCHAR(20) NOT NULL,
    OrderDate DATETIME NOT NULL,
    DeliveryDate DATETIME NULL,
    Name NVARCHAR(50) NULL,
    Address NVARCHAR(60) NOT NULL,
    Phone NVARCHAR(24) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,
    StatusCode INT NOT NULL,
    Note NVARCHAR(50) NULL
);
GO

-- Create table Customer
CREATE TABLE Customer (
    IdCustomer INT IDENTITY(1,1) PRIMARY KEY,
    PassWord NVARCHAR(50) NULL,
    FullName NVARCHAR(50) NOT NULL,
    Sex BIT NOT NULL,
    BirthDay DATETIME NOT NULL,
    Address NVARCHAR(60) NULL,
    Phone NVARCHAR(24) NULL,
    Email NVARCHAR(50) NOT NULL,
    Effect BIT NOT NULL,
    RandomKey VARCHAR(50) NULL
);
GO

-- Create table Invoice Detail
CREATE TABLE InvoiceDetail (
    IdInvoiceDetail INT IDENTITY(1,1) PRIMARY KEY,
    IdInvoice INT NOT NULL,
    IdProduct INT NOT NULL,
	IdCustomer INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (IdInvoice) REFERENCES Invoice(IdInvoice),
    FOREIGN KEY (IdProduct) REFERENCES Product(IdProduct),
	FOREIGN KEY (IdCustomer) REFERENCES Customer(IdCustomer)
);
GO

-- Insert data Brands
--ASUS, HP , Lenovo, Acer, Dell, MSI, LG, Apple, Samsung, Xiaomi, Huawei

INSERT INTO Brands (Name, ImageBrands) VALUES ('ASUS', 'Brands_ASUS.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('HP', 'Brands_HP.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('Dell', 'Brands_Dell.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('MSI', 'Brands_MSI.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('LG', 'Brands_LG.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('Apple', 'Brands_Apple.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('Samsung', 'Brands_Samsung.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('Xiaomi', 'Brands_Xiaomi.jpg');
INSERT INTO Brands (Name, ImageBrands) VALUES ('Huawei', 'Brands_Huawei.jpg');

-- Insert data Category
-- Tablet, Telephone, Laptop, Smartwatch
INSERT INTO Category (Name) VALUES ('Telephone');
INSERT INTO Category (Name) VALUES ('Laptop');
INSERT INTO Category (Name) VALUES ('Smartwatch');
INSERT INTO Category (Name) VALUES ('Tablet');


select * from Category
select * from Brands

-- Insert data Product
-- ASUS
INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (1, 1, 'ASUS ROG Phone 7 Ultimate 16GB 512GB', 'Asus ROG phone 7 Ultimate 16GB 512GB owns Snapdragon 8 Gen 2 chip with super power from Qualcomm. The screen is made from an amoled screen with a huge size of 6.78 inches for Full HD Plus image quality. Super quality camera with resolution up to 50MP comes with an unrivaled 6000mAh battery and 65W HyperCharge charging mode.', 26190000, 'Telephone_ASUS1.jpg', 'Telephone_ASUS2.jpg', 'Telephone_ASUS3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (1, 2, 'Laptop Asus VivoBook 14 OLED A1405VA KM095W', 'Shine to the world with the Asus VivoBook 14 OLED Laptop A1405VA-KM095W, a full-featured laptop with an extremely bright OLED HDR screen with cinematic DCI-P3 color range. Asus VivoBook 14 OLED Laptop A1405VA-KM095W lets you get things done easily, anytime, anywhere: every aspect is improved, from the powerful 13th generation Intel® mobile processor to the 180-degree hinge °, slim geometric design and modern colors.', 16390000, 'Laptop_ASUS1.jpg', 'Laptop_ASUS2.jpg', 'Laptop_ASUS3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (1, 2, 'Laptop Gaming ASUS ROG Zephyrus Duo 16', 'ASUS ROG Zephyrus Duo 16 GX650PZ-NM031W Gaming Laptop with exclusive second screen called ROG ScreenPad Plus™, world most powerful AMD Ryzen™ 9 7945HX CPU, NVIDIA® GeForce RTX™ 4080 GPU with TGP up to 175W, ROG Zephyrus Duo 16 delivers the highest performance and smoothest multitasking experience on the Windows 11 Home platform. The Mini LED panel screen on ROG Zephyrus Duo 16 provides deeper contrast and higher brightness than traditional LED screens. To ensure the system is always cool and stable, ROG Zephyrus Duo 16 is equipped with AAS Plus 2.0 technology with large air intakes up to 28.5 mm and Conductonaut Extreme liquid metal thermal paste from Thermal Grizzly.', 103990000, 'Laptop_ASUSDuo1.jpg', 'Laptop_ASUSDuo1.jpg', 'Laptop_ASUSDuo1.jpg');

-- HP
INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (2, 2, 'Laptop Gaming HP Omen 16 Xf0071AX 8W945PA', 'With the latest generation Intel Core Ryzen 7 powerful processor, optimized RTX 4000 graphics card and spacious memory capacity, the HP Omen 16-xf0071AX 8W945PA Gaming Laptop delivers a smooth and enjoyable gaming experience, ensures that you ll never have to worry about whether your computer has enough power to handle all the games demands, no matter where you are in the world.', 62490000, 'Laptop_HP1.jpg', 'Laptop_HP2.jpg', 'Laptop_HP3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (2, 2, 'Laptop Gaming HP VICTUS 16 R0128TX 8C5N3PA', 'HP VICTUS 16-r0128TX 8C5N3PA Gaming Laptop Take your creative projects to the next level with NVIDIA Studio. Unleash RTX and AI acceleration for leading creative applications, the most stable state with NVIDIA Studio drivers, and quickly bring creative ideas to life with the toolset monopoly.', 31990000, 'Laptop_HPVICTUS1.jpg', 'Laptop_HPVICTUS2.jpg', 'Laptop_HPVICTUS3.jpg');

-- Dell
INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (3, 2, 'Laptop gaming Dell G15 5530 i7H165W11GR4060', 'HP VICTUS 16-r0128TX 8C5N3PA Gaming Laptop Take your creative projects to the next level with NVIDIA Studio. Unleash RTX and AI acceleration for leading creative applications, the most stable state with NVIDIA Studio drivers, and quickly bring creative ideas to life with the toolset monopoly.', 31990000, 'Laptop_Dell1.jpg', 'Laptop_Dell2.jpg', 'Laptop_Dell3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (3, 2, 'Laptop gaming Dell Alienware M15 R6 P109F001CBL', 'Equipped with the 11th generation Intel Core i7 chip - i7-11800H, Dell Alienware M15 R6 P109F001CBL provides lightning-fast processing capabilities. Combined with the RTX 3060 graphics card, the RTX 30 series cards possess the technologies that promote the best image processing performance today such as DLSS, Dynamic Boost 2.0 and Tensor core, for image processing and video editing. , enjoying AAA games will be extremely smooth.', 31990000, 'Laptop_DellAlienware1.jpg', 'Laptop_DellAlienware2.jpg', 'Laptop_DellAlienware3.jpg');

-- MSI
INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (4, 2, 'Laptop Gaming MSI Bravo 15 C7VFK 275VN', 'NVIDIA® GeForce RTX™ Series 40 GPU for Gaming Laptop MSI Bravo 15 C7VFK 275VN provides high performance for gaming and creative work on laptops. With Ada Lovelace architecture, they support DLSS 3 and full ray tracing, creating high quality images. The Max-Q technology suite also ensures optimal performance, energy savings and sound. These GPUs are ideal for graphics-intensive tasks like gaming and video editing. However, you need to consider the specific configuration of each laptop to ensure it suits your needs. Also check compatibility with the software and games you plan to use.', 44990000, 'Laptop_MSI1.jpg', 'Laptop_MSI2.jpg', 'Laptop_MSI3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (4, 2, 'Laptop Gaming MSI Raider GE68 HX 13VG 048VN', 'MSI Raider GE68 HX 13VG 048VN Gaming Laptop combines luxury, advanced technology and top performance, MSI Raider GE68 HX 13VG 048VN Gaming Laptop is the pinnacle of gaming laptops. With the latest 13th generation 2023 Intel ® Core ™ i7-13700HX processor, it has the strongest performance at the moment. Packed with the monster power of the NVIDIA® GeForce RTX™ 4070 graphics card. Luxurious aesthetics delivered by Mystic Light. MSI Raider GE68 HX 13VG 048VN Gaming Laptop regenerates the most powerful gaming laptop.', 44990000, 'Laptop_MSIRaider1.jpg', 'Laptop_MSIRaider2.jpg', 'Laptop_MSIRaider3.jpg');

-- LG
INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (5, 2, 'Laptop LG Gram 2023 16Z90R EAH75A5', 'MSI Raider GE68 HX 13VG 048VN Gaming Laptop combines luxury, advanced technology and top performance, MSI Raider GE68 HX 13VG 048VN Gaming Laptop is the pinnacle of gaming laptops. With the latest 13th generation 2023 Intel ® Core ™ i7-13700HX processor, it has the strongest performance at the moment. Packed with the monster power of the NVIDIA® GeForce RTX™ 4070 graphics card. Luxurious aesthetics delivered by Mystic Light. MSI Raider GE68 HX 13VG 048VN Gaming Laptop regenerates the most powerful gaming laptop.', 46990000, 'Laptop_LG1.jpg', 'Laptop_LG2.jpg', 'Laptop_LG3.jpg');

-- Apple
INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (6, 2, 'Laptop Apple MacBook Pro 14 inch M3 Pro 18GB/512GB', 'The Apple M3 Pro chip represents a turning point in performance and energy efficiency. With 3 nm manufacturing technology and over 37 billion transistors, it provides superior performance and long battery life. This chip perfectly combines a multi-core CPU and a powerful GPU, especially optimized for tasks that require excellence in graphics, programming and creativity. Possessing the super power of Apple silicon chip with 11 CPU cores provides remarkable performance for multi-threaded tasks and work that requires fast processing. Especially useful for programmers who work with demanding applications such as video making, graphic engineering, creative industry related tasks.', 49490000, 'Laptop_MacBook1.jpg', 'Laptop_MacBook2.jpg', 'Laptop_MacBook3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (6, 1, 'iPhone 15 Pro Max 256GB', 'iPhone 15 Pro Max will continue to be a phone with a typical flat screen and back from Apple, bringing elegant and luxurious beauty. The main material of the iPhone 15 Pro Max is still the metal frame and tempered glass back, creating durability and sturdiness. However, with advanced technology, this frame has been upgraded to titanium material instead of stainless steel or aluminum in previous generations.', 29890000, 'Telephone_iPhone1.jpg', 'Telephone_iPhone2.jpg', 'Telephone_iPhone3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (6, 3, 'Apple Watch Ultra 2 GPS', 'Apple Watch Ultra 2 GPS + Cellular 49mm with Titanium rim and Ocean strap is Apple smartwatch that attracted a lot of attention from the media and technology lovers at the Wonderlust event in 2023. The watch has a trendy appearance. The upper is both extremely sporty and unique, and the internal features also have improvements that promise to satisfy users expectations.', 21990000, 'Watch_Apple1.jpg', 'Watch_Apple2.jpg', 'Watch_Apple3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (6, 4, 'iPad Pro M2 12.9 inch WiFi Cellular 128GB', 'Apple is increasingly outpacing many competitors in terms of performance on tablet lines, this is specifically demonstrated by the appearance of the super powerful Apple M2 chip on iPad Pro M2 12.9 inch WiFi Cellular 128GB. Besides, the device also has upgrades such as iPadOS 16 operating system, 40.88 Wh battery.', 21990000, 'Table_Apple1.jpg', 'Table_Apple2.jpg', 'Table_Apple3.jpg');

-- Samsung
INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (7, 1, 'Samsung Galaxy S24 Ultra 5G 256GB', 'Samsung Galaxy S24 Ultra high-end phone model was launched in early 2024, the product continues to inherit and improve from the previous generation. The special feature is the use of Snapdragon 8 Gen 3 for Galaxy chip, 200 MP camera and integrated many AI features.', 29690000, 'Telephone_Samsung1.jpg', 'Telephone_Samsung2.jpg', 'Telephone_Samsung3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (7, 3, 'Samsung Galaxy Watch5 Pro LTE 45mm', 'Apple Watch Ultra 2 GPS + Cellular 49mm with Titanium rim and Ocean strap is Apples smartwatch that attracted a lot of attention from the media and technology lovers at the Wonderlust event in 2023. The watch has a trendy appearance. The upper is both extremely sporty and unique, and the internal features also have improvements that promise to satisfy users expectations.', 10990000, 'Watch_Samsung1.jpg', 'Watch_Samsung2.jpg', 'Watch_Samsung3.jpg');

INSERT INTO Product (IdBrands, IdCategory, NameProduct, Description, Price, Image, ImageDetailOne, ImageDetailTwo) 
VALUES (7, 4, 'Samsung Galaxy Tab S9+ 5G 256GB', 'The Galaxy Tab S9+ 5G has the design of its predecessor, the Galaxy Tab S8+, but the difference is in the rear camera cluster and long bezels, which are blurred to create a softer feeling. Galaxy Tab S9+ 5G is finished in one piece, providing solidity and elegance when held and used. At the same time, Samsung has optimized and refined the front, making the edges thinner, providing a wide viewing angle for easy viewing.', 24990000, 'Table_Samsung1.jpg', 'Table_Samsung2.jpg', 'Table_Samsung3.jpg');

-- Xiaomi
-- Huawei
