bcp Customer.dbo.State in data\State.txt -c -t "|"  -T -S SCHVW2K12R2-DB\MSSQL2014
bcp Customer.dbo.City in data\City.txt -c -t "|"  -T -S SCHVW2K12R2-DB\MSSQL2014
bcp Customer.dbo.Contact in data\Contact.txt -c -t "|"  -T -S SCHVW2K12R2-DB\MSSQL2014
Pause