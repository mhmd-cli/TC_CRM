-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 12, 2024 at 09:53 AM
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
-- Database: `tc_crm`
--

-- --------------------------------------------------------

--
-- Table structure for table `benefits`
--

CREATE TABLE `benefits` (
  `BenefitName` varchar(255) NOT NULL,
  `Description` text NOT NULL,
  `Status` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `benefits`
--

INSERT INTO `benefits` (`BenefitName`, `Description`, `Status`) VALUES
('Discounted Membership', 'Get a discount on next year\'s membership.', 'Inactive'),
('Free Workshop', 'Access to exclusive workshops.', 'Active'),
('Priority Support', 'Access to priority customer support.', 'Active'),
('something', 'something', 'Active');

-- --------------------------------------------------------

--
-- Table structure for table `chatmessages`
--

CREATE TABLE `chatmessages` (
  `message_id` int(11) NOT NULL,
  `Timestamp` timestamp NOT NULL DEFAULT current_timestamp(),
  `Username` varchar(255) NOT NULL,
  `Message` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `chatmessages`
--

INSERT INTO `chatmessages` (`message_id`, `Timestamp`, `Username`, `Message`) VALUES
(1, '2024-12-12 04:25:01', 'User1', 'Hello, everyone!'),
(2, '2024-12-12 04:30:01', 'User2', 'Hi, User1! How\'s it going?'),
(3, '2024-12-12 04:35:01', 'User1', 'I\'m doing well, thanks! How about you?'),
(4, '2024-12-12 04:32:01', 'Mohamed', 'Excited to be here!'),
(5, '2024-12-12 04:34:01', 'Jamil', 'Hi Mohamed, welcome!'),
(6, '2024-12-12 04:35:40', 'Mohamed', 'Hello guys, this is a new message!');

-- --------------------------------------------------------

--
-- Table structure for table `conversions`
--

CREATE TABLE `conversions` (
  `id` int(11) NOT NULL,
  `Date` date NOT NULL,
  `Demographic` varchar(255) NOT NULL,
  `PurchaseType` varchar(255) NOT NULL,
  `OneOffPurchases` int(11) NOT NULL,
  `Memberships` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `conversions`
--

INSERT INTO `conversions` (`id`, `Date`, `Demographic`, `PurchaseType`, `OneOffPurchases`, `Memberships`) VALUES
(1, '2023-01-01', 'Young Adults', 'Event', 50, 5),
(2, '2023-02-01', 'Young Adults', 'Event', 60, 8),
(3, '2023-03-01', 'Young Adults', 'Subscription', 70, 15),
(4, '2023-01-01', 'Middle Aged', 'Event', 80, 12),
(5, '2023-02-01', 'Middle Aged', 'Subscription', 90, 20),
(6, '2023-03-01', 'Middle Aged', 'Event', 100, 25);

-- --------------------------------------------------------

--
-- Table structure for table `membershipbenefits`
--

CREATE TABLE `membershipbenefits` (
  `MembershipType` varchar(50) NOT NULL,
  `BenefitName` varchar(255) NOT NULL,
  `Description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `membershipbenefits`
--

INSERT INTO `membershipbenefits` (`MembershipType`, `BenefitName`, `Description`) VALUES
('Gold', 'Free Workshop', 'Access to exclusive workshops.'),
('Gold', 'Priority Support', 'Access to priority customer support.'),
('Gold', 'something', 'something'),
('Silver', 'Discounted Membership', ' A discount on membership renewal.');

-- --------------------------------------------------------

--
-- Table structure for table `modules`
--

CREATE TABLE `modules` (
  `ModuleName` varchar(255) NOT NULL,
  `Description` text NOT NULL,
  `Available` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `modules`
--

INSERT INTO `modules` (`ModuleName`, `Description`, `Available`) VALUES
('Advanced Leadership', 'Learn advanced leadership skills for team management.', 0),
('Community Building', 'Focuses on building stronger communities.', 0),
('Intro to Culture', 'An introductory course to cultural awareness.', 1);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `uid` int(11) NOT NULL,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `profileStatus` tinyint(1) NOT NULL,
  `role` varchar(50) DEFAULT NULL,
  `membershipType` varchar(50) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`uid`, `username`, `password`, `email`, `profileStatus`, `role`, `membershipType`, `created_at`) VALUES
(1, 'Mohamed Abdelrahman', 'password123', 'mohamed@example.com', 1, 'Member', 'Standard', '2024-12-12 04:25:56'),
(2, 'User1', 'password1', 'user1@example.com', 1, 'Member', 'Standard', '2024-12-12 04:35:01'),
(3, 'User2', 'password2', 'user2@example.com', 1, 'Member', 'Standard', '2024-12-12 04:35:01'),
(4, 'User3', 'password3', 'user3@example.com', 1, 'Member', 'Standard', '2024-12-12 04:35:01'),
(5, 'Mohamed', 'password4', 'mohamed@example.com', 1, 'Member', 'Standard', '2024-12-12 04:35:01'),
(6, 'Jamil', 'password5', 'jamil@example.com', 1, 'Member', 'Standard', '2024-12-12 04:35:01');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `benefits`
--
ALTER TABLE `benefits`
  ADD PRIMARY KEY (`BenefitName`);

--
-- Indexes for table `chatmessages`
--
ALTER TABLE `chatmessages`
  ADD PRIMARY KEY (`message_id`);

--
-- Indexes for table `conversions`
--
ALTER TABLE `conversions`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `membershipbenefits`
--
ALTER TABLE `membershipbenefits`
  ADD PRIMARY KEY (`MembershipType`,`BenefitName`),
  ADD KEY `BenefitName` (`BenefitName`);

--
-- Indexes for table `modules`
--
ALTER TABLE `modules`
  ADD PRIMARY KEY (`ModuleName`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`uid`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `chatmessages`
--
ALTER TABLE `chatmessages`
  MODIFY `message_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `conversions`
--
ALTER TABLE `conversions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `membershipbenefits`
--
ALTER TABLE `membershipbenefits`
  ADD CONSTRAINT `membershipbenefits_ibfk_1` FOREIGN KEY (`BenefitName`) REFERENCES `benefits` (`BenefitName`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
