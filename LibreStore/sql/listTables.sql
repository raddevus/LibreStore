-- List all user tables and count them
SELECT 
    name AS TableName
FROM 
    sys.tables
WHERE 
    is_ms_shipped = 0
order by name;

-- Count of user tables
SELECT 
    COUNT(*) AS UserTableCount
FROM 
    sys.tables
WHERE 
    is_ms_shipped = 0;

Go
