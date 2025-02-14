CREATE DATABASE backendExamProject

USE backendExamProject

CREATE TABLE tickets(
	ticket_id VARCHAR(50) PRIMARY KEY NOT NULL,
	category_name VARCHAR(255) NOT NULL,
	ticket_code VARCHAR(50)UNIQUE NOT NULL,
	ticket_name VARCHAR(255) NOT NULL,
	event_date DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
	ticket_price DECIMAL(10,2) NOT NULL,
	ticket_quota INT NOT NULL,
	ticket_remaining_quota INT NOT NULL,
	ticket_has_seat_number BIT NOT NULL DEFAULT 0,
	ticket_created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	ticket_modified_at DATETIME NULL,
	ticket_created_by VARCHAR(255) NULL,
	ticket_modified_by VARCHAR(255) NULL,
);

CREATE TABLE user_accounts(
	user_account_id VARCHAR(50) PRIMARY KEY NOT NULL,
	user_account_name VARCHAR(100) NOT NULL,
	user_account_email VARCHAR(255) NOT NULL UNIQUE,
	user_account_password VARCHAR(255) NOT NULL,
	user_account_address VARCHAR(255) NOT NULL,
	user_account_phone_number VARCHAR(20) NOT NULL UNIQUE,
	user_account_created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	user_account_modified_at DATETIME NULL,
	user_account_created_by VARCHAR(255) NULL,
	user_account_modified_by VARCHAR(255) NULL,
);

CREATE TABLE payment_methods(
	payment_method_id INT PRIMARY KEY NOT NULL,
	method_name VARCHAR(100) NOT NULL,
	payment_methods_created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	payment_methods_modified_at DATETIME NULL,
	payment_methods_created_by VARCHAR(255) NULL,
	payment_methods_modified_by VARCHAR(255) NULL,
);

CREATE TABLE booked_tickets(
	booked_ticket_id VARCHAR(50) PRIMARY KEY NOT NULL,
	user_account_id VARCHAR(50) FOREIGN KEY REFERENCES user_accounts(user_account_id) NOT NULL,
	booked_ticket_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	booked_ticket_total_price DECIMAL(10,2) NOT NULL,
	payment_method_id INT FOREIGN KEY REFERENCES payment_methods(payment_method_id) NOT NULL,
	booked_ticket_paid_amount DECIMAL (10,2) NOT NULL,
	booked_ticket_change_amount DECIMAL (10,2) NULL,
	booked_ticket_created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	booked_ticket_modified_at DATETIME NULL,
	booked_ticket_created_by VARCHAR(255) NULL,
	booked_ticket_modified_by VARCHAR(255) NULL,
)

CREATE TABLE booked_tickets_detail(
	booked_ticket_detail_id VARCHAR(50) PRIMARY KEY NOT NULL,
	booked_ticket_id VARCHAR(50) FOREIGN KEY REFERENCES booked_tickets(booked_ticket_id) NOT NULL,
	ticket_id VARCHAR(50) FOREIGN KEY REFERENCES tickets(ticket_id) NOT NULL,
	booked_ticket_details_quantity INT NOT NULL,
	booked_ticket_details_subtotal_price DECIMAL(10,2) NOT NULL,
	booked_ticket_details_seat_number VARCHAR(255) NULL,
	booked_ticket_details_created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	booked_ticket_details_modified_at DATETIME NULL,
	booked_ticket_details_created_by VARCHAR(255) NULL,
	booked_ticket_details_modified_by VARCHAR(255) NULL,
);
