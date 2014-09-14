-------------------------------------------------------------------------------------------------------------------------------------------------------
--- The script for creating the eva database
-------------------------------------------------------------------------------------------------------------------------------------------------------

-- Don't allow SQL Server 2000 and earlier
DECLARE @ver NVARCHAR(128)
SET @ver = CAST(serverproperty('ProductVersion') AS NVARCHAR)
SET @ver = SUBSTRING(@ver, 1, CHARINDEX('.', @ver) - 1)
DECLARE @verInt INT
SET @verInt = CAST(@ver AS INT)
IF ( @verInt <= 8 )
BEGIN
	RAISERROR('Skypebot Eva requires SQL Server 2005 or later', 20, -1) WITH LOG
	RETURN
END

IF NOT EXISTS (SELECT name FROM dbo.sysobjects WHERE id = object_id(N'Projects') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE Projects (
		Id INT IDENTITY(1,1) NOT NULL,
		Name NVARCHAR(250) NOT NULL,
		Alias NVARCHAR(250) NOT NULL,
		CONSTRAINT PK_Projects PRIMARY KEY CLUSTERED (Id ASC)
	)
END

IF NOT EXISTS (SELECT name FROM dbo.sysobjects WHERE id = object_id(N'ChatSubscriptions') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE ChatSubscription (
		Id INT IDENTITY(1,1) NOT NULL,
		ChatName NVARCHAR(250) NOT NULL,
		ProjectId INT NOT NULL,
		CONSTRAINT PK_ChatSubscriptions PRIMARY KEY CLUSTERED (Id ASC),
		CONSTRAINT FK_ChatSubscriptions_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
	)
END

IF NOT EXISTS (SELECT name FROM dbo.sysobjects WHERE id = object_id(N'Resources') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE Resources (
		Id INT IDENTITY(1,1) NOT NULL,
		Name NVARCHAR(250) NOT NULL,
		Type NVARCHAR(250) NOT NULL,
		Url NVARCHAR(250) NOT NULL,
		Username NVARCHAR (250) NOT NULL,
		Password NVARCHAR (250) NOT NULL,
		AdditionalFields NVARCHAR(250) NOT NULL,
		ProjectId INT NOT NULL,
		CONSTRAINT PK_Resources PRIMARY KEY CLUSTERED (Id ASC),
		CONSTRAINT FK_Resources_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
	)
END

IF NOT EXISTS (SELECT name FROM dbo.sysobjects WHERE id = object_id(N'DisabledChats') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE DisabledChats (
		Id INT IDENTITY(1,1) NOT NULL,
		Name NVARCHAR(250) NOT NULL,
		CONSTRAINT PK_EnabledChats PRIMARY KEY CLUSTERED (Id ASC)
	)
END

IF NOT EXISTS (SELECT name FROM dbo.sysobjects WHERE id = object_id(N'Feedbacks') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE Feedbacks (
		Id INT IDENTITY(1,1) NOT NULL,
		UsersSkypeId NVARCHAR(250) NOT NULL,
		[Text] NVARCHAR(500) NOT NULL,
		CONSTRAINT PK_Feedbacks PRIMARY KEY CLUSTERED (Id ASC)
	)
END