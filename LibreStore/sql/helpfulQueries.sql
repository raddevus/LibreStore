-- just a list of helpful queries I use during testing
select mt.key,us.* from maintoken as mt join usage as us on us.maintoken
id = mt.id;

select seq from sqlite_sequence where name='Usage';

select * from usage LIMIT 2;

-- similar to select @@IDENTITY or SELECT SCOPE_IDENTITY() 
SELECT last_insert_rowid();

-- select bucket based on a) maintoken.key b) bucket.id
select b.* from MainToken as mt 
join bucket as b on mt.id = b.mainTokenId 
where mt.Key='thisOneIs9' and b.Id = 6;

-- select cya and display maintoken.key
select mt.key,cya.* from cyabucket as cya 
join maintoken as mt 
on mt.id = cya.mainTokenId;
