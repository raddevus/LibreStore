-- From sqlcmd 
-- :setvar TABLENAME "'<Your-Table-Name>'"
-- :setvar TABLENAME "'AuthorizationClaim'"

SELECT 
        c.name AS ColumnName,
        t.name AS DataType,
        c.is_nullable AS IsNullable,
        c.is_identity AS IsIdentity,
        c.column_id AS ColumnID
    FROM 
        sys.columns c
    JOIN 
        sys.types t ON c.user_type_id = t.user_type_id
    JOIN 
        sys.objects o ON c.object_id = o.object_id
    WHERE 
        o.type = 'U'  -- User-defined table
        AND o.name = $(TABLENAME)
    ORDER BY 
        c.column_id;
GO
