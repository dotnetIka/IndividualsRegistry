INSERT INTO IndividualsRegistry.dbo.PersonRelationships (PersonId,RelatedPersonId,RelationType,CreatedAt,UpdatedAt,DeletedAt) VALUES
	 (1,2,N'ნათესავი','2024-09-18 12:11:31.0302627',NULL,NULL);

INSERT INTO IndividualsRegistry.dbo.PhoneNumbers ([Type],Number,PersonId,CreatedAt,UpdatedAt,DeletedAt) VALUES
	 (N'მობილური',N'577070955',1,'2024-09-18 12:00:28.5515127',NULL,'2024-09-18 20:01:02.9348576'),
	 (N'მობილური',N'577070944',2,'2024-09-18 12:08:47.9467727',NULL,NULL),
	 (N'მობილური',N'577070955',1,'2024-09-18 20:01:02.9398402',NULL,NULL),
	 (N'მობილური',N'577070999',1,'2024-09-18 20:01:03.0122019',NULL,NULL),
	 (N'მობილური',N'577070955',3,'2024-09-18 20:15:20.9245612',NULL,NULL);

INSERT INTO IndividualsRegistry.dbo.Persons (FirstName,LastName,Gender,PersonalNumber,DateOfBirth,CreatedAt,UpdatedAt,DeletedAt) VALUES
	 (N'irakli',N'khonelidze',N'კაცი',N'01024088149','2003-09-17 17:55:05.3430000','2024-09-18 12:00:02.7352056','2024-09-18 20:01:02.8905316',NULL),
	 (N'giorgi',N'khonelidze',N'კაცი',N'01024088149','2003-09-17 17:55:05.3430000','2024-09-18 12:08:36.1752063',NULL,NULL),
	 (N'lela',N'khonelidze',N'კაცი',N'01024088149','2003-09-17 17:55:05.3430000','2024-09-18 20:15:20.6424621',NULL,NULL);
