create database LibreStore;
go

use LibreStore;
go

-- ##########################
-- MainToken as a Single Line
CREATE TABLE [MainToken]([ID] BIGINT NOT NULL IDENTITY(1,1),[OwnerId] BIGINT NOT NULL default(0), [Key] NVARCHAR(128) NOT NULL UNIQUE CONSTRAINT minKey check(LEN([Key]) >= 10), [Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxCreated check(LEN(Created) <= 30), [Active] bit default (1));

-- ##########################
-- Bucket as a Single Line
CREATE TABLE [Bucket]([ID] BIGINT NOT NULL IDENTITY(1,1), [MainTokenId] BIGINT NOT NULL, [Intent] NVARCHAR(20) CONSTRAINT maxIntent check(LEN(Intent) <= 20), [Data] NVARCHAR(MAX) NOT NULL CONSTRAINT maxData check(LEN(Data) <= 20000), [Hmac] NVARCHAR(64) NOT NULL CONSTRAINT maxHmac check(LEN(Hmac) <= 64), [Iv] NVARCHAR(32) NOT NULL CONSTRAINT maxIv check(LEN(Iv) <= 32), [Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxBucketCreated check(LEN(Created) <= 30), [Updated] NVARCHAR(30) CONSTRAINT maxBucketUpdated check(LEN(Updated) <= 30),[Active] BIT default(1));

-- ###########################
-- Usage as a Single Line
CREATE TABLE [Usage]([ID] BIGINT NOT NULL IDENTITY(1,1), [MainTokenId] BIGINT NOT NULL default(0), [IpAddress] NVARCHAR(60) CONSTRAINT ipAddrMax check(LEN(IpAddress) <= 60),     [Action] NVARCHAR(75) CONSTRAINT actionMax check(LEN(Action) <= 75), [Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxUsageCreated check(LEN(Created) <= 30), [Active] BIT default (1));

-- ###########################
-- Owner as a Single Line
CREATE TABLE [Owner]([ID] BIGINT NOT NULL IDENTITY(1,1), [Email] NVARCHAR(200) UNIQUE CONSTRAINT maxEmail check(LEN(Email) <= 200), [ExpireDate] NVARCHAR(30) CONSTRAINT maxExpire check(LEN(ExpireDate) <= 30), [Subscribed] BIT default(0), [Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxOwnerCreated check(LEN(Created) <= 30), [Updated] NVARCHAR(30) CONSTRAINT maxOwnerUpdated check(LEN(Updated) <= 30), [Active] BIT default(1));

-- ###########################
-- CyaBucket as a Single Line
CREATE TABLE [CyaBucket]( [ID] BIGINT NOT NULL IDENTITY(1,1), [MainTokenId] BIGINT NOT NULL UNIQUE, [Data] NVARCHAR(MAX) NOT NULL CONSTRAINT cyaDataMax check(LEN(Data) <= 40000),    [Hmac] NVARCHAR(64) NOT NULL CONSTRAINT cyaHmacMax check(LEN(Hmac) <= 64), [Iv] NVARCHAR(32) NOT NULL  CONSTRAINT cyaIvMax check(LEN(Iv) <= 32), [Created] NVARCHAR(30) default (getdate()) CONSTRAINT cyaCreatedMax check(LEN(Created) <= 30), [Updated] NVARCHAR(30) CONSTRAINT cyaUpdatedMax check(LEN(Updated) <= 30), [Active] BIT default(1));