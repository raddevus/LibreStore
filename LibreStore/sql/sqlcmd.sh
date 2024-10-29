# use the following command to start sqlcmd on Docker
# -No means no encryption so connection will succeed
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P $1 -No
