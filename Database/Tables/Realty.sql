CREATE TABLE [dbo].[Realty]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(100) NOT NULL, 
    [Price] MONEY NOT NULL
)
