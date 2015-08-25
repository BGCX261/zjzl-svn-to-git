set charset utf8;

create database if not exists zjzl default charset=utf8;
use zjzl;

#tables for factory info
create table if not exists employee
(id int unsigned auto_increment primary key,
name varchar(16) not null,
password varchar(255) not null,
enable tinyint(1) default 1,
acl varchar(50) ,
flag tinyint(1) default 1
)default charset=utf8;

#groups��ʱ��ʹ��
create table if not exists groups #����Ҫͬ����ws��
(id int unsigned auto_increment primary key,
name varchar(16) not null,
flag tinyint(1) default 1
)default charset=utf8;

#create table if not exists factory #�ڸ��ز���Ҫ
#(id smallint unsigned auto_increment primary key,
#name varchar(128) not null
#)default charset=utf8;

create table if not exists counter
(id smallint unsigned auto_increment primary key,
name varchar(64) not null,
type varchar(1) not null,
flag tinyint(1) default 1
)default charset=utf8;

create table if not exists product
(id smallint unsigned auto_increment primary key,
name varchar(64) not null,
rname varchar(64) not null,
level varchar(16) not null,
price decimal(7,2),
flag tinyint(1) default 1
)default charset=utf8;

create table if not exists material
(id smallint unsigned auto_increment primary key,
name varchar(64) not null,
rname varchar(64) not null,
level varchar(16) not null,
price decimal(7,2),
flag tinyint(1) default 1
)default charset=utf8;

#tables for material purchase
create table if not exists pur_organization
(id smallint unsigned auto_increment primary key,
name varchar(128) not null,
upper int not null default 0,
flag tinyint(1) default 1
)default charset=utf8;

create table if not exists pur_customer
(id int unsigned auto_increment primary key,
tag varchar(20) not null, #��ӡ���������룬��Ϊϵͳ������
name varchar(16) not null,
upper int not null default 0,
upper_used int default 0,
org_id smallint default 0,
ni varchar(40) not null,
flag tinyint(1) default 1
)default charset=utf8;

ALTER TABLE pur_customer ADD UNIQUE index_pur_tag (tag);

create table if not exists pur_detail
(id int unsigned auto_increment primary key,
#factory_id smallint unsigned not null, #�ڸ��ز���Ҫ
counter varchar(8),
emp_id smallint unsigned not null,
deal_time datetime not null ,
material_id smallint unsigned not null,
quantity decimal(7,2) not null,
customer_id smallint unsigned not null,
price decimal(7,2) not null,
flag tinyint(1) default 1
)default charset=utf8;

#tables for sale
create table if not exists sale_customer
(id int unsigned auto_increment primary key,
name varchar(16) not null,
is_corp tinyint(1) not null default 1,
flag tinyint(1) default 1
)default charset=utf8;

create table if not exists sale_detail
(id int unsigned auto_increment primary key,
#factory_id smallint unsigned not null, #�ڸ��ز���Ҫ
counter varchar(8),
emp_id smallint unsigned not null,
deal_time datetime not null ,
product_id smallint unsigned not null,
quantity decimal(7,2) not null,
customer_id smallint unsigned not null,
price decimal(7,2) not null,
flag tinyint(1) default 1
)default charset=utf8;

#tables for opearation detail
create table if not exists operation_detail
(id int unsigned auto_increment primary key,
#factory_id smallint unsigned not null, #�ڸ��ز���Ҫ
counter varchar(8) not null,
emp_id smallint unsigned not null,
begin_time datetime not null ,
end_time datetime not null ,
flag tinyint(1) default 1
)default charset=utf8;

create table if not exists material_cost
(id int unsigned auto_increment primary key,
material_id smallint unsigned not null,
quantity decimal(7,2) not null,
operation_id int unsigned,
flag tinyint(1) default 1
)default charset=utf8;

create table if not exists product_out
(id int unsigned auto_increment primary key,
product_id smallint unsigned not null,
quantity decimal(7,2) not null,
operation_id int unsigned,
flag tinyint(1) default 1
)default charset=utf8;

# basic data
set charset gb2312;
insert into sale_customer(name) values('gr����');
insert into pur_organization(name, upper) values('qt����', 0);
select @tmpid := id from pur_organization where name='qt����';
insert into pur_customer(tag, name, upper, ni, org_id) values('1', 'bw������Ա', 0, '111111111111111111', @tmpid);

insert into employee(name, password, acl) values('admin', 't1Wnv35BEJg=', ',1,2,3,4,');#1234

grant select,insert,update,delete on zjzl.* to zjzl@"%" Identified by "123";
