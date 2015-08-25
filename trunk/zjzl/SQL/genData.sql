use zjzl;
set charset gb2312;
#used to genrate testing data

insert into employee(name, password, acl) values('bob', 't1Wnv35BEJg=', ',1,');#123456
insert into employee(name, password, acl) values('tom', 't1Wnv35BEJg=', ',2,');
insert into employee(name, password, acl) values('zip', 't1Wnv35BEJg=', ',3,');

insert into material(name, rname, level, price) values('ycҰ��', 'zj�Ͼ�����', '5', 0.05);
insert into material(name, rname, level, price) values('ycҰ��', 'zj�Ͼ�����', '4', 0.10);
insert into material(name, rname, level, price) values('ycҰ��', 'zj�Ͼ�����', '3', 0.15);
insert into material(name, rname, level, price) values('ycҰ��', 'zj�Ͼ�����', '2', 0.20);
insert into material(name, rname, level, price) values('ytҺ��', 'jj�ƾ�', '2', 40.00);
insert into material(name, rname, level, price) values('ytҺ��', 'jj�ƾ�', '3', 20.00);
insert into material(name, rname, level, price) values('ypҩƬ', 'jm��ĸ', '��', 23.00);
insert into material(name, rname, level, price) values('ypҩƬ', 'jm��ĸ', '4', 16.50);
insert into material(name, rname, level, price) values('mx��ޣ��', 'zm����ޣ', '2', 1.50);
insert into material(name, rname, level, price) values('mx��ޣ��', 'zm����ޣ', '3', 1.30);
insert into material(name, rname, level, price) values('mx��ޣ��', 'zm����ޣ', '4', 1.10);

insert into product(name, rname, level, price) values('nslţ����', 'cl����A', '1', 20.00);
insert into product(name, rname, level, price) values('nslţ����', 'cl����A', '2', 15.00);
insert into product(name, rname, level, price) values('48����Һ', 'nyũҩA', '1', 50.00);
insert into product(name, rname, level, price) values('48����Һ', 'nyũҩA', '2', 30.00);

insert into pur_organization(name, upper) values('ylԣ¡����������', 50000);
insert into pur_organization(name, upper) values('ft��̨', 50000);
insert into pur_organization(name, upper) values('hr����', 50000);

insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100001', 'wl����һ', 1000, '110653612864357', 2);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100002', 'cc�²�', 1000, '110653612864123', 2);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100003', 'bz��չ��', 1000, '110653612864456', 2);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100004', 'lx�����',2000,'123456789012345',1);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100005', 'dz����',200,'123456789012345',1);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100006', 'lws½��˫',1009,'123456789012346',1);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100007', 'txy١����',1000,'123456789023456789',3);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100008', 'ds��ʮ��',20000,'212121212121212122',3);
insert into pur_customer(tag, name, upper, ni, org_id) values('6900000100009', 'ylԬ��',200,'123451234512345123',3);

select @tmpid := id from material where rname='zj�Ͼ�����' and level='3';
select @cus_id := id from pur_customer where tag='6900000100002';
select @emp_id := id from employee where name='bob';
insert into pur_detail(counter, emp_id, deal_time, material_id, quantity, customer_id, price)  
 values('C2', @emp_id, now(), @tmpid, 200, @cus_id, 0.15);
update pur_customer set upper_used= upper_used+200 where id= @cus_id;
select @emp_id := id from employee where name='tom';
insert into pur_detail(counter, emp_id, deal_time, material_id, quantity, customer_id, price)  
 values('C1', @emp_id, now(), @tmpid, 300, @cus_id, 0.15);
update pur_customer set upper_used= upper_used+300 where id= @cus_id;

insert into sale_customer(name) values('sy��Ԫ');
insert into sale_customer(name) values('mn��ţ');
insert into sale_customer(name) values('yl����');
insert into sale_customer(name) values('xj�Ľ�');
