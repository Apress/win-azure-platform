CREATE USER testuser1 FOR LOGIN testlogin1;

CREATE USER testuser2 FOR LOGIN testlogin2;

CREATE USER testuser3 FOR LOGIN testlogin3;

EXEC sp_addrolemember 'db_owner', 'testuser1'
EXEC sp_addrolemember 'db_datawriter', 'testuser2'
EXEC sp_addrolemember 'db_datareader', 'testuser3'