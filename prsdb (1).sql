-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Aug 09, 2024 at 08:25 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `prsdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `allergies`
--

CREATE TABLE `allergies` (
  `ID` int(11) NOT NULL,
  `AllergyDetails` varchar(255) DEFAULT NULL,
  `PID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `diagnosis`
--

CREATE TABLE `diagnosis` (
  `DId` int(20) NOT NULL,
  `PId` int(20) NOT NULL,
  `Patient_Name` varchar(255) NOT NULL,
  `Symptoms` varchar(255) NOT NULL,
  `Diagnostic_Test` varchar(255) NOT NULL,
  `Medicines` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `diagnosis`
--

INSERT INTO `diagnosis` (`DId`, `PId`, `Patient_Name`, `Symptoms`, `Diagnostic_Test`, `Medicines`) VALUES
(8373, 73632, 'jddhede', 'jduw', 'jjbfh', 'jwehfgf');

-- --------------------------------------------------------

--
-- Table structure for table `forgotpass`
--

CREATE TABLE `forgotpass` (
  `username` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `healthcare`
--

CREATE TABLE `healthcare` (
  `ID` int(50) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Specialization` varchar(255) NOT NULL,
  `ContactNumber` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `healthcare`
--

INSERT INTO `healthcare` (`ID`, `Name`, `Specialization`, `ContactNumber`) VALUES
(1, 'pranses', 'sdfsf', 909873);

-- --------------------------------------------------------

--
-- Table structure for table `logindb`
--

CREATE TABLE `logindb` (
  `UserID` int(255) NOT NULL,
  `UserName` varchar(255) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `patientinfo`
--

CREATE TABLE `patientinfo` (
  `PID` int(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Age` int(15) NOT NULL,
  `Address` varchar(50) NOT NULL,
  `Status` varchar(50) NOT NULL,
  `BirthDay` varchar(50) NOT NULL,
  `ContactNumber` varchar(15) DEFAULT NULL,
  `Company` text NOT NULL,
  `Position` text NOT NULL,
  `LMP` text NOT NULL,
  `Symptoms` varchar(255) NOT NULL,
  `Diagnostic_Test` varchar(255) NOT NULL,
  `Medicines` varchar(255) NOT NULL,
  `Record_Date` timestamp(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE current_timestamp(6),
  `MedicalConditions` varchar(255) NOT NULL,
  `FamilyMedicalHistory` varchar(255) NOT NULL,
  `Medications` varchar(255) NOT NULL,
  `Allergies` varchar(255) DEFAULT NULL,
  `AppointmentDateTime` datetime DEFAULT NULL,
  `CreatedDate` timestamp NOT NULL DEFAULT current_timestamp(),
  `AppointmentStatus` varchar(50) DEFAULT NULL,
  `VisitType` varchar(50) DEFAULT NULL,
  `Patient_Name` varchar(50) NOT NULL,
  `Email` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `patientinfo`
--

INSERT INTO `patientinfo` (`PID`, `Name`, `Age`, `Address`, `Status`, `BirthDay`, `ContactNumber`, `Company`, `Position`, `LMP`, `Symptoms`, `Diagnostic_Test`, `Medicines`, `Record_Date`, `MedicalConditions`, `FamilyMedicalHistory`, `Medications`, `Allergies`, `AppointmentDateTime`, `CreatedDate`, `AppointmentStatus`, `VisitType`, `Patient_Name`, `Email`) VALUES
(162, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:57:26.328762', '', '', '', NULL, '2024-08-08 07:00:32', '2024-08-05 04:00:19', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(163, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:58:13.639046', '', '', '', NULL, '2024-08-08 10:00:32', '2024-08-05 04:00:33', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(164, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:58:30.062469', '', '', '', NULL, '2024-08-08 11:00:32', '2024-08-05 04:01:15', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(165, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:58:39.787519', '', '', '', NULL, '2024-08-08 12:00:32', '2024-08-05 04:01:27', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(166, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:58:51.800770', '', '', '', NULL, '2024-08-08 13:00:32', '2024-08-05 04:01:42', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(167, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:59:00.825245', '', '', '', NULL, '2024-08-08 14:00:32', '2024-08-05 04:01:56', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(169, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:59:35.035304', '', '', '', NULL, '2024-08-08 16:00:32', '2024-08-06 11:59:35', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(170, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:59:50.828789', '', '', '', NULL, '2024-08-08 17:00:32', '2024-08-05 07:09:14', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(171, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 11:59:59.361565', '', '', '', NULL, '2024-08-08 18:00:32', '2024-08-05 07:10:23', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(172, 'Alma Tamboan', 0, '', '', '', NULL, '', '', '', '', '', '', '2024-08-06 12:00:09.131800', '', '', '', NULL, '2024-08-08 19:00:32', '2024-08-05 07:11:40', 'Scheduled', 'Viewing Result', '', 'albiolacrisel@gmail.com'),
(173, 'hghjg', 76, 'hh', 'Single', '08-08-2024', '08977776', 'hgh', 'ht', 'hh', '', '', '', '2024-08-08 09:42:14.306928', '', '', '', NULL, '2024-08-10 07:00:22', '2024-08-08 09:41:00', 'Scheduled', 'Follow up', '', 'albiolacrisel@gmail.com'),
(174, 'mondie', 20, 'luzon', 'Married', '07-08-2024', '09566481052', 'jgff', 'jkhg', 'jgg', '', '', '', '2024-08-08 10:27:41.000000', '', '', '', NULL, NULL, '2024-08-08 10:27:41', NULL, NULL, '', '');

-- --------------------------------------------------------

--
-- Table structure for table `resetpass`
--

CREATE TABLE `resetpass` (
  `tokenID` int(255) DEFAULT NULL,
  `userID` int(255) NOT NULL,
  `token` varchar(255) NOT NULL,
  `expiry` datetime(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `scheduledtasks`
--

CREATE TABLE `scheduledtasks` (
  `TaskId` int(11) NOT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `AppointmentDateTime` datetime DEFAULT NULL,
  `NotificationDateTime` datetime DEFAULT NULL,
  `TaskStatus` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `VerificationCode` varchar(255) DEFAULT NULL,
  `VerifyCode` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `username`, `password`, `email`, `VerificationCode`, `VerifyCode`) VALUES
(18, 'lesirc', 'asdfghjkl', 'jacekalix436@gmail.com', NULL, NULL),
(19, 'moday', 'asdfghjkl', 'jacekalix436@gmail.com', NULL, NULL),
(20, 'o', 'k', 'llo', NULL, NULL),
(21, 'snow', 'sssssssss', 'jacekalix436@gmail.com', NULL, NULL),
(22, 'snowlab', 'snowww', 'jacekalix436@gmail.com', NULL, NULL),
(24, 'admin', 'snowy', 'albiolacrisel@gmail.com', NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `verificationcodes`
--

CREATE TABLE `verificationcodes` (
  `UserID` int(255) NOT NULL,
  `VerificationCode` varchar(255) NOT NULL,
  `Expiration` datetime(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `verificationcodes`
--

INSERT INTO `verificationcodes` (`UserID`, `VerificationCode`, `Expiration`) VALUES
(22, '939695', '2024-07-07 15:15:55.62910'),
(22, '319392', '2024-07-26 16:33:24.53387'),
(22, '733973', '2024-08-05 12:27:03.46249'),
(22, '254759', '2024-08-05 16:30:48.11173'),
(22, '240622', '2024-08-06 20:19:19.39213');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `allergies`
--
ALTER TABLE `allergies`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `PID` (`PID`);

--
-- Indexes for table `diagnosis`
--
ALTER TABLE `diagnosis`
  ADD PRIMARY KEY (`DId`);

--
-- Indexes for table `healthcare`
--
ALTER TABLE `healthcare`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `logindb`
--
ALTER TABLE `logindb`
  ADD PRIMARY KEY (`UserID`);

--
-- Indexes for table `patientinfo`
--
ALTER TABLE `patientinfo`
  ADD PRIMARY KEY (`PID`);

--
-- Indexes for table `scheduledtasks`
--
ALTER TABLE `scheduledtasks`
  ADD PRIMARY KEY (`TaskId`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `allergies`
--
ALTER TABLE `allergies`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `diagnosis`
--
ALTER TABLE `diagnosis`
  MODIFY `DId` int(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8374;

--
-- AUTO_INCREMENT for table `healthcare`
--
ALTER TABLE `healthcare`
  MODIFY `ID` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `logindb`
--
ALTER TABLE `logindb`
  MODIFY `UserID` int(255) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `patientinfo`
--
ALTER TABLE `patientinfo`
  MODIFY `PID` int(255) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=175;

--
-- AUTO_INCREMENT for table `scheduledtasks`
--
ALTER TABLE `scheduledtasks`
  MODIFY `TaskId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `allergies`
--
ALTER TABLE `allergies`
  ADD CONSTRAINT `allergies_ibfk_1` FOREIGN KEY (`PID`) REFERENCES `patientinfo` (`PID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
