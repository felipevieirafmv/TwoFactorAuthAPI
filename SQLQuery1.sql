use master
go

if exists(select * from sys.databases where name = 'TwoFactorAuthAPI')
	drop database TwoFactorAuthAPI
go

create database TwoFactorAuthAPI
go

use TwoFactorAuthAPI
go

create table UserData
(
	Id int identity primary key,
	UserName varchar(255) not null,
	UserPassword varchar(MAX) not null,
	Email varchar(255) not null,
	Salt varchar(200) not null
)
go
