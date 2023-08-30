CREATE TABLE MainToken (ID INT AUTO_INCREMENT, 
OwnerId INT NOT NULL default(0),
`Key` VARCHAR(128) NOT NULL UNIQUE check(length(`Key`) <= 128) check(length(`Key`) >= 10)  COLLATE utf8mb4_0900_as_cs, 
Created NVARCHAR(30) default (now()) check(length(Created) <= 30),
Active BOOLEAN default (1), 
PRIMARY KEY(ID));

CREATE TABLE MainToken (ID INT AUTO_INCREMENT, OwnerId INT NOT NULL default(0),`Key` VARCHAR(128) NOT NULL UNIQUE check(length(`Key`) <= 128) check(length(`Key`) >= 10), Created NVARCHAR(30) default (now()) check(length(Created) <= 30),Active BOOLEAN default (1), PRIMARY KEY(ID));


CREATE TABLE Bucket(ID INT AUTO_INCREMENT,
MainTokenId INTEGER NOT NULL,
Intent VARCHAR(20) check(length(Intent) <= 20),
Data Text(20000) NOT NULL check(length(Data) <= 20000),
Hmac VARCHAR(64) NOT NULL check(length(Hmac) <= 64),
Iv VARCHAR(32) NOT NULL check(length(Iv) <= 32),
Created VARCHAR(30) default (now()) check(length(Created) <= 30),
Updated VARCHAR(30) check(length(Updated) <= 30),
Active BOOLEAN default(1), 
PRIMARY KEY(ID))


CREATE TABLE Bucket(ID INT AUTO_INCREMENT,MainTokenId INTEGER NOT NULL,Intent VARCHAR(20) check(length(Intent) <= 20),Data Text(20000) NOT NULL check(length(Data) <= 20000),Hmac VARCHAR(64) NOT NULL check(length(Hmac) <= 64),Iv VARCHAR(32) NOT NULL check(length(Iv) <= 32),Created VARCHAR(30) default (now()) check(length(Created) <= 30),Updated VARCHAR(30) check(length(Updated) <= 30),Active BOOLEAN default(1), PRIMARY KEY(ID));

CREATE TABLE `Usage`
(ID INT AUTO_INCREMENT,
MainTokenId INTEGER NOT NULL default(0),
IpAddress VARCHAR(60) check(length(IpAddress) <= 60),
Action VARCHAR(75) check(length(Action) <= 75),
Created VARCHAR(30) default ((now()) check(length(Created) <= 30),
Active BOOLEAN default (1),
PRIMARY KEY(ID))

CREATE TABLE `Usage`(ID INT AUTO_INCREMENT, MainTokenId INTEGER NOT NULL default(0),IpAddress VARCHAR(60) check(length(IpAddress) <= 60),Action VARCHAR(75) check(length(Action) <= 75),Created VARCHAR(30) default (now()) check(length(Created) <= 30),Active BOOLEAN default (1),PRIMARY KEY(ID));

CREATE TABLE Owner
(ID INT AUTO_INCREMENT,
Email VARCHAR(200) UNIQUE check(length(Email) <= 200),
ExpireDate VARCHAR(30) check(length(ExpireDate) <= 30),
Subscribed BOOLEAN default(0),
Created VARCHAR(30) default (now()) check(length(Created) <= 30),
Updated VARCHAR(30) check(length(Updated) <= 30),
Active BOOLEAN default(1),
PRIMARY KEY(ID))

CREATE TABLE Owner(ID INT AUTO_INCREMENT,Email VARCHAR(200) UNIQUE check(length(Email) <= 200),ExpireDate VARCHAR(30) check(length(ExpireDate) <= 30),Subscribed BOOLEAN default(0),Created VARCHAR(30) default (now()) check(length(Created) <= 30),Updated VARCHAR(30) check(length(Updated) <= 30),Active BOOLEAN default(1),PRIMARY KEY(ID));

CREATE TABLE CyaBucket
(ID INT AUTO_INCREMENT,
MainTokenId INTEGER NOT NULL UNIQUE,
Data Text(40000) NOT NULL check(length(Data) <= 40000),
Hmac VARCHAR(64) NOT NULL check(length(Hmac) <= 64),
Iv VARCHAR(32) NOT NULL check(length(Iv) <= 32),
Created VARCHAR(30) default (now()) check(length(Created) <= 30),
Updated VARCHAR(30) check(length(Updated) <= 30),
Active BOOLEAN default(1),
PRIMARY KEY(ID))

CREATE TABLE CyaBucket(ID INT AUTO_INCREMENT,MainTokenId INTEGER NOT NULL UNIQUE,Data Text(40000) NOT NULL check(length(Data) <= 40000),Hmac VARCHAR(64) NOT NULL check(length(Hmac) <= 64),Iv VARCHAR(32) NOT NULL check(length(Iv) <= 32),Created VARCHAR(30) default (now()) check(length(Created) <= 30),Updated VARCHAR(30) check(length(Updated) <= 30),Active BOOLEAN default(1),PRIMARY KEY(ID));