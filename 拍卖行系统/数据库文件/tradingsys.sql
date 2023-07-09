/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306_copy
Source Server Version : 80023
Source Host           : localhost:3306
Source Database       : tradingsys

Target Server Type    : MYSQL
Target Server Version : 80023
File Encoding         : 65001

Date: 2023-07-09 16:16:59
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for item
-- ----------------------------
DROP TABLE IF EXISTS `item`;
CREATE TABLE `item` (
  `id` int NOT NULL AUTO_INCREMENT,
  `itemid` int DEFAULT NULL,
  `itemname` varchar(255) DEFAULT NULL,
  `sellprice` int DEFAULT NULL,
  `buyprice` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of item
-- ----------------------------
INSERT INTO `item` VALUES ('1', '1', '苹果', '9', '12');
INSERT INTO `item` VALUES ('2', '2', '药剂', '25', '50');
INSERT INTO `item` VALUES ('3', '3', '宝石', '100', '200');
INSERT INTO `item` VALUES ('4', '4', '铁块', '25', '50');
INSERT INTO `item` VALUES ('5', '5', '肉', '25', '50');
INSERT INTO `item` VALUES ('6', '6', '纪念币', '100', '200');
INSERT INTO `item` VALUES ('7', '7', '背包', '60', '120');
INSERT INTO `item` VALUES ('8', '8', '腰带', '50', '100');
INSERT INTO `item` VALUES ('9', '9', '鞋子', '45', '90');
INSERT INTO `item` VALUES ('10', '10', '手套', '40', '80');
INSERT INTO `item` VALUES ('11', '11', '斧子', '35', '70');
INSERT INTO `item` VALUES ('12', '12', '项链', '50', '100');
INSERT INTO `item` VALUES ('13', '13', '手镯', '50', '100');
INSERT INTO `item` VALUES ('14', '14', '斗篷', '150', '300');
INSERT INTO `item` VALUES ('15', '15', '盔甲', '100', '200');
INSERT INTO `item` VALUES ('16', '16', '头盔', '100', '200');
INSERT INTO `item` VALUES ('17', '17', '铁剑', '100', '200');
INSERT INTO `item` VALUES ('18', '18', '盾牌', '100', '200');
INSERT INTO `item` VALUES ('19', '19', '卷轴', '2000', '10000');

-- ----------------------------
-- Table structure for trad
-- ----------------------------
DROP TABLE IF EXISTS `trad`;
CREATE TABLE `trad` (
  `id` int NOT NULL AUTO_INCREMENT,
  `tradeid` int DEFAULT NULL,
  `account` varchar(255) DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  `itemid` int DEFAULT NULL,
  `sellcount` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of trad
-- ----------------------------
INSERT INTO `trad` VALUES ('1', '1', '789', 'QQQ', '1', '1');
INSERT INTO `trad` VALUES ('2', '2', '???', '???', '0', '0');
INSERT INTO `trad` VALUES ('3', '3', '564', 'bieli2', '1', '1');
INSERT INTO `trad` VALUES ('4', '4', '789', 'QQQ', '1', '1');
INSERT INTO `trad` VALUES ('5', '5', '789', 'QQQ', '1', '5');
INSERT INTO `trad` VALUES ('6', '6', '123', '别离', '1', '5');
INSERT INTO `trad` VALUES ('7', '7', '???', '???', '0', '0');
INSERT INTO `trad` VALUES ('8', '8', '???', '???', '0', '0');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `account` varchar(255) DEFAULT NULL,
  `passward` varchar(255) DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `sex` varchar(255) DEFAULT NULL,
  `coin` int(10) unsigned zerofill DEFAULT NULL,
  `diamond` int(10) unsigned zerofill DEFAULT NULL,
  `registtime` datetime DEFAULT NULL,
  `tradcount` int(10) unsigned zerofill DEFAULT NULL,
  `buycount` int(10) unsigned zerofill DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('1', '564', '5678', 'bieli2', '男', '0000000001', '0000000000', '2021-11-22 12:19:34', '0000000001', '0000000001');
INSERT INTO `user` VALUES ('3', '123', '123', '别离', '女', '0000002132', '0000000100', '2021-11-22 12:40:17', '0000000021', '0000000001');
INSERT INTO `user` VALUES ('6', '234', '123', '或雪', '女', '0000000867', '0000000100', '2021-11-25 16:30:45', '0000000005', '0000000004');
INSERT INTO `user` VALUES ('7', '567', '123', 'BLHX', '男', '0000000773', '0000000100', '2021-11-25 16:41:41', '0000000004', '0000000003');
INSERT INTO `user` VALUES ('8', 'qwe', '123', 'WWW', '男', '0000000850', '0000000100', '2021-11-26 13:59:54', '0000000001', '0000000001');
INSERT INTO `user` VALUES ('9', '789', '123', 'QQQ', '男', '0000000838', '0000000100', '2021-12-11 18:24:50', '0000000010', '0000000007');
INSERT INTO `user` VALUES ('10', 'BLHX', 'BLHX', '别离或雪', '男', '0000000657', '0000000100', '2022-07-01 16:07:12', '0000000004', '0000000004');

-- ----------------------------
-- Table structure for userbag
-- ----------------------------
DROP TABLE IF EXISTS `userbag`;
CREATE TABLE `userbag` (
  `id` int NOT NULL AUTO_INCREMENT,
  `account` varchar(255) DEFAULT NULL,
  `itemid` int DEFAULT NULL,
  `count` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userbag
-- ----------------------------
INSERT INTO `userbag` VALUES ('1', '123', '1', '20');
INSERT INTO `userbag` VALUES ('2', '123', '2', '41');
INSERT INTO `userbag` VALUES ('3', '123', '3', '3');
INSERT INTO `userbag` VALUES ('4', '123', '4', '0');
INSERT INTO `userbag` VALUES ('5', '123', '17', '1');
INSERT INTO `userbag` VALUES ('6', '123', '18', '1');
INSERT INTO `userbag` VALUES ('7', '123', '19', '1');
INSERT INTO `userbag` VALUES ('8', '234', '1', '12');
INSERT INTO `userbag` VALUES ('9', '234', '4', '0');
INSERT INTO `userbag` VALUES ('10', '234', '2', '1');
INSERT INTO `userbag` VALUES ('11', '567', '4', '4');
INSERT INTO `userbag` VALUES ('12', '567', '3', '1');
INSERT INTO `userbag` VALUES ('13', 'qwe', '4', '6');
INSERT INTO `userbag` VALUES ('14', '789', '1', '13');
INSERT INTO `userbag` VALUES ('15', '567', '1', '3');
INSERT INTO `userbag` VALUES ('16', '564', '1', '0');
INSERT INTO `userbag` VALUES ('17', 'BLHX', '2', '9');
INSERT INTO `userbag` VALUES ('18', 'BLHX', '1', '2');
INSERT INTO `userbag` VALUES ('19', 'BLHX', '18', '1');
