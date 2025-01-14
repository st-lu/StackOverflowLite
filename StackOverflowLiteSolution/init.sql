-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema stackoverflow_lite
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema stackoverflow_lite
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `stackoverflow_lite` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `stackoverflow_lite` ;

-- -----------------------------------------------------
-- Table `stackoverflow_lite`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stackoverflow_lite`.`Users` (
                                                             `Id` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    `Username` LONGTEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
    `Email` LONGTEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
    PRIMARY KEY (`Id`))
    ENGINE = InnoDB
    DEFAULT CHARACTER SET = utf8mb4
    COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `stackoverflow_lite`.`Questions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stackoverflow_lite`.`Questions` (
                                                                 `Id` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    `Content` LONGTEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
    `UserId` CHAR(36) CHARACTER SET 'ascii' NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    `Score` INT NOT NULL DEFAULT '0',
    `ViewsCount` INT NOT NULL DEFAULT '0',
    `IsVisible` TINYINT(1) NOT NULL DEFAULT '0',
    `TextCategory` INT NOT NULL DEFAULT '0',
    PRIMARY KEY (`Id`),
    INDEX `IX_Questions_UserId` (`UserId` ASC) VISIBLE,
    CONSTRAINT `FK_Questions_Users_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `stackoverflow_lite`.`Users` (`Id`)
    ON DELETE CASCADE)
    ENGINE = InnoDB
    DEFAULT CHARACTER SET = utf8mb4
    COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `stackoverflow_lite`.`Answers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stackoverflow_lite`.`Answers` (
                                                               `Id` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    `Content` LONGTEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
    `Score` INT NOT NULL,
    `UserId` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    `QuestionId` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    `IsVisible` TINYINT(1) NOT NULL DEFAULT '0',
    `TextCategory` INT NOT NULL DEFAULT '0',
    PRIMARY KEY (`Id`),
    INDEX `IX_Answers_QuestionId` (`QuestionId` ASC) VISIBLE,
    INDEX `IX_Answers_UserId` (`UserId` ASC) VISIBLE,
    CONSTRAINT `FK_Answers_Questions_QuestionId`
    FOREIGN KEY (`QuestionId`)
    REFERENCES `stackoverflow_lite`.`Questions` (`Id`)
    ON DELETE CASCADE,
    CONSTRAINT `FK_Answers_Users_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `stackoverflow_lite`.`Users` (`Id`)
    ON DELETE CASCADE)
    ENGINE = InnoDB
    DEFAULT CHARACTER SET = utf8mb4
    COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `stackoverflow_lite`.`OidcUserMappings`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stackoverflow_lite`.`OidcUserMappings` (
                                                                        `UserId` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    `SubClaim` LONGTEXT CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
    PRIMARY KEY (`UserId`),
    CONSTRAINT `FK_OidcUserMappings_Users_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `stackoverflow_lite`.`Users` (`Id`)
    ON DELETE CASCADE)
    ENGINE = InnoDB
    DEFAULT CHARACTER SET = utf8mb4
    COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `stackoverflow_lite`.`UserQuestionViews`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stackoverflow_lite`.`UserQuestionViews` (
                                                                         `UserId` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    `QuestionId` CHAR(36) CHARACTER SET 'ascii' NOT NULL,
    PRIMARY KEY (`UserId`, `QuestionId`),
    INDEX `IX_UserQuestionViews_QuestionId` (`QuestionId` ASC) VISIBLE,
    CONSTRAINT `FK_UserQuestionViews_Questions_QuestionId`
    FOREIGN KEY (`QuestionId`)
    REFERENCES `stackoverflow_lite`.`Questions` (`Id`)
    ON DELETE CASCADE,
    CONSTRAINT `FK_UserQuestionViews_Users_UserId`
    FOREIGN KEY (`UserId`)
    REFERENCES `stackoverflow_lite`.`Users` (`Id`)
    ON DELETE CASCADE)
    ENGINE = InnoDB
    DEFAULT CHARACTER SET = utf8mb4
    COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `stackoverflow_lite`.`__EFMigrationsHistory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stackoverflow_lite`.`__EFMigrationsHistory` (
                                                                             `MigrationId` VARCHAR(150) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
    `ProductVersion` VARCHAR(32) CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_0900_ai_ci' NOT NULL,
    PRIMARY KEY (`MigrationId`))
    ENGINE = InnoDB
    DEFAULT CHARACTER SET = utf8mb4
    COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

INSERT INTO `stackoverflow_lite`.`Users` (`Id`, `Username`,`Email`) VALUES
                                                                 ('00000000-0000-0000-0000-000000000001', 'Alice', 'test@email.com'),
                                                                 ('00000000-0000-0000-0000-000000000002', 'Malina', 'malina@gmail.com'),
                                                                 ('00000000-0000-0000-0000-000000000003', 'Radu', 'radu@gmail.com'),
                        ('00000000-0000-0000-0000-000000000004', 'Luana', 'luana@yahoo.com');


INSERT INTO `stackoverflow_lite`.`Questions` (`Id`, `Content`, `UserId`, `Score`, `ViewsCount`) VALUES
                                                                                                    ('dcc0d6ab-9037-4414-ae8f-5d90aaeb1e62', 'What is the purpose of design patterns in software development?', '00000000-0000-0000-0000-000000000001', 15, 120),
                                                                                                    ('07b3d82a-8a60-49d3-8cb2-9fae7be5ec23', 'How does garbage collection work in Java?', '00000000-0000-0000-0000-000000000002', 18, 110),
                                                                                                    ('1f8e6b23-78b0-494f-a234-284cf99d7a33', 'What is the difference between a stack and a queue?', '00000000-0000-0000-0000-000000000003', 22, 130),
                                                                                                    ('c5ad8e89-91ea-4eb4-91dc-54cb2ed3a3d4', 'How can I optimize SQL queries for better performance?', '00000000-0000-0000-0000-000000000004', 12, 80),
                                                                                                    ('d8242f2e-9672-49a2-8af1-37a7355e5f23', 'What is the role of RESTful APIs in modern web development?', '00000000-0000-0000-0000-000000000003', 25, 140),
                                                                                                    ('483a4f18-45e1-40de-b174-7c295ae92345', 'What are the main differences between Python 2 and Python 3?', '00000000-0000-0000-0000-000000000001', 17, 95),
                                                                                                    ('f3c5b632-5d6b-4f5e-8412-8e2d1b74f349', 'How does version control work in Git?', '00000000-0000-0000-0000-000000000002', 20, 105),
                                                                                                    ('ad1d9f3e-c6f9-4b47-85f8-5c8de271a6d1', 'What is the significance of Big-O notation in algorithms?', '00000000-0000-0000-0000-000000000003', 16, 85),
                                                                                                    ('63cddb52-b1f7-4d2f-8498-dcc74d2f5d82', 'How can I debug code effectively in an IDE?', '00000000-0000-0000-0000-000000000004', 13, 75),
                                                                                                    ('a93e3c61-5e2b-4cd8-93f8-f59ed8f5e423', 'What are the key benefits of using containerization tools like Docker?', '00000000-0000-0000-0000-000000000003', 21, 115),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440000', 'What is the difference between a deep copy and a shallow copy in Python?', '00000000-0000-0000-0000-000000000001', 32, 150),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440001', 'How can I optimize a SQL query with multiple joins?', '00000000-0000-0000-0000-000000000002', 28, 200),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440002', 'What are the benefits of using TypeScript over JavaScript?', '00000000-0000-0000-0000-000000000003', 45, 310),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440003', 'How do I debug a segmentation fault in C++?', '00000000-0000-0000-0000-000000000004', 39, 250),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440004', 'What are the best practices for RESTful API design?', '00000000-0000-0000-0000-000000000003', 50, 400),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440005', 'How do I handle concurrency in Java using Executors?', '00000000-0000-0000-0000-000000000001', 27, 180),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440006', 'What is the difference between Docker and Kubernetes?', '00000000-0000-0000-0000-000000000002', 33, 220),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440007', 'How do I improve the performance of a React application?', '00000000-0000-0000-0000-000000000003', 41, 300),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440008', 'What is the purpose of garbage collection in Java?', '00000000-0000-0000-0000-000000000004', 24, 130),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440009', 'How do I implement OAuth 2.0 in a web application?', '00000000-0000-0000-0000-000000000003', 35, 270),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440010', 'What is the difference between SQL and NoSQL databases?', '00000000-0000-0000-0000-000000000001', 29, 210),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440011', 'How do I write unit tests for a Python application?', '00000000-0000-0000-0000-000000000002', 40, 320),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440012', 'What are the advantages of using GraphQL over REST?', '00000000-0000-0000-0000-000000000003', 38, 260),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440013', 'How do I set up a CI/CD pipeline using GitHub Actions?', '00000000-0000-0000-0000-000000000004', 37, 240),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440014', 'What is the purpose of dependency injection in Spring Boot?', '00000000-0000-0000-0000-000000000003', 31, 190),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440015', 'How do I secure a Django application against common vulnerabilities?', '00000000-0000-0000-0000-000000000001', 42, 330),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440016', 'What are the differences between synchronous and asynchronous programming?', '00000000-0000-0000-0000-000000000002', 34, 280),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440017', 'How do I use WebSockets for real-time communication?', '00000000-0000-0000-0000-000000000003', 36, 290),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440018', 'What are the best practices for handling errors in Node.js?', '00000000-0000-0000-0000-000000000004', 43, 350),
                                                                                                    ('550e8400-e29b-41d4-a716-446655440019', 'How do I design a scalable microservices architecture?', '00000000-0000-0000-0000-000000000003', 48, 390);


INSERT INTO `stackoverflow_lite`.`Answers` (`Id`, `Content`, `Score`, `UserId`, `QuestionId`, `IsVisible`, `TextCategory`) VALUES
                                                                                                                               ('550e8400-e29b-41d4-a716-446655440020', 'A deep copy creates an independent clone, while a shallow copy links to the original.', 25, '00000000-0000-0000-0000-000000000001', '550e8400-e29b-41d4-a716-446655440000', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655440021', 'Optimize SQL queries by using indexed columns and avoiding unnecessary joins.', 18, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440001', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441022', 'TypeScript adds type safety and helps with code maintainability.', 30, '00000000-0000-0000-0000-000000000002', '550e8400-e29b-41d4-a716-446655440001', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441003', 'Use tools like GDB to debug segmentation faults effectively.', 27, '00000000-0000-0000-0000-000000000004', '550e8400-e29b-41d4-a716-446655440001', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441004', 'Follow the principles of statelessness and proper URI structure for REST APIs.', 35, '00000000-0000-0000-0000-000000000001', '550e8400-e29b-41d4-a716-446655440004', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441005', 'Executors in Java simplify thread management and enhance concurrency.', 22, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440002', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441006', 'Docker is for containerization, Kubernetes is for orchestrating containers.', 29, '00000000-0000-0000-0000-000000000002', '550e8400-e29b-41d4-a716-446655440004', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441007', 'Use memoization and avoid unnecessary re-renders in React.', 24, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440007', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441008', 'Garbage collection automates memory management and prevents memory leaks.', 19, '00000000-0000-0000-0000-000000000004', '550e8400-e29b-41d4-a716-446655440009', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441009', 'Use OAuth libraries for secure and standardized implementation.', 28, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440009', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441010', 'SQL databases excel in structured data, NoSQL is great for flexible schemas.', 23, '00000000-0000-0000-0000-000000000001', '550e8400-e29b-41d4-a716-446655440010', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441011', 'Use Pytest or unittest modules for testing in Python.', 26, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440011', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441012', 'GraphQL offers better control and reduces over-fetching compared to REST.', 31, '00000000-0000-0000-0000-000000000002', '550e8400-e29b-41d4-a716-446655440010', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441013', 'GitHub Actions provide seamless CI/CD pipeline integration.', 27, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440013', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441014', 'Dependency injection promotes testability and loose coupling.', 21, '00000000-0000-0000-0000-000000000004', '550e8400-e29b-41d4-a716-446655440015', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441015', 'Secure Django apps with tools like Django Security Middleware.', 33, '00000000-0000-0000-0000-000000000001', '550e8400-e29b-41d4-a716-446655440015', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441016', 'Asynchronous programming improves scalability and responsiveness.', 34, '00000000-0000-0000-0000-000000000002', '550e8400-e29b-41d4-a716-446655440015', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441017', 'WebSockets enable real-time data transmission between client and server.', 29, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440015', 1, 2),
                                                                                                                               ('550e8400-e29b-41d4-a716-446655441018', 'Handle Node.js errors with centralized error-handling middleware.', 32, '00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440015', 1, 2);

INSERT INTO `stackoverflow_lite`.`UserQuestionViews` (`UserId`, `QuestionId`) VALUES
    ('00000000-0000-0000-0000-000000000001', '550e8400-e29b-41d4-a716-446655440012'),
    ('00000000-0000-0000-0000-000000000001', '550e8400-e29b-41d4-a716-446655440013'),
    ('00000000-0000-0000-0000-000000000002', '550e8400-e29b-41d4-a716-446655440012'),
    ('00000000-0000-0000-0000-000000000003', '550e8400-e29b-41d4-a716-446655440012'),
    ('00000000-0000-0000-0000-000000000004', '550e8400-e29b-41d4-a716-446655440012');

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

UPDATE Questions
SET `isVisible` = '1',
    `TextCategory` = '2';

UPDATE Answers
SET `isVisible` = '1',
    `TextCategory` = '2';
