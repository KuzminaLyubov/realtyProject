/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SET NOCOUNT ON
GO 
SET IDENTITY_INSERT [Realty].[Users] ON
GO
MERGE INTO [Realty].[Users] AS Target
USING (VALUES
  (1,'admin', 'admin', hashbytes('MD5','admin'))
 ,(2,'Кузьмина Любовь', 'lyubov', hashbytes('MD5','qwerty'))
 ,(3,'Сергей Ефремов', 'efremov', hashbytes('MD5','12345'))

) AS Source ([Id],[FullName],[Login],[HashedPassword])
ON (Target.[Id] = Source.[Id])
WHEN MATCHED THEN
 UPDATE SET
 [FullName] = Source.[FullName],
 [Login] = Source.[Login],
 [HashedPassword] = Source.[HashedPassword]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([Id],[FullName],[Login],[HashedPassword])
 VALUES(Source.[Id],Source.[FullName],Source.[Login],Source.[HashedPassword])
;

SET IDENTITY_INSERT [Realty].[Users] OFF
GO


SET NOCOUNT ON
GO 
SET IDENTITY_INSERT [Realty].[Owners] ON
GO
MERGE INTO [Realty].[Owners] AS Target
USING (VALUES
  (1,'Иванов Андрей Игоревич', '+73248324212')
 ,(2,'Петров Степан Семенович', '+74833424212')
 ,(3,'Сидоров Семен Семенович', '+78343824212')
 ,(4,'Васечкин Василий Степанович', '+72483224212')
 ,(5,'Захарова Инна Викторовна', '+73234294212')
 ,(6,'Акунина Василиса Анатольевна', '+73340243212')
 ,(7,'Воронцова Наталья Петровна', '+79238754674')
 ,(8,'Лукьянов Александр Александрович', '+79788774698')
 ,(9,'Борисов Константин Николаевич', '+79238864536')
 ,(10,'Ященко Марина Дмитриевна', '+78569346823')
 ,(11,'Миронов Николай Андреевич', '+78894562145')
 ,(12,'Ситко Карина Кирилловна', '+78432765900')
 ,(13,'Королев Никита Миронович', '+78932456009')
 ,(14,'Лапшина Диана Алексеевна', '+78964532112')
) AS Source ([Id],[FullName],[PhoneNumber])
ON (Target.[Id] = Source.[Id])
WHEN MATCHED THEN
 UPDATE SET
 [FullName] = Source.[FullName],
 [PhoneNumber] = Source.[PhoneNumber]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([Id],[FullName],[PhoneNumber])
 VALUES(Source.[Id],Source.[FullName],Source.[PhoneNumber])
;

SET IDENTITY_INSERT [Realty].[Owners] OFF
GO

SET IDENTITY_INSERT [Realty].[RealEstates] ON
GO
MERGE INTO [Realty].[RealEstates] AS Target
USING (VALUES
  (1,1,'Дача 6 соток','Подмосковье, Можайское шоссе, 27', 1500000)
 ,(2,2,'1к квартира','Москва, Ленинский проспект, 1', 10000000)
 ,(3,2,'Участок','Купавна, Моск. обл., ул. Ленина, д.20', 600000)
 ,(4,4,'Дача 6 соток','Подмосковье, Можайское шоссе, 27', 1500000)
 ,(5,5,'2к квартира','Москва, Рязанский проспект, д.100', 10000000)
 ,(6,3,'Участок','Купавна, Моск. обл., ул. Ленина, д.20', 600000)
 ,(7,6,'Участок','Железнодорожный, Моск. обл., ул. 8 марта, д.2', 600000)
 ,(8,9,'3к квартира','Сочи, ул. Ломоносовский проспект, д.90/3, кв.56', 1000000)
 ,(9,11,'5к квартира','Москва, ул. Новокузнецкая, д.89, кв.14', 12000000)
 ,(10,10,'Участок', 'Лобня, ул.Некрасова, д.67', 1300000)
 ,(11,14,'Дача 12 соток', 'Моск.обл., ул. Петровская, 167', 123000000)
 ,(12,13,'4к квартира', 'Ленинский проспект, д.32,кв.12', 4000000)
 ,(13,7,'Дача','Люберцы, ул.Знатный проезд, д.5, кв.2', 1000000)
 ,(14,8,'Комната', 'Москва, ул.академика Люлькина, д.3/2, кв.2', 10000)
) AS Source ([Id],[OwnerId], [Title], [Address], [Price])
ON (Target.[Id] = Source.[Id])
WHEN MATCHED THEN
 UPDATE SET
 [OwnerId] = Source.[OwnerId],
 [Title] = Source.[Title],
 [Address] = Source.[Address],
 [Price] = Source.[Price]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([Id],[OwnerId], [Title], [Address], [Price])
 VALUES(Source.[Id],Source.[OwnerId], Source.[Title], Source.[Address], Source.[Price])
;

SET IDENTITY_INSERT [Realty].[RealEstates] OFF
GO


SET NOCOUNT OFF
GO


