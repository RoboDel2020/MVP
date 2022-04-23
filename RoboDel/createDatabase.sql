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
 Country char(20),
 PRIMARY KEY (ID)
);

CREATE TABLE Restaurant (
 Email varchar(50),
 Password varchar(50),
 Name varchar(100) NOT NULL,
 Type varchar(20),
 Price decimal(10, 2),
 Status varchar(20),
 PhoneNumber varchar(20),
 Address varchar(100) NOT NULL,
 City varchar(50) NOT NULL,
 State char(20),
 Zip char(15),
 Country char(20) NOT NULL,
 Longitude decimal(10, 6),
 Latitude decimal(10, 6),
 PRIMARY KEY (Email)
);

INSERT INTO Restaurant
VALUES("rest001@gmail.com","pass001", "Subway","FastFood",2.5,"active","6085245457","1401 University Ave","Madison","WI","53715", "US", 43.073148, -89.409090);
 
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



