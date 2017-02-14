--Contact View
SELECT c1.Id, FirstName, LastName, Address1, Address2, Notes, ZipCode, HomePhone, WorkPhone, CellPhone, EMail, CityId, c2.Name as CityName, StateId, s.Name as StateName, c1.Active, c1.ModifiedDt, c1.CreateDt 
FROM Contact c1 JOIN City c2 ON (c2.Id = c1.CityId) JOIN State s ON (s.Id = c2.StateId) 
WHERE c1.Active=1