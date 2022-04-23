-- CREATE USER 'newuser'@'localhost' IDENTIFIED BY 'password';
CREATE USER IF NOT EXISTS robodelUser@localhost IDENTIFIED BY 'robodel123';

DROP DATABASE IF EXISTS `RoboDelMVP`;
SET default_storage_engine=InnoDB;
SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;

CREATE DATABASE IF NOT EXISTS RoboDelMVP
    DEFAULT CHARACTER SET utf8mb4
    DEFAULT COLLATE utf8mb4_unicode_ci;
USE RoboDelMVP;

GRANT SELECT, INSERT, UPDATE, DELETE, FILE ON *.* TO 'robodelUser'@'localhost';
GRANT ALL PRIVILEGES ON `robodelUser`.* TO 'robodelUser'@'localhost';
GRANT ALL PRIVILEGES ON `RoboDelMVP`.* TO 'robodelUser'@'localhost';
FLUSH PRIVILEGES;

-- Tables
CREATE TABLE Customer (
 ID int UNSIGNED NOT NULL AUTO_INCREMENT,
 FirstName varchar(100) NOT NULL,
 LastName varchar(100),
 PhoneNumber varchar(20) NOT NULL,
 Email varchar(50),
 Address varchar(100) NOT NULL,
 City varchar(50) NOT NULL,
 State char(20),
 Zip char(20),
 Country char(20) NOT NULL,
 PRIMARY KEY (ID)
);

INSERT INTO Customer(FirstName,LastName,PhoneNumber,Email,Address,City,State,Zip,Country)
VALUES("Hayk","Kosyan", "094252647","hayk@gmail.com", "615 W Johnson St","Madison", "WI","53706","US" );
INSERT INTO Customer(FirstName,LastName,PhoneNumber,Email,Address,City,State,Zip,Country)
VALUES("Armen","Sargsyan", "077424514","armen@gmail.com", "903 Delaplaine ct, Apt 234","Madison", "WI","53715","US" );
INSERT INTO Customer(FirstName,LastName,PhoneNumber,Email,Address,City,State,Zip,Country)
VALUES("Karen","Tumanyan", "099654142","karen@gmail.com", "1810 Monroe St","Madison", "WI","53711","US" );


CREATE TABLE Restaurant (
 Email varchar(30),
 Password varchar(50),
 Name varchar(100) NOT NULL,
 Type varchar(20),
 Price decimal(10, 2),
 Status varchar(20) NOT NULL,
 PhoneNumber varchar(20),
 Address varchar(100) NOT NULL,
 City varchar(50) NOT NULL,
 State char(20),
 Zip char(15),
 Country char(20) NOT NULL,
 Longitude decimal(11, 8),
 Latitude decimal(10, 8),
 PRIMARY KEY (Email)
);

INSERT INTO Restaurant
VALUES("rest001@gmail.com","pass001", "Subway","FastFood",2.5,"active","6085245457","1401 University Ave","Madison","WI","53715", "US", 43.07320680240717, -89.4091213348218);
INSERT INTO Restaurant
VALUES("rest002@gmail.com","pass002", "McDonalds","FastFood",2.0,"active","6089985415","1102 Regent St","Madison","WI","53715", "US", 43.068036717942924, -89.40441712219484);
 
 
CREATE TABLE User (
 Username varchar(50) NOT NULL,
 Password varchar(50) Not NULL,
 Email varchar(50),
 FirstName varchar(100) NOT NULL,
 LastName varchar(100) NOT NULL,
 PhoneNumber varchar(20),
 Status varchar(20) NOT NULL,
 Address varchar(100),
 City varchar(50) NOT NULL,
 State char(20),
 Zip char(20),
 Country char(20),
 PRIMARY KEY (Username)
);

INSERT INTO User
VALUES("username001","pass001","001@gmail.com","Artak","Meloyan", "6085355344","Active", "908 Washingtion street","Madison", "WI","53456","USA" );
INSERT INTO User
VALUES("username002","pass002","002@gmail.com","Lilit","Karayan", "6088745344","Active", "403 Cameron street","Madison", "WI","53456","USA" );


CREATE TABLE Role (
  Role char(20) NOT NULL,
  PRIMARY KEY (Role)
);

CREATE TABLE UserRole (
  Username varchar(50) NOT NULL,
  Role char(20) NOT NULL,
  UNIQUE (Username, Role),
  FOREIGN KEY (Role) REFERENCES Role (Role),
  FOREIGN KEY (Username) REFERENCES User (Username)
);

CREATE TABLE `Order` (
  ID int UNSIGNED NOT NULL AUTO_INCREMENT,
  DateTime datetime NOT NULL,
  Status varchar(20) NOT NULL,
  Longitude decimal(11, 8),
  Latitude decimal(10, 8),
  RestaurantEmail varchar(30) NOT NULL,
  CustomerID int UNSIGNED NOT NULL,
  PRIMARY KEY (ID),
  FOREIGN KEY (RestaurantEmail) REFERENCES Restaurant (Email),
  FOREIGN KEY (CustomerID) REFERENCES Customer (ID)
);

INSERT INTO `Order`(DateTime, Status, Longitude, Latitude, RestaurantEmail, CustomerID)
VALUES(NOW(),"pending", 43.0718530746374, -89.39687374871734, "rest001@gmail.com",1);
INSERT INTO `Order`(DateTime, Status, Longitude, Latitude, RestaurantEmail, CustomerID)
VALUES(NOW(),"pending", 43.06485559171917, -89.41740470312469, "rest001@gmail.com",3);
INSERT INTO `Order`(DateTime, Status, Longitude, Latitude, RestaurantEmail, CustomerID)
VALUES(NOW(),"pending", 43.05896571575645, -89.40065873664759, "rest002@gmail.com",2);


CREATE TABLE Courier (
 ID int UNSIGNED NOT NULL AUTO_INCREMENT,
 PRIMARY KEY (ID)
);

INSERT INTO Courier
VALUES();
INSERT INTO Courier
VALUES();
INSERT INTO Courier
VALUES();
INSERT INTO Courier
VALUES();
INSERT INTO Courier
VALUES();

CREATE TABLE Robot (
	CourierID int UNSIGNED NOT NULL,
    StateOfRobot varchar(20) NOT NULL,
	City varchar(50) NOT NULL,
	State char(20),
	Country char(20) NOT NULL,
	UNIQUE (CourierID),
	FOREIGN KEY (CourierID) REFERENCES Courier (ID)
);

INSERT INTO Robot
VALUES(1,"active","Madison","WI","US");
INSERT INTO Robot
VALUES(3,"inactive","Madison","WI","US");
INSERT INTO Robot
VALUES(4,"active","Madison","WI","US");
INSERT INTO Robot
VALUES(5,"active","Madison","WI","US");

CREATE TABLE HumanCourier (
	CourierID int UNSIGNED NOT NULL,
	Username varchar(50)  NOT NULL,
	City varchar(50) NOT NULL,
	State char(20),
	Country char(20) NOT NULL,
	UNIQUE (CourierID),
	FOREIGN KEY (CourierID) REFERENCES Courier (ID),
	FOREIGN KEY (Username) REFERENCES User (Username)
);
INSERT INTO HumanCourier
VALUES(2,"username002","Madison","WI","US");

