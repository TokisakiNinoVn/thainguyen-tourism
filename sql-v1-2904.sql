-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.0.30 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Version:             12.1.0.6537
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for nhhoang
CREATE DATABASE IF NOT EXISTS `nhhoang` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `nhhoang`;

-- Dumping structure for table nhhoang.blogcomments
CREATE TABLE IF NOT EXISTS `blogcomments` (
  `id` int NOT NULL AUTO_INCREMENT,
  `blog_id` int DEFAULT NULL,
  `user_id` int DEFAULT NULL,
  `comment_content` varchar(255) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT NULL,
  `updatedAt` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.blogcomments: ~0 rows (approximately)

-- Dumping structure for table nhhoang.blogmedia
CREATE TABLE IF NOT EXISTS `blogmedia` (
  `id` int NOT NULL AUTO_INCREMENT,
  `blog_id` int DEFAULT NULL,
  `media_url` varchar(100) DEFAULT NULL,
  `media_type` varchar(50) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT NULL,
  `updatedAt` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.blogmedia: ~0 rows (approximately)

-- Dumping structure for table nhhoang.blogs
CREATE TABLE IF NOT EXISTS `blogs` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(100) DEFAULT NULL,
  `content` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `status` varchar(100) DEFAULT NULL,
  `authorId` int DEFAULT NULL,
  `thumbnail` int DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT NULL,
  `updatedAt` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.blogs: ~0 rows (approximately)
INSERT INTO `blogs` (`id`, `title`, `content`, `status`, `authorId`, `thumbnail`, `createdAt`, `updatedAt`) VALUES
	(1, 'string', 'string', 'string', 0, 0, '2025-04-28 18:06:05', '2025-04-28 18:06:05'),
	(2, 'string', 'string', 'draft', 0, 0, '2025-04-28 20:42:12', '2025-04-28 20:42:12');

-- Dumping structure for table nhhoang.placemedia
CREATE TABLE IF NOT EXISTS `placemedia` (
  `id` int NOT NULL AUTO_INCREMENT,
  `placeId` int DEFAULT NULL,
  `mediaUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `mediaType` int DEFAULT NULL,
  `createdAt` datetime DEFAULT NULL,
  `updatedAt` datetime DEFAULT NULL,
  `imageFor` char(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=260 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.placemedia: ~4 rows (approximately)
INSERT INTO `placemedia` (`id`, `placeId`, `mediaUrl`, `mediaType`, `createdAt`, `updatedAt`, `imageFor`) VALUES
	(246, 18, '/uploads/3f1ca35d-cd30-4f3a-81c6-5dab874bbada.png', 1, '2025-04-21 08:51:41', '2025-04-21 08:51:41', 'thumbnail'),
	(251, 18, '/uploads/7ad224a4-67b4-4dca-85b3-ef7426ea01af.jpg', 1, '2025-04-21 09:12:55', '2025-04-21 09:12:55', 'place-media'),
	(252, 18, '/uploads/394d4f28-5ebd-4b05-961a-10d0da9de342.JPG', 2, '2025-04-21 09:13:04', '2025-04-21 09:13:04', 'place-media'),
	(255, 18, '/uploads/568502a9-6374-4ae5-bbe5-ba3da69ffb23.mp4', 3, '2025-04-29 04:49:21', '2025-04-29 04:49:21', 'place-media'),
	(256, 20, '/uploads/e0f6dd5c-9bbc-4125-8f87-e444fafa1b63.mp3', 4, '2025-04-29 05:03:06', '2025-04-29 05:03:06', 'place-media'),
	(259, 18, '/uploads/27c2332c-b08a-43ad-bdb9-b5021ced6670.mp3', 4, '2025-04-29 05:20:42', '2025-04-29 05:20:42', 'place-media');

-- Dumping structure for table nhhoang.places
CREATE TABLE IF NOT EXISTS `places` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `description` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `latitude` double DEFAULT NULL,
  `longitude` double DEFAULT NULL,
  `thumbnail` int DEFAULT NULL,
  `createdAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.places: ~4 rows (approximately)
INSERT INTO `places` (`Id`, `name`, `description`, `latitude`, `longitude`, `thumbnail`, `createdAt`, `updatedAt`) VALUES
	(18, 'Khu chè Ô Long - HTX Chè Mộc Nguyên', '<p><strong>Khu chè Ô Long HTX Chè Mộc Nguyên</strong></p><p>Khu chè Ô Long HTX Chè Mộc Nguyên là một địa điểm lý tưởng cho những ai yêu thích không gian thiên nhiên tươi đẹp và văn hóa trà truyền thống. Nằm giữa những đồi chè xanh mướt, khu vực này không chỉ nổi tiếng với những sản phẩm trà chất lượng cao mà còn mang đến trải nghiệm thú vị cho du khách.</p><p>Tại đây, bạn có thể tham gia vào các hoạt động như hái chè, tham quan quy trình sản xuất trà và thưởng thức các loại trà Ô Long đặc trưng. Đội ngũ nhân viên nhiệt tình sẽ hướng dẫn bạn cách pha trà đúng cách để cảm nhận được hương vị tinh tế của từng loại trà.</p><p>Ngoài ra, khu chè còn có không gian thư giãn với những góc ngồi lý tưởng để bạn có thể nhâm nhi trà và ngắm nhìn cảnh sắc thiên nhiên tuyệt đẹp. Hãy đến với Khu chè Ô Long HTX Chè Mộc Nguyên để trải nghiệm nét văn hóa trà phong phú và tạo nên những kỷ niệm đáng nhớ!</p>', 21.794374212489576, 105.90983106629511, 246, '2025-04-21 08:50:41', '2025-04-21 08:52:32'),
	(20, 'q', '', 21.83122795476815, 106.0363426479364, NULL, '2025-04-21 09:38:05', '2025-04-21 09:38:05'),
	(21, 'q', '', 21.73171472322514, 106.01435899499306, NULL, '2025-04-21 09:38:11', '2025-04-21 09:38:11'),
	(22, 'wd', '', 21.630855411590886, 105.94978201447212, NULL, '2025-04-21 09:38:15', '2025-04-21 09:38:15');

-- Dumping structure for table nhhoang.reviewmedia
CREATE TABLE IF NOT EXISTS `reviewmedia` (
  `id` int NOT NULL AUTO_INCREMENT,
  `review_id` int DEFAULT NULL,
  `media_url` char(255) DEFAULT NULL,
  `media_type` char(50) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT NULL,
  `updatedAt` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.reviewmedia: ~0 rows (approximately)

-- Dumping structure for table nhhoang.reviews
CREATE TABLE IF NOT EXISTS `reviews` (
  `id` int NOT NULL AUTO_INCREMENT,
  `place_id` int DEFAULT NULL,
  `user_id` int DEFAULT NULL,
  `rating` float DEFAULT NULL,
  `review_description` varchar(100) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT NULL,
  `updatedAt` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.reviews: ~0 rows (approximately)

-- Dumping structure for table nhhoang.typemedia
CREATE TABLE IF NOT EXISTS `typemedia` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  `code` varchar(50) DEFAULT NULL,
  `description` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.typemedia: ~4 rows (approximately)
INSERT INTO `typemedia` (`id`, `name`, `code`, `description`) VALUES
	(1, 'image', 'image', 'Ảnh'),
	(2, 'image360', 'image360', 'Ảnh 360'),
	(3, 'video', 'video', 'Video'),
	(4, 'audio', 'audio', 'Âm thanh');

-- Dumping structure for table nhhoang.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `email` char(255) DEFAULT NULL,
  `password` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '0',
  `role` varchar(50) DEFAULT NULL,
  `photoURL` char(255) DEFAULT NULL,
  `displayName` char(255) DEFAULT NULL,
  `createdAt` datetime DEFAULT NULL,
  `isGoogleLogin` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table nhhoang.users: ~4 rows (approximately)
INSERT INTO `users` (`id`, `email`, `password`, `role`, `photoURL`, `displayName`, `createdAt`, `isGoogleLogin`) VALUES
	(1, 'nino@gmail.com', '$2b$10$1NfXyAgF5M9EJt5s/r6gneC2DUEiKjoJ.m1ETaZciIpwRQ5K2qHlW', 'admin', NULL, NULL, '2025-04-09 21:51:11', 0),
	(2, 'ninoi@gmail.com', '$2b$10$1NfXyAgF5M9EJt5s/r6gneC2DUEiKjoJ.m1ETaZciIpwRQ5K2qHlW', 'user', NULL, NULL, '2025-04-09 21:55:05', 1),
	(3, NULL, '$2b$10$1NfXyAgF5M9EJt5s/r6gneC2DUEiKjoJ.m1ETaZciIpwRQ5K2qHlW', 'admin', NULL, NULL, '2025-04-09 21:55:09', 1),
	(4, NULL, '$2b$10$1NfXyAgF5M9EJt5s/r6gneC2DUEiKjoJ.m1ETaZciIpwRQ5K2qHlW', 'admin', NULL, NULL, '2025-04-09 21:55:10', 1),
	(5, 'trieuduytancbg@gmail.com', '__google_login__', 'user', 'https://lh3.googleusercontent.com/a/ACg8ocLqPUyILLftHgI8uvflx7GvZEe5JQ3tTAn1sE7EteRsFa2vBcnU=s96-c', 'Tokisaki Nino', '2025-04-10 01:09:00', 1),
	(6, 'tongduytan.ego@gmail.com', '__google_login__', 'user', 'https://lh3.googleusercontent.com/a/ACg8ocJefq2VBO8QWyUA2gBK2fVwIe6RzqV4AwAwetLDgsjKtURzZA=s96-c', 'Tokisaki Nino', '2025-04-21 07:46:22', 1);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
