/*WV

Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=False;Trust Server Certificate=False;Command Timeout=0

WV*/

Create database DBTest;
use DBTest

create table Product(
Id int primary key identity,
BarCod varchar(50) unique,
[Name] varchar(50),
Brand varchar(50),
Category varchar(100),
Price decimal(10,2)
)


INSERT INTO Product(BarCod,Name,Brand,Category,Price) values
('50910010','Monitor Aoc  - Curvo  Gaming ','AOC','Tecnologia','1200'),
('50910011','IdeaPad 3i','LENOVO','Tecnologia','1700'),
('50910012','SoyMomo Tablet Lite','SOYMOMO','Tecnologia','300'),
('50910013','Lavadora 21 KG WLA-21','WINIA','ElectroHogar','1749'),
('50910014','Congelador 145 Lt Blanco','ELECTROLUX','ElectroHogar','779'),
('50910015','Cafetera TH-130','THOMAS','ElectroHogar','119'),
('50910016','Reloj análogo Hombre 058','GUESS','Accesorios','699'),
('50910017','Billetera de Cuero Mujer Sophie','REYES','Accesorios','270'),
('50910018','Bufanda Rec Mango Mujer','MANGO','Accesorios','169.90'),
('50910019','Sofá Continental 3 Cuerpos','MICA','Muebles','1299'),
('50910020','Futón New Elina 3 Cuerpos','MICA','Muebles','1349'),
('50910021','Mesa Comedor Volterra 6 Sillas','TUHOME','Muebles','624.12')

select * from Product

create proc sp_getAll_products
as
begin
	select 
	Id, BarCod, [Name],
	Brand,Category, Price
	from Product
end

go

create proc sp_get_product
	@Id int
as
begin
	select 
	Id, BarCod, [Name],
	Brand,Category, Price
	from Product
end

go

create proc sp_save_product(
@barCod varchar(50),
@name varchar(50),
@brand varchar(50),
@category varchar(100),
@price decimal(10,2)
)as
begin
	insert into Product(BarCod,[Name],Brand,Category,Price)
	values(@barCod,@name,@brand,@category,@price)
end

go


create proc sp_edit_product(
@id int,
@barCod varchar(50) null,
@name varchar(50) null,
@brand varchar(50) null,
@category varchar(100) null,
@price decimal(10,2) null
)as
begin

	update Product set
	BarCod = isnull(@barCod,BarCod),
	[Name] = isnull(@name,Name),
	Brand= isnull(@brand,Brand),
	Category = isnull(@category,Category),
	Price= ISNULL(@price,Price)
	where Id = @id

end

go

create proc sp_delete_product(
@id int
)as
begin

 delete from Product where Id = @id

end



CREATE PROCEDURE sp_get_product_by_id @Id int AS  SELECT * FROM Product WHERE Id = @Id;

select * from Product

EXEC sp_get_product @Id = 2;


EXEC sp_get_product 2;

EXEC sp_get_product_by_id 2;

EXEC sp_get_product_by_id @Id = 2;

