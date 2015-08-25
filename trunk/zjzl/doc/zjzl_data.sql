-- MySQL dump 10.10
--
-- Host: localhost    Database: zjzl
-- ------------------------------------------------------
-- Server version	5.0.22-community-nt

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `counter`
--


/*!40000 ALTER TABLE `counter` DISABLE KEYS */;
LOCK TABLES `counter` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `counter` ENABLE KEYS */;

--
-- Dumping data for table `employee`
--


/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
LOCK TABLES `employee` WRITE;
INSERT INTO `employee` VALUES (1,'bob','t1Wnv35BEJg=',1,',1,',1),(2,'tom','T1xV2mXqmwGBFLFaWxs6tw==',1,',1,2,4,',1),(3,'zip','t1Wnv35BEJg=',1,',3,',1),(4,'kitty','t1Wnv35BEJg=',1,',1,2,',1),(5,'micky','wXxXqLiuVL98tPeTT/LFJ+tc8tmj04Gw9S/+5ARzLaT6snpCY/NPBfD8td8HralT',1,',1,2,3,4,',1),(6,'lily','t1Wnv35BEJg=',1,',1,2,',1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;

--
-- Dumping data for table `factory`
--


/*!40000 ALTER TABLE `factory` DISABLE KEYS */;
LOCK TABLES `factory` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `factory` ENABLE KEYS */;

--
-- Dumping data for table `groups`
--


/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
LOCK TABLES `groups` WRITE;
INSERT INTO `groups` VALUES (1,'pur',1),(2,'sale',1),(3,'manage',1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;

--
-- Dumping data for table `material`
--


/*!40000 ALTER TABLE `material` DISABLE KEYS */;
LOCK TABLES `material` WRITE;
INSERT INTO `material` VALUES (1,'zj紫荆泽兰','5','0.05','yc野草'),(2,'zj紫荆泽兰','4','0.10','yc野草'),(3,'zj紫荆泽兰','3','0.15','yc野草'),(4,'zj紫荆泽兰','2','0.25','yc野草'),(6,'yt液体','2','40.00','jj酒精'),(7,'yt液体','3','20.00','jj酒精'),(8,'yp药片','三','23.00','jm酵母'),(9,'yp药片','4','16.50','jm酵母'),(10,'zm紫苜蓿','2','1.00','mx苜蓿草'),(11,'zm紫苜蓿','3','0.85','mx苜蓿草'),(12,'yp药片','2','30.00','jm酵母');
UNLOCK TABLES;
/*!40000 ALTER TABLE `material` ENABLE KEYS */;

--
-- Dumping data for table `material_cost`
--


/*!40000 ALTER TABLE `material_cost` DISABLE KEYS */;
LOCK TABLES `material_cost` WRITE;
INSERT INTO `material_cost` VALUES (1,3,'100.00',5),(2,3,'50.00',6),(3,1,'50.00',6),(4,4,'80.00',7),(5,6,'20.00',7),(6,6,'50.00',8),(7,3,'150.00',8),(8,4,'500.00',9);
UNLOCK TABLES;
/*!40000 ALTER TABLE `material_cost` ENABLE KEYS */;

--
-- Dumping data for table `operation_detail`
--


/*!40000 ALTER TABLE `operation_detail` DISABLE KEYS */;
LOCK TABLES `operation_detail` WRITE;
INSERT INTO `operation_detail` VALUES (1,'CP',4,'2009-02-13 13:10:00','2009-02-13 14:10:00',1),(5,'C1',3,'2009-02-18 12:05:00','2009-02-18 13:06:00',1),(6,'qq',2,'2009-02-18 13:09:00','2009-02-18 14:29:00',1),(7,'PP',4,'2009-02-23 00:00:00','2009-02-23 00:00:00',1),(8,'qq',1,'2009-02-23 13:01:00','2009-02-23 14:07:00',1),(9,'C2',2,'2009-04-05 13:00:00','2009-04-05 14:00:00',1),(10,'C3',3,'2009-04-09 11:17:53','2009-04-09 12:53:53',0);
UNLOCK TABLES;
/*!40000 ALTER TABLE `operation_detail` ENABLE KEYS */;

--
-- Dumping data for table `product`
--


/*!40000 ALTER TABLE `product` DISABLE KEYS */;
LOCK TABLES `product` WRITE;
INSERT INTO `product` VALUES (1,'cl草料','1','25.00','cl草料D'),(2,'ny农药','1','50.00','ny农药D'),(3,'cl草料','2','20.00','cl草料D'),(4,'fb粉饼','2','50.00','nsl牛饲料'),(5,'nsl牛饲料','3','38.00','nsl牛饲料');
UNLOCK TABLES;
/*!40000 ALTER TABLE `product` ENABLE KEYS */;

--
-- Dumping data for table `product_out`
--


/*!40000 ALTER TABLE `product_out` DISABLE KEYS */;
LOCK TABLES `product_out` WRITE;
INSERT INTO `product_out` VALUES (1,1,'60.00',5),(2,1,'60.00',6),(3,2,'20.00',6),(4,1,'60.00',7),(5,2,'10.00',7),(6,1,'120.00',8),(7,2,'20.00',8),(8,1,'400.00',9),(9,2,'54.00',18),(10,2,'96.00',32),(11,2,'240.00',80),(12,2,'240.00',80),(13,2,'240.00',80);
UNLOCK TABLES;
/*!40000 ALTER TABLE `product_out` ENABLE KEYS */;

--
-- Dumping data for table `pur_customer`
--


/*!40000 ALTER TABLE `pur_customer` DISABLE KEYS */;
LOCK TABLES `pur_customer` WRITE;
INSERT INTO `pur_customer` VALUES (1,'351234567890','wl伍六一',500,200,2,'110653612864357',1),(2,'351234567123','cc陈才',2000,300,2,'110653612864123',1),(3,'351234567456','bz白展堂',1000,0,2,'110653612864456',1),(4,'1','bw编外人员',0,330,3,'111111111111111111',1),(5,'456789','lx吕秀才',2000,0,3,'123456789012345',1),(6,'4567890','lxc lv xiu cai',200,0,3,'123456789012345',1),(7,'123456789012345','dz大嘴',200,0,2,'123456789012345',1),(8,'123456789012346','lws陆无双',1009,0,6,'123456789012346',1),(9,'123456789023456789','txy佟湘玉',1000,0,4,'123456789023456789',1),(10,'690000019','ds杜十娘',20000,0,5,'212121212121212122',1),(11,'69000001010','yl袁朗',200,0,1,'123451234512345123',1),(12,'6900000100011','xyy喜羊羊',0,0,4,'123451234541234556',1),(13,'6900000100012','shl孙红雷',10,0,6,'1101234567890564',1),(14,'6900000100013','lyy懒羊羊',100,0,8,'110110110012345',1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `pur_customer` ENABLE KEYS */;

--
-- Dumping data for table `pur_detail`
--


/*!40000 ALTER TABLE `pur_detail` DISABLE KEYS */;
LOCK TABLES `pur_detail` WRITE;
INSERT INTO `pur_detail` VALUES (1,'C1',2,'2009-02-03 17:25:30',3,'200.00',1,'0.15',1),(2,'C1',2,'2009-02-03 17:35:15',3,'300.00',2,'0.15',1),(3,'C1',2,'2009-02-03 17:36:54',2,'200.00',2,'0.10',1),(4,'C@',2,'2009-02-05 14:23:27',3,'200.00',2,'0.15',1),(5,'C2',1,'2009-02-05 14:48:35',3,'200.00',2,'0.15',1),(6,'C1',1,'2009-02-05 14:48:35',3,'300.00',2,'0.15',1),(7,'C1',2,'2009-02-05 14:55:45',3,'200.00',1,'0.15',1),(8,'C1',2,'2009-02-23 14:41:10',3,'30.00',4,'0.15',1),(9,'C1',2,'2009-02-23 14:42:33',1,'100.00',4,'0.05',1),(10,'C1',5,'2009-04-05 17:03:45',6,'200.00',4,'40.00',1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `pur_detail` ENABLE KEYS */;

--
-- Dumping data for table `pur_organization`
--


/*!40000 ALTER TABLE `pur_organization` DISABLE KEYS */;
LOCK TABLES `pur_organization` WRITE;
INSERT INTO `pur_organization` VALUES (1,'yl裕隆回族自治乡',50000),(2,'ft丰台',50000),(3,'qt其他',0),(4,'hr怀柔',0),(5,'tz通州',0),(6,'dx大兴',0),(7,'my密云',0),(8,'xw宣武区2',0);
UNLOCK TABLES;
/*!40000 ALTER TABLE `pur_organization` ENABLE KEYS */;

--
-- Dumping data for table `sale_corporation`
--


/*!40000 ALTER TABLE `sale_corporation` DISABLE KEYS */;
LOCK TABLES `sale_corporation` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `sale_corporation` ENABLE KEYS */;

--
-- Dumping data for table `sale_customer`
--


/*!40000 ALTER TABLE `sale_customer` DISABLE KEYS */;
LOCK TABLES `sale_customer` WRITE;
INSERT INTO `sale_customer` VALUES (1,'gr个人',1,1),(2,'sy三元',1,1),(3,'mn蒙牛',1,1),(4,'yl伊利',1,1),(5,'js江苏牧羊',1,1),(6,'xj夏进',1,1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `sale_customer` ENABLE KEYS */;

--
-- Dumping data for table `sale_detail`
--


/*!40000 ALTER TABLE `sale_detail` DISABLE KEYS */;
LOCK TABLES `sale_detail` WRITE;
INSERT INTO `sale_detail` VALUES (1,'C1',1,'2009-01-17 18:28:52',1,'20.00',1,'20.00',1),(2,'C1',1,'2009-01-17 18:28:52',2,'50.00',1,'50.00',1),(3,'C1',0,'2009-02-04 17:36:07',2,'200.00',2,'50.00',1),(4,'C1',0,'2009-02-04 17:36:07',1,'20.00',2,'20.00',1),(5,'C1',0,'2009-02-04 17:37:11',1,'20.00',3,'20.00',1),(6,'C1',0,'2009-02-04 17:37:11',2,'50.00',3,'50.00',1),(7,'C1',0,'2009-02-05 11:27:18',1,'20.00',1,'20.00',1),(8,'C1',0,'2009-02-05 11:28:37',1,'20.00',1,'20.00',1),(9,'C1',0,'2009-02-05 11:35:50',1,'20.00',1,'20.00',1),(10,'C1',0,'2009-02-05 11:39:04',1,'20.00',1,'20.00',1),(11,'C1',2,'2009-02-23 15:43:47',1,'50.00',1,'20.00',1),(12,'C1',2,'2009-02-23 15:43:47',2,'10.00',1,'50.00',1),(13,'C1',2,'2009-02-23 15:45:43',1,'50.00',1,'20.00',1),(14,'C1',5,'2009-04-05 17:02:01',2,'20.00',2,'50.00',1);
UNLOCK TABLES;
/*!40000 ALTER TABLE `sale_detail` ENABLE KEYS */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

