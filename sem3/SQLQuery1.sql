create database OnlineCatere;
use OnlineCatere;
-- Table structure for table 'Acity'
CREATE TABLE Acity (
  ACityId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  CityName varchar(100) DEFAULT NULL,
  ParentId int DEFAULT NULL,
  Flag bit DEFAULT 0
);

-- Table structure for table 'Category'
CREATE TABLE Category (
  CategoryId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  Name varchar(50) DEFAULT NULL,
  ParentId int DEFAULT NULL,
  Flag bit DEFAULT 0
);

-- Table structure for table 'Customer'
CREATE TABLE Customer (
  CustomerId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  FirstName nvarchar(30) DEFAULT NULL,
  MiddleName nvarchar(30) DEFAULT NULL,
  LastName nvarchar(30) DEFAULT NULL,
  Gender varchar(20) DEFAULT NULL,
  BirthOfDate datetime DEFAULT NULL,
  PhoneNumber varchar(10) DEFAULT NULL,
  Email varchar(100) DEFAULT NULL,
  Address varchar(500) DEFAULT NULL,
  UrlImage varchar(255) null,
  TypeCustomer varchar(50) DEFAULT NULL,
  CLoginName varchar(50) DEFAULT NULL,
  Password varchar(255) DEFAULT NULL,
  Flag bit DEFAULT 0
);
CREATE TABLE Supplier (
  SupplierId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  SName varchar(50) DEFAULT NULL,
  PhoneNumber varchar(10) DEFAULT NULL,
  Address varchar(500) DEFAULT NULL,
  Email varchar(100) DEFAULT NULL,
  SLevel varchar(30) DEFAULT NULL,
  ACityId int DEFAULT NULL,
  UrlImage varchar(255) DEFAULT NULL,
  SLogin varchar(50) Default null,
  Password varchar(255) ,
  Flag bit DEFAULT 0,
  FOREIGN KEY (ACityId) REFERENCES Acity (ACityId)
);

CREATE TABLE Service (
  ServiceId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  ServiceName varchar(100) NOT NULL,
  Description varchar(500) DEFAULT NULL,
  SupplierId int NOT NULL,
  Flag bit DEFAULT 0,
  FOREIGN KEY (SupplierId) REFERENCES Supplier (SupplierId) -- Liên kết với bảng Supplier

);
-- Table structure for table 'Room'
CREATE TABLE Room (
  RoomId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  RoomName varchar(50) DEFAULT NULL,
  Capacity int DEFAULT NULL,
  RoomPrice float DEFAULT NULL,
  ServiceId int DEFAULT NULL,
   Flag bit DEFAULT 0,
  CONSTRAINT FK_Room_Service FOREIGN KEY (ServiceId) REFERENCES Service(ServiceId)
);
-- Table structure for table 'SuppMenu'
CREATE TABLE SuppMenu (
  MenuItemId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  ItemName varchar(50) DEFAULT NULL,
  Price float DEFAULT NULL,
  CategoryId int DEFAULT NULL,
  SupplierId int DEFAULT NULL,
  UrlImage varchar(255) DEFAULT NULL,
  Flag bit DEFAULT 0,
  FOREIGN KEY (SupplierId) REFERENCES Supplier (SupplierId),
  FOREIGN KEY (CategoryId) REFERENCES Category (CategoryId)
);
-- Table structure for table 'SuppDetail'
CREATE TABLE SuppDetail (
  SuppDetailId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  SupplierId int DEFAULT NULL,
  NameDetail varchar(50) DEFAULT NULL,
  NumPeople int DEFAULT NULL,
  CustomerCost float DEFAULT NULL,
  SupplierCost float DEFAULT NULL,
  Flag bit DEFAULT 0,
  FOREIGN KEY (SupplierId) REFERENCES Supplier (SupplierId)
);
-- Table structure for table 'SuppInvoice'
CREATE TABLE SuppInvoice (
  SupInvoiceId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  SupInvoiceDate datetime DEFAULT NULL,
  SupplierId int DEFAULT NULL,
  ListRoom varchar(50) DEFAULT NULL,
  PersonInvoice varchar(50) DEFAULT NULL,
  Flag bit DEFAULT 0,
  FOREIGN KEY (SupplierId) REFERENCES Supplier (SupplierId)
);
-- Table structure for table 'CustOderSupp'
CREATE TABLE CustOderSupp (
  CustOderSuppId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  CustomerId int DEFAULT NULL,
  RoomId int DEFAULT NULL,
  DeliveryDate datetime DEFAULT NULL,
  VAT float DEFAULT NULL,
  Status varchar(100) DEFAULT NULL,
  NumPeople int DEFAULT NULL,
  Flag bit DEFAULT 0,
  FOREIGN KEY (CustomerId) REFERENCES Customer (CustomerId),
  FOREIGN KEY (RoomId) REFERENCES Room (RoomId)
);
CREATE TABLE CustInvoice (
  InvoiceId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  InvoiceDate datetime DEFAULT NULL,
  CustOderSuppId int DEFAULT NULL,
  CustomerId int DEFAULT NULL,
  RoomId int DEFAULT NULL,
  VAT float DEFAULT NULL,
  ListRoom varchar(50) DEFAULT NULL,
  Flag bit DEFAULT 0,
  FOREIGN KEY (CustomerId) REFERENCES Customer (CustomerId),
  FOREIGN KEY (RoomId) REFERENCES Room (RoomId),
  FOREIGN KEY (CustOderSuppId) REFERENCES CustOderSupp (CustOderSuppId)
);
-- Table structure for table 'CustOderMenu'
CREATE TABLE CustOderMenu (
  CustOderMenuId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  MenuItemId int DEFAULT NULL,
  RoomId int DEFAULT NULL,
  CustOderSuppId int DEFAULT NULL,
  Flag bit DEFAULT 0,
  FOREIGN KEY (MenuItemId) REFERENCES SuppMenu (MenuItemId),
  FOREIGN KEY (RoomId) REFERENCES Room (RoomId),
  FOREIGN KEY (CustOderSuppId) REFERENCES CustOderSupp (CustOderSuppId)
);

CREATE TABLE CustomerFeedback (
  FeedbackId int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
  SupplierId int NOT NULL,
  CustomerId int NOT NULL,
  Comment varchar(500) DEFAULT NULL,
  FeedbackDate datetime DEFAULT GETDATE(),
  Flag bit DEFAULT 0,
  FOREIGN KEY (SupplierId) REFERENCES Supplier (SupplierId),
  FOREIGN KEY (CustomerId) REFERENCES Customer (CustomerId)
);


-- Table structure for table 'Message'
CREATE TABLE Message (
  MessageId int IDENTITY(1,1) NOT NULL PRIMARY KEY,
  CustomerId int NOT NULL,
  SupplierId int NOT NULL,
  Content varchar(255) DEFAULT NULL,
  SentDate datetime DEFAULT null,
  Flag bit DEFAULT 0,
  FOREIGN KEY (CustomerId) REFERENCES Customer (CustomerId),
  FOREIGN KEY (SupplierId) REFERENCES Supplier (SupplierId)
);