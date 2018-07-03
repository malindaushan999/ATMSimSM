CREATE DATABASE  IF NOT EXISTS `atmsim_db` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `atmsim_db`;
-- MySQL dump 10.13  Distrib 5.7.22, for Linux (x86_64)
--
-- Host: 127.0.0.1    Database: atmsim_db
-- ------------------------------------------------------
-- Server version	5.7.22-0ubuntu0.16.04.1

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
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account` (
  `acc_no` varchar(12) NOT NULL,
  `pin` int(4) NOT NULL,
  `amount` double DEFAULT NULL,
  `user_id` varchar(12) DEFAULT NULL,
  PRIMARY KEY (`acc_no`),
  KEY `fk_account_user_idx` (`user_id`),
  CONSTRAINT `fk_account_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
INSERT INTO `account` VALUES ('002119625194',3920,4892072,'USR-1'),('019560310879',8338,1340721,'USR-2'),('029388437377',7471,5217836,'USR-3'),('039725685618',9719,6522284,'USR-4'),('055718139943',3094,8299638,'USR-5'),('057543569004',2793,1890935,'USR-6'),('063311776645',7054,7206787,'USR-7'),('068098482898',8658,6251427,'USR-8'),('068576711874',9966,3562210,'USR-9'),('072485856343',4063,2847325,'USR-10'),('083387335203',9031,9143414,'USR-11'),('086133849830',5359,4499606,'USR-12'),('091577193953',8814,6945442,'USR-13'),('099296281272',3983,3794393,'USR-14'),('099750715663',5834,3825570,'USR-15'),('113010846290',2744,7717804,'USR-16'),('134893468233',1691,846138,'USR-17'),('141461744549',7871,8326940,'USR-18'),('154924482467',3758,9019030,'USR-19'),('171114155306',1444,386187,'USR-20'),('175760003932',4160,1967028,'USR-21'),('218115478258',2739,9297345,'USR-22'),('218155524993',5479,7323430,'USR-23'),('221897666563',1484,8368508,'USR-24'),('225624580600',5782,8040961,'USR-25'),('255037134101',4516,8218679,'USR-26'),('265408524562',9584,5634680,'USR-27'),('279045203233',7691,1918176,'USR-28'),('294423419573',7110,3288024,'USR-29'),('308194424758',5082,8398040,'USR-30'),('338005244746',5442,3637878,'USR-31'),('346585694993',6882,9977217,'USR-32'),('353396093221',8798,7963759,'USR-33'),('354811891037',4322,5934440,'USR-34'),('373107868132',7990,4691309,'USR-35'),('381993024776',6614,5464897,'USR-36'),('395049082506',3722,9445272,'USR-37'),('395560982224',6476,6339527,'USR-38'),('398139492688',7231,6180550,'USR-39'),('420707645455',3178,2084986,'USR-40'),('452850348208',8687,3020429,'USR-41'),('455944345079',3376,2082987,'USR-42'),('468687086132',2338,5102686,'USR-43'),('475337690725',1084,3964824,'USR-44'),('485436996153',6937,568022,'USR-45'),('487953929840',5222,9651614,'USR-46'),('537230404526',3834,7928088,'USR-47'),('566099585697',4580,303276,'USR-48'),('570753250031',9000,1585789,'USR-49'),('581789742350',3828,2343972,'USR-50'),('582586822372',1191,3400313,NULL),('594195907773',8942,2283277,NULL),('605065143858',4184,7885196,NULL),('605744726959',4008,9029508,NULL),('606805456108',9490,8257121,NULL),('637653682431',7330,8572320,NULL),('649100855202',8484,8754864,NULL),('682335329272',6889,4925782,NULL),('688082658006',9320,8423132,NULL),('691896917536',5071,6969275,NULL),('693384196343',9280,249016,NULL),('695100771391',4376,5036881,NULL),('695811003462',7245,9221571,NULL),('696907735328',8925,4482248,NULL),('715269567240',8440,3147360,NULL),('724615578956',8905,3070805,NULL),('734430442552',3790,331406,NULL),('740800107818',1180,5590207,NULL),('743402992028',6481,9637783,NULL),('746062949858',9853,2491450,NULL),('751322550835',2526,1485283,NULL),('774049637462',2271,9833735,NULL),('793424048652',5306,1882832,NULL),('794085539043',4341,1231793,NULL),('802408210273',8584,3560319,NULL),('803989132653',9328,5535895,NULL),('825028373227',1907,1217936,NULL),('828687921342',9791,5607340,NULL),('835615269195',1300,1049700,NULL),('839725393378',5793,251919,NULL),('842599837609',6736,1831287,NULL),('859334719989',6692,3789300,NULL),('877405421604',1877,4218872,NULL),('892440257409',3768,6460132,NULL),('897415708518',2770,677410,NULL),('900186875918',5350,6153716,NULL),('902251475777',7444,3256718,NULL),('907160557542',7489,8218767,NULL),('915335694413',3578,7605714,NULL),('936996464246',6223,582520,NULL),('937895214337',4489,8840190,NULL),('939157316320',6822,3118999,NULL),('942604022538',5271,7312093,NULL),('949566023314',9171,6120925,NULL),('954298025841',1869,7015134,NULL),('961260575102',6757,6868344,NULL),('971421143960',8544,3627362,NULL),('977343115810',8702,7370710,NULL),('985001458524',5518,4587731,NULL),('989525449112',1258,3514588,NULL);
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `transaction` (
  `user_id` varchar(12) NOT NULL,
  `acc_no` varchar(12) NOT NULL,
  `timestamp` datetime NOT NULL,
  `type` varchar(45) NOT NULL,
  `amount` double NOT NULL,
  `transfered_to` varchar(12) DEFAULT NULL,
  PRIMARY KEY (`user_id`,`acc_no`),
  KEY `fk_transaction_account_idx` (`acc_no`),
  CONSTRAINT `fk_transaction_account` FOREIGN KEY (`acc_no`) REFERENCES `account` (`acc_no`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_transaction_user` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction`
--

LOCK TABLES `transaction` WRITE;
/*!40000 ALTER TABLE `transaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `transaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user` (
  `user_id` varchar(12) NOT NULL,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES ('USR-1','Giacomo Lambert'),('USR-10','Ainsley Hernandez'),('USR-100','Aidan Hubbard'),('USR-11','Violet Haley'),('USR-12','Roary Blair'),('USR-13','Fallon Mooney'),('USR-14','Bevis Mcintyre'),('USR-15','Charde Christensen'),('USR-16','Iris Dunn'),('USR-17','Tanya Avery'),('USR-18','Destiny Terry'),('USR-19','Karen Sargent'),('USR-2','Isadora Raymond'),('USR-20','Simone Wilkinson'),('USR-21','Jena Horton'),('USR-22','Jerome Odom'),('USR-23','Micah Ellison'),('USR-24','Yoshi Joyner'),('USR-25','Justin Gomez'),('USR-26','Kessie Dickson'),('USR-27','Hermione Mckinney'),('USR-28','Adria Wong'),('USR-29','Carl Parker'),('USR-3','Rhiannon Mullen'),('USR-30','Kato Mclaughlin'),('USR-31','Casey Combs'),('USR-32','Oprah Perkins'),('USR-33','Carol Carr'),('USR-34','Grace Byrd'),('USR-35','Rhea Burgess'),('USR-36','Allen Norton'),('USR-37','Lillian Merrill'),('USR-38','Armando Manning'),('USR-39','Nathaniel Cotton'),('USR-4','Anjolie Kirby'),('USR-40','Astra Woodard'),('USR-41','Graiden Horton'),('USR-42','Geraldine Becker'),('USR-43','Penelope Bond'),('USR-44','Lael Leach'),('USR-45','Eve Avery'),('USR-46','Yuri Smith'),('USR-47','Tatiana Stephenson'),('USR-48','Castor Hart'),('USR-49','Erica Brennan'),('USR-5','Ainsley Hatfield'),('USR-50','Justin Manning'),('USR-51','Declan Kelly'),('USR-52','Jescie Wynn'),('USR-53','Reed Mcneil'),('USR-54','Ariel Mcbride'),('USR-55','Damian Allison'),('USR-56','Flynn Cervantes'),('USR-57','Geoffrey Allen'),('USR-58','Shelby Brewer'),('USR-59','Jolene Lancaster'),('USR-6','Celeste Hicks'),('USR-60','Keaton Brock'),('USR-61','Simon Whitney'),('USR-62','Joan Hawkins'),('USR-63','Chelsea Snider'),('USR-64','Laurel Rivera'),('USR-65','Gwendolyn Vincent'),('USR-66','Wing Roth'),('USR-67','Nevada Cantrell'),('USR-68','Karleigh Lynn'),('USR-69','Kirsten Bonner'),('USR-7','Orson Delacruz'),('USR-70','Benjamin Downs'),('USR-71','Ann Velez'),('USR-72','Xenos Briggs'),('USR-73','Kuame Mason'),('USR-74','Ursula Bowen'),('USR-75','Joseph Peck'),('USR-76','Hall Sullivan'),('USR-77','Ethan Randall'),('USR-78','Tanisha Conley'),('USR-79','Francis Sherman'),('USR-8','Alden Alexander'),('USR-80','Kessie Nichols'),('USR-81','Peter Lowery'),('USR-82','Kylan Christian'),('USR-83','Chanda Leon'),('USR-84','Lucas Dorsey'),('USR-85','Anthony Stark'),('USR-86','Cally Guy'),('USR-87','Kim Levine'),('USR-88','Keane Hill'),('USR-89','Denise Price'),('USR-9','Quynn Cote'),('USR-90','Isaiah Mercado'),('USR-91','Erin Sanford'),('USR-92','Chancellor Wilkins'),('USR-93','Jerry Patton'),('USR-94','Leandra Arnold'),('USR-95','Blaze Fletcher'),('USR-96','Pamela Moss'),('USR-97','Lucy Campbell'),('USR-98','Gay Sargent'),('USR-99','Blaine Nguyen');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-06-21 22:24:17
