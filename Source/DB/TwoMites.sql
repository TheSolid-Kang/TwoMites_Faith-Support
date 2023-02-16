-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        10.5.5-MariaDB - mariadb.org binary distribution
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- twomites 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `twomites` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_bin */;
USE `twomites`;

-- 테이블 twomites.bible 구조 내보내기
CREATE TABLE IF NOT EXISTS `bible` (
  `b_id` int(11) NOT NULL AUTO_INCREMENT,
  `b_book` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `b_chapter` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `b_verse` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `b_descript` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `b_full_descript` varchar(1024) COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.bible:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `bible` DISABLE KEYS */;
/*!40000 ALTER TABLE `bible` ENABLE KEYS */;

-- 테이블 twomites.bible_contemplation 구조 내보내기
CREATE TABLE IF NOT EXISTS `bible_contemplation` (
  `bc_date` datetime DEFAULT NULL,
  `bc_book` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `bc_chapter` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `bc_verse` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `bc_descript` longtext COLLATE utf8_bin DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.bible_contemplation:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `bible_contemplation` DISABLE KEYS */;
/*!40000 ALTER TABLE `bible_contemplation` ENABLE KEYS */;

-- 테이블 twomites.bible_summary 구조 내보내기
CREATE TABLE IF NOT EXISTS `bible_summary` (
  `bs_date` datetime DEFAULT NULL,
  `bs_book` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `bs_chapter` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `bs_verse` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `bs_descript` longtext COLLATE utf8_bin DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.bible_summary:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `bible_summary` DISABLE KEYS */;
/*!40000 ALTER TABLE `bible_summary` ENABLE KEYS */;

-- 테이블 twomites.bible_title 구조 내보내기
CREATE TABLE IF NOT EXISTS `bible_title` (
  `bt_id` int(11) NOT NULL AUTO_INCREMENT,
  `bt_name` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `bt_name_key` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`bt_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.bible_title:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `bible_title` DISABLE KEYS */;
/*!40000 ALTER TABLE `bible_title` ENABLE KEYS */;

-- 테이블 twomites.fellowship 구조 내보내기
CREATE TABLE IF NOT EXISTS `fellowship` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fd_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `descript` longtext COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fd_id` (`fd_id`),
  CONSTRAINT `fellowship_ibfk_1` FOREIGN KEY (`fd_id`) REFERENCES `fellowship_department` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.fellowship:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `fellowship` DISABLE KEYS */;
/*!40000 ALTER TABLE `fellowship` ENABLE KEYS */;

-- 테이블 twomites.fellowship_department 구조 내보내기
CREATE TABLE IF NOT EXISTS `fellowship_department` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `department` varchar(128) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.fellowship_department:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `fellowship_department` DISABLE KEYS */;
/*!40000 ALTER TABLE `fellowship_department` ENABLE KEYS */;

-- 테이블 twomites.fellowship_photo 구조 내보내기
CREATE TABLE IF NOT EXISTS `fellowship_photo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `f_id` int(11) DEFAULT NULL,
  `file_ori_name` longtext COLLATE utf8_bin DEFAULT NULL,
  `file_sys_name` varchar(255) COLLATE utf8_bin DEFAULT NULL,
  `phto_original_file_path` longtext COLLATE utf8_bin DEFAULT NULL,
  `phto_thumbnail_file_path` longtext COLLATE utf8_bin DEFAULT NULL,
  `note` longtext COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `f_id` (`f_id`),
  CONSTRAINT `fellowship_photo_ibfk_1` FOREIGN KEY (`f_id`) REFERENCES `fellowship` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.fellowship_photo:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `fellowship_photo` DISABLE KEYS */;
/*!40000 ALTER TABLE `fellowship_photo` ENABLE KEYS */;

-- 테이블 twomites.fellowship_testimony 구조 내보내기
CREATE TABLE IF NOT EXISTS `fellowship_testimony` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `f_id` int(11) DEFAULT NULL,
  `christian` varchar(124) CHARACTER SET utf8 DEFAULT NULL,
  `testimony` longtext COLLATE utf8_bin DEFAULT NULL,
  `gender` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `f_id` (`f_id`),
  CONSTRAINT `fellowship_testimony_ibfk_1` FOREIGN KEY (`f_id`) REFERENCES `fellowship` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.fellowship_testimony:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `fellowship_testimony` DISABLE KEYS */;
/*!40000 ALTER TABLE `fellowship_testimony` ENABLE KEYS */;

-- 테이블 twomites.the_word 구조 내보내기
CREATE TABLE IF NOT EXISTS `the_word` (
  `tw_pk_id` int(11) NOT NULL AUTO_INCREMENT,
  `tw_wt_key` int(11) DEFAULT NULL,
  `tw_pastor` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `tw_date` datetime DEFAULT NULL,
  `tw_created_at` datetime DEFAULT NULL,
  `tw_modified_at` datetime DEFAULT NULL,
  `tw_title` varchar(124) COLLATE utf8_bin DEFAULT NULL,
  `tw_the_word` longtext COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`tw_pk_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- 테이블 데이터 twomites.the_word:~0 rows (대략적) 내보내기
/*!40000 ALTER TABLE `the_word` DISABLE KEYS */;
/*!40000 ALTER TABLE `the_word` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
