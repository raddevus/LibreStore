CREATE TABLE IF NOT EXISTS [MainToken2]
(
    [ID] INTEGER NOT NULL PRIMARY KEY,
    [OwnerId] INTEGER NOT NULL default(0),
    [Key] NVARCHAR(128)  NOT NULL UNIQUE check(length(Key) <= 128) check(length(key) >= 10),
    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
    [Active] BOOLEAN default (1)
);

insert into MainToken2 select * from MainToken;

drop table MainToken;

alter table MainToken2 rename to MainToken;

CREATE TABLE IF NOT EXISTS [Bucket2]
(
    [ID] INTEGER NOT NULL PRIMARY KEY,
    [MainTokenId] INTEGER NOT NULL,
    [Intent] NVARCHAR(20) check(length(Intent) <= 20),
    [Data] NVARCHAR(20000) NOT NULL check(length(Data) <= 20000),
    [Hmac] NVARCHAR(64) NOT NULL check(length(Hmac) <= 64),
    [Iv] NVARCHAR(32) NOT NULL check(length(Iv) <= 32),
    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
    [Updated] NVARCHAR(30) check(length(Updated) <= 30),
    [Active] BOOLEAN default(1)
);

insert into Bucket2 select * from Bucket;

drop table Bucket;

alter table Bucket2 rename to Bucket;

CREATE TABLE IF NOT EXISTS [Usage2]
(
    [ID] INTEGER NOT NULL PRIMARY KEY,
    [MainTokenId] INTEGER NOT NULL default(0),
    [IpAddress] NVARCHAR(60) check(length(IpAddress) <= 60),
    [Action] NVARCHAR(75) check(length(Action) <= 75),
    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
    [Active] BOOLEAN default (1)
);

insert into Usage2 select * from Usage;

drop table Usage;

alter table Usage2 rename to Usage;

CREATE TABLE IF NOT EXISTS [CyaBucket2]
(
    [ID] INTEGER NOT NULL PRIMARY KEY,
    [MainTokenId] INTEGER NOT NULL UNIQUE,
    [Data] NVARCHAR(40000) NOT NULL check(length(Data) <= 40000),
    [Hmac] NVARCHAR(64) NOT NULL check(length(Hmac) <= 64),
    [Iv] NVARCHAR(32) NOT NULL check(length(Iv) <= 32),
    [Created] NVARCHAR(30) default (datetime('now','localtime')) check(length(Created) <= 30),
    [Updated] NVARCHAR(30) check(length(Updated) <= 30),
    [Active] BOOLEAN default(1)
);

insert into CyaBucket2 select * from CyaBucket;

drop table CyaBucket;

alter table CyaBucket2 rename to CyaBucket;


select count(*) from cyabucket; --48
select count(*) from maintoken; --56
select count(*) from usage; -- 2269
select count(*) from bucket; --1
