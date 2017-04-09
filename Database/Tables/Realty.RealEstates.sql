CREATE TABLE [Realty].[RealEstates]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[OwnerID] INT NOT NULL,
    [Title] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(100) NOT NULL, 
    [Price] MONEY NOT NULL,
	CONSTRAINT FK_RealEstates_Owners 
	FOREIGN KEY (OwnerID) REFERENCES [Realty].[Owners] (Id)
)
