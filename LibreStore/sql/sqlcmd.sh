# use the following command to start sqlcmd on Docker
# -No means no encryption so connection will succeed
# -W -s "|" sets column widths & pipe delimeter between columns
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P $1 -No -W -s "|"
