-- list stored procs in order of created date
SELECT 
    s.name AS SchemaName,
    p.name AS ProcedureName,
    p.create_date AS CreatedOn
FROM 
    sys.procedures p
JOIN 
    sys.schemas s ON p.schema_id = s.schema_id
WHERE 
    p.is_ms_shipped = 0
ORDER BY 
    p.create_date ASC;
GO
