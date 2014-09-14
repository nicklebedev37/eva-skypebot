IF NOT EXISTS (SELECT * FROM Projects)
BEGIN
	INSERT INTO Projects(Name, Alias)
	VALUES (N'FirstProject', N'pr1')
END

IF NOT EXISTS (SELECT * FROM Resources)
BEGIN
	INSERT INTO Resources(Name, [Type], Url, Username, [Password], AdditionalFields, ProjectId)
	VALUES (N'FirstProjectJira', N'jira', N'https://www.somecompany.com/jira/', 'login', 'password', '<additionalfields><issuetype>IST</issuetype><issuetype>LIB</issuetype></additionalfields>', 4),
	       (N'FirstProjectCruiseControl', N'cc', N'http://ccserver.rp.ru/ccnet', '', '', '<additionalfields><targetserver>local</targetserver><build>FirstProject1.0</build><build>FirstProject1.0-Daily</build></additionalfields>', 4)
END