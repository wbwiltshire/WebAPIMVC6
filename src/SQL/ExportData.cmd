bcp Customer.dbo.State out data\State.txt -c -t "|" -T -S SCHVW2K12R2-DB\MSSQL2014 
bcp Customer.dbo.City out data\City.txt -c -t "|" -T -S SCHVW2K12R2-DB\MSSQL2014 
bcp Customer.dbo.Contact out data\Contact.txt -c -t "|" -T -S SCHVW2K12R2-DB\MSSQL2014 
Pause