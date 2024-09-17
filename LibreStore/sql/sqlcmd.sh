# use the following command to start sqlcmd on Docker
# -No means no encryption so connection will succeed
sqlcmd -S localhost -U SA -P $1 -No
