
CREATE TABLE [MainToken]([ID] INTEGER NOT NULL IDENTITY(1,1),
[OwnerId] INTEGER NOT NULL default(0),
[Key] NVARCHAR(128) NOT NULL UNIQUE CONSTRAINT minKey check(LEN([Key]) >= 10),
[Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxCreated check(LEN(Created) <= 30),
[Active] bit default (1))

-- ##########################
-- MainToken as a Single Line
CREATE TABLE [MainToken]([ID] INTEGER NOT NULL IDENTITY(1,1),[OwnerId] INTEGER NOT NULL default(0), [Key] NVARCHAR(128) NOT NULL UNIQUE CONSTRAINT minKey check(LEN([Key]) >= 10), [Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxCreated check(LEN(Created) <= 30), [Active] bit default (1));

CREATE TABLE [Bucket]
(
    [ID] INTEGER NOT NULL IDENTITY(1,1),
    [MainTokenId] INTEGER NOT NULL,
    [Intent] NVARCHAR(20) CONSTRAINT maxIntent check(LEN(Intent) <= 20),
    [Data] NVARCHAR(MAX) NOT NULL CONSTRAINT maxData check(LEN(Data) <= 20000),
    [Hmac] NVARCHAR(64) NOT NULL CONSTRAINT maxHmac check(LEN(Hmac) <= 64),
    [Iv] NVARCHAR(32) NOT NULL CONSTRAINT maxIv check(LEN(Iv) <= 32),
    [Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxBucketCreated check(LEN(Created) <= 30),
    [Updated] NVARCHAR(30) CONSTRAINT maxBucketUpdated check(LEN(Updated) <= 30),
    [Active] BIT default(1)
)
-- ##########################
-- Bucket as a Single Line
CREATE TABLE [Bucket]([ID] INTEGER NOT NULL IDENTITY(1,1), [MainTokenId] INTEGER NOT NULL, [Intent] NVARCHAR(20) CONSTRAINT maxIntent check(LEN(Intent) <= 20), [Data] NVARCHAR(MAX) NOT NULL CONSTRAINT maxData check(LEN(Data) <= 20000), [Hmac] NVARCHAR(64) NOT NULL CONSTRAINT maxHmac check(LEN(Hmac) <= 64), [Iv] NVARCHAR(32) NOT NULL CONSTRAINT maxIv check(LEN(Iv) <= 32), [Created] NVARCHAR(30) default (getdate()) CONSTRAINT maxBucketCreated check(LEN(Created) <= 30), [Updated] NVARCHAR(30) CONSTRAINT maxBucketUpdated check(LEN(Updated) <= 30),[Active] BIT default(1));

