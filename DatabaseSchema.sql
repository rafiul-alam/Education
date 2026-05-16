CREATE DATABASE FoodOrderingSystemDB;
GO

USE FoodOrderingSystemDB;
GO

-- Admin Table
CREATE TABLE [Admin] (
    admin_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT GETDATE()
);

-- Customer Table
CREATE TABLE [Customer] (
    customer_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    phone NVARCHAR(20) NOT NULL,
    address NVARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);

-- FoodItem Table
CREATE TABLE [FoodItem] (
    food_id INT IDENTITY(1,1) PRIMARY KEY,
    admin_id INT FOREIGN KEY REFERENCES [Admin](admin_id),
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(MAX),
    price DECIMAL(10,2) NOT NULL,
    category NVARCHAR(50) NOT NULL,
    image VARBINARY(MAX) NULL,
    available BIT DEFAULT 1,
    created_at DATETIME DEFAULT GETDATE(),
    updated_at DATETIME DEFAULT GETDATE()
);

-- Cart Table
CREATE TABLE [Cart] (
    cart_id INT IDENTITY(1,1) PRIMARY KEY,
    customer_id INT FOREIGN KEY REFERENCES [Customer](customer_id),
    created_at DATETIME DEFAULT GETDATE()
);

-- CartItem Table
CREATE TABLE [CartItem] (
    cart_item_id INT IDENTITY(1,1) PRIMARY KEY,
    cart_id INT FOREIGN KEY REFERENCES [Cart](cart_id),
    food_id INT FOREIGN KEY REFERENCES [FoodItem](food_id),
    quantity INT NOT NULL DEFAULT 1,
    unit_price DECIMAL(10,2) NOT NULL
);

-- Order Table
CREATE TABLE [Order] (
    order_id INT IDENTITY(1,1) PRIMARY KEY,
    customer_id INT FOREIGN KEY REFERENCES [Customer](customer_id),
    -- payment_id will be FK logic later to avoid circular logic right now.
    order_status NVARCHAR(50) NOT NULL, -- Pending, Accepted, Paid, Delivering, Completed, Cancelled
    total_amount DECIMAL(10,2) NOT NULL,
    delivery_address NVARCHAR(255) NOT NULL,
    placed_at DATETIME DEFAULT GETDATE(),
    cancelled_at DATETIME NULL
);

-- OrderItem Table
CREATE TABLE [OrderItem] (
    order_item_id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT FOREIGN KEY REFERENCES [Order](order_id) ON DELETE CASCADE,
    food_id INT FOREIGN KEY REFERENCES [FoodItem](food_id),
    quantity INT NOT NULL,
    unit_price DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL
);

-- Payment Table
CREATE TABLE [Payment] (
    payment_id INT IDENTITY(1,1) PRIMARY KEY,
    order_id INT FOREIGN KEY REFERENCES [Order](order_id) ON DELETE CASCADE,
    method NVARCHAR(50) NOT NULL, -- Cash on Delivery, bKash, Nagad
    status NVARCHAR(50) NOT NULL, -- Payment Pending, Payment Completed
    amount DECIMAL(10,2) NOT NULL,
    transaction_ref NVARCHAR(100) NULL,
    paid_at DATETIME NULL
);

-- Update Order table to reference Payment logic can be handled purely in code so we don't end up with circular schema references if Payment references Order as well.
