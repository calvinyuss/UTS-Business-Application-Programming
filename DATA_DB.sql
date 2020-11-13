CREATE DATABASE DB_DATA;

SET IMPLICIT_TRANSACTIONS OFF;

BEGIN TRANSACTION;

USE DB_DATA;

CREATE TABLE "tables" (
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1), 
	table_number TINYINT UNIQUE NOT NULL,
);

CREATE TABLE orders (
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1), 
	table_id INT NOT NULL, 
	"status" VARCHAR(10) NOT NULL /** accepted, cancelled, pending */
);

CREATE TABLE order_items(
	id BIGINT PRIMARY KEY NOT NULL IDENTITY(1,1), 
	order_id INT NOT NULL, 
	menu_id INT NOT NULL, 
	quantity TINYINT NOT NULL, 
	unit_price SMALLMONEY NOT NULL, 
	create_at DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
); 

CREATE TABLE menus(
	id INT PRIMARY KEY NOT NULL IDENTITY(1,1), 
	"name" varchar(50) NOT NULL, 
	price SMALLMONEY NOT NULL, 
	img_url varchar(255) NOT NULL, 
	deleted_at DATETIME,
);

CREATE TABLE payments(
	id INT NOT NULL IDENTITY(1,1), 
	order_id INT NOT NULL,
	amount SMALLMONEY NOT NULL, 
	created_at DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
);

CREATE TABLE admins(
	id INT NOT NULL IDENTITY(1,1), 
	username varchar(50) NOT NULL, 
	password varchar(255) NOT NULL
);


ALTER TABLE orders
ADD CONSTRAINT FK_table_order
FOREIGN KEY (table_id) REFERENCES "tables"(id);

ALTER TABLE order_items
ADD CONSTRAINT FK_order_item
FOREIGN KEY (order_id) REFERENCES orders(id);

ALTER TABLE order_items
ADD CONSTRAINT FK_order_menu
FOREIGN KEY (menu_id) REFERENCES menus(id);

ALTER TABLE payments
ADD CONSTRAINT FK_payment_order
FOREIGN KEY (order_id) REFERENCES orders(id);

COMMIT TRANSACTION;