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

insert into dbo.Realty ([Title], [Address], [Price]) values ('Дача 6 соток','Подмосковье, Можайское шоссе, 27', 1500000);
insert into dbo.Realty ([Title], [Address], [Price]) values ('Чудесная 1к квартира','Москва, Ленинский проспект, 1', 10000000);
insert into dbo.Realty ([Title], [Address], [Price]) values ('Участок','Купавна, Моск. обл., ул. Ленина, д.20', 600000);

