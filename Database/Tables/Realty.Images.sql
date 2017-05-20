CREATE TABLE [Realty].[Pictures]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(100) NOT NULL, 
    [Content] VARBINARY(MAX) NOT NULL,
	[RealEstateId] INT NOT NULL,
	CONSTRAINT FK_Pictures_RealEstates 
	FOREIGN KEY (RealEstateId) REFERENCES [Realty].[RealEstates] (Id)
)
