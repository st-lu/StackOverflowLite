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

-- Setări pentru import
SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

INSERT INTO `stackoverflow_lite`.`Users` (`Id`, `Username`) VALUES
                                                                 ('00000000-0000-0000-0000-000000000001', 'Alice'),
                                                                 ('00000000-0000-0000-0000-000000000002', 'Bob'),
                                                                 ('00000000-0000-0000-0000-000000000003', 'Charlie'),
                                                                 ('00000000-0000-0000-0000-000000000004', 'David'),
                                                                 ('00000000-0000-0000-0000-000000000005', 'Eve');

INSERT INTO `stackoverflow_lite`.`Questions` (`Id`, `Content`, `UserId`, `Score`, `ViewsCount`) VALUES
                                                                                                     ('00000000-0000-0000-0000-000000000201', 'What is the meaning of life?', '00000000-0000-0000-0000-000000000001', 15, 120),
                                                                                                     ('00000000-0000-0000-0000-000000000202', 'How do we define happiness?', '00000000-0000-0000-0000-000000000002', 18, 110),
                                                                                                     ('00000000-0000-0000-0000-000000000203', 'What is love?', '00000000-0000-0000-0000-000000000003', 22, 130),
                                                                                                     ('00000000-0000-0000-0000-000000000204', 'How can one achieve peace of mind?', '00000000-0000-0000-0000-000000000004', 12, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000205', 'What is the purpose of education?', '00000000-0000-0000-0000-000000000005', 25, 140),
                                                                                                     ('00000000-0000-0000-0000-000000000206', 'What is the secret to success?', '00000000-0000-0000-0000-000000000001', 17, 95),
                                                                                                     ('00000000-0000-0000-0000-000000000207', 'How do we find true love?', '00000000-0000-0000-0000-000000000002', 20, 105),
                                                                                                     ('00000000-0000-0000-0000-000000000208', 'What makes a person wise?', '00000000-0000-0000-0000-000000000003', 16, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000209', 'How does one achieve financial freedom?', '00000000-0000-0000-0000-000000000004', 13, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000210', 'What are the keys to a happy life?', '00000000-0000-0000-0000-000000000005', 21, 115),
                                                                                                     ('00000000-0000-0000-0000-000000000211', 'How do you overcome fear?', '00000000-0000-0000-0000-000000000001', 14, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000212', 'What is true friendship?', '00000000-0000-0000-0000-000000000002', 19, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000213', 'What does it mean to be kind?', '00000000-0000-0000-0000-000000000003', 15, 60),
                                                                                                     ('00000000-0000-0000-0000-000000000214', 'How do we find inner strength?', '00000000-0000-0000-0000-000000000004', 18, 95),
                                                                                                     ('00000000-0000-0000-0000-000000000215', 'What is the role of family in life?', '00000000-0000-0000-0000-000000000005', 22, 120),
                                                                                                     ('00000000-0000-0000-0000-000000000216', 'How do you maintain work-life balance?', '00000000-0000-0000-0000-000000000001', 17, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000217', 'What makes a good leader?', '00000000-0000-0000-0000-000000000002', 20, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000218', 'How can one live sustainably?', '00000000-0000-0000-0000-000000000003', 19, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000219', 'What are the benefits of meditation?', '00000000-0000-0000-0000-000000000004', 23, 100),
                                                                                                     ('00000000-0000-0000-0000-000000000220', 'How do we measure success?', '00000000-0000-0000-0000-000000000005', 16, 65),
                                                                                                     ('00000000-0000-0000-0000-000000000221', 'What is the impact of technology on society?', '00000000-0000-0000-0000-000000000001', 14, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000222', 'How do we handle stress?', '00000000-0000-0000-0000-000000000002', 18, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000223', 'What is the value of education?', '00000000-0000-0000-0000-000000000003', 21, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000224', 'How does one find purpose?', '00000000-0000-0000-0000-000000000004', 13, 60),
                                                                                                     ('00000000-0000-0000-0000-000000000225', 'What is the importance of community?', '00000000-0000-0000-0000-000000000005', 17, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000226', 'How can one develop resilience?', '00000000-0000-0000-0000-000000000001', 20, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000227', 'What is the role of creativity in life?', '00000000-0000-0000-0000-000000000002', 22, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000228', 'How do we deal with failure?', '00000000-0000-0000-0000-000000000003', 15, 65),
                                                                                                     ('00000000-0000-0000-0000-000000000229', 'What are the benefits of traveling?', '00000000-0000-0000-0000-000000000004', 18, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000230', 'How does one achieve personal growth?', '00000000-0000-0000-0000-000000000005', 23, 95),
                                                                                                     ('00000000-0000-0000-0000-000000000232', 'How do we create meaningful relationships?', '00000000-0000-0000-0000-000000000001', 20, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000233', 'What are the key elements of a successful career?', '00000000-0000-0000-0000-000000000002', 25, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000234', 'How can one find balance between work and life?', '00000000-0000-0000-0000-000000000003', 17, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000235', 'What is the role of spirituality in personal development?', '00000000-0000-0000-0000-000000000004', 22, 100),
                                                                                                     ('00000000-0000-0000-0000-000000000236', 'How can we contribute to a better world?', '00000000-0000-0000-0000-000000000005', 18, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000237', 'What are the keys to effective communication?', '00000000-0000-0000-0000-000000000001', 16, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000238', 'How do we overcome self-doubt?', '00000000-0000-0000-0000-000000000002', 19, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000239', 'What are the benefits of lifelong learning?', '00000000-0000-0000-0000-000000000003', 21, 95),
                                                                                                     ('00000000-0000-0000-0000-000000000240', 'How can we foster creativity in ourselves and others?', '00000000-0000-0000-0000-000000000004', 20, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000241', 'What role does empathy play in personal relationships?', '00000000-0000-0000-0000-000000000005', 24, 105),
                                                                                                     ('00000000-0000-0000-0000-000000000242', 'How can we effectively manage our time?', '00000000-0000-0000-0000-000000000001', 15, 65),
                                                                                                     ('00000000-0000-0000-0000-000000000243', 'What are some strategies for overcoming procrastination?', '00000000-0000-0000-0000-000000000002', 19, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000244', 'How do we maintain motivation in long-term projects?', '00000000-0000-0000-0000-000000000003', 22, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000245', 'What is the importance of setting goals?', '00000000-0000-0000-0000-000000000004', 17, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000246', 'How do we handle criticism constructively?', '00000000-0000-0000-0000-000000000005', 20, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000247', 'What is the value of self-reflection?', '00000000-0000-0000-0000-000000000001', 18, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000248', 'How do we develop good habits?', '00000000-0000-0000-0000-000000000002', 21, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000249', 'What makes a person trustworthy?', '00000000-0000-0000-0000-000000000003', 16, 65),
                                                                                                     ('00000000-0000-0000-0000-000000000250', 'How do we balance ambition with contentment?', '00000000-0000-0000-0000-000000000004', 19, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000251', 'What role does gratitude play in happiness?', '00000000-0000-0000-0000-000000000005', 22, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000252', 'How can we improve our problem-solving skills?', '00000000-0000-0000-0000-000000000001', 15, 60),
                                                                                                     ('00000000-0000-0000-0000-000000000253', 'What is the importance of resilience?', '00000000-0000-0000-0000-000000000002', 18, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000254', 'How can we cultivate a positive mindset?', '00000000-0000-0000-0000-000000000003', 21, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000255', 'What are some ways to build self-confidence?', '00000000-0000-0000-0000-000000000004', 20, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000256', 'How do we develop effective leadership skills?', '00000000-0000-0000-0000-000000000005', 22, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000257', 'What is the role of perseverance in achieving goals?', '00000000-0000-0000-0000-000000000001', 17, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000258', 'How can we improve our decision-making abilities?', '00000000-0000-0000-0000-000000000002', 19, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000259', 'What makes a fulfilling life?', '00000000-0000-0000-0000-000000000003', 21, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000260', 'How do we manage change effectively?', '00000000-0000-0000-0000-000000000004', 18, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000261', 'What is the importance of having hobbies?', '00000000-0000-0000-0000-000000000005', 23, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000262', 'How can we foster a culture of collaboration?', '00000000-0000-0000-0000-000000000001', 20, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000263', 'What is the role of self-discipline in success?', '00000000-0000-0000-0000-000000000002', 22, 95),
                                                                                                     ('00000000-0000-0000-0000-000000000264', 'How do we create a meaningful career?', '00000000-0000-0000-0000-000000000003', 19, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000265', 'What is the value of mindfulness?', '00000000-0000-0000-0000-000000000004', 15, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000266', 'How can we improve interpersonal skills?', '00000000-0000-0000-0000-000000000005', 18, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000267', 'What makes a relationship successful?', '00000000-0000-0000-0000-000000000001', 21, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000268', 'How do we handle difficult conversations?', '00000000-0000-0000-0000-000000000002', 19, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000269', 'What are the benefits of volunteering?', '00000000-0000-0000-0000-000000000003', 22, 90),
                                                                                                     ('00000000-0000-0000-0000-000000000270', 'How do we stay motivated during setbacks?', '00000000-0000-0000-0000-000000000004', 20, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000271', 'What is the significance of goal setting?', '00000000-0000-0000-0000-000000000005', 23, 85),
                                                                                                     ('00000000-0000-0000-0000-000000000272', 'How can we develop emotional intelligence?', '00000000-0000-0000-0000-000000000001', 18, 70),
                                                                                                     ('00000000-0000-0000-0000-000000000273', 'What are the keys to building trust?', '00000000-0000-0000-0000-000000000002', 21, 80),
                                                                                                     ('00000000-0000-0000-0000-000000000274', 'How do we create a positive work environment?', '00000000-0000-0000-0000-000000000003', 19, 75),
                                                                                                     ('00000000-0000-0000-0000-000000000275', 'What is the role of curiosity in learning?', '00000000-0000-0000-0000-000000000004', 20, 85);

INSERT INTO `stackoverflow_lite`.`Answers` (`Id`, `Content`, `Score`, `UserId`, `QuestionId`) VALUES
                                                                                                   ('00000000-0000-0000-0000-000000000301', 'Finding meaning is a personal journey.', 10, '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000201'),
                                                                                                   ('00000000-0000-0000-0000-000000000302', 'Happiness often comes from within.', 8, '00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000202'),
                                                                                                   ('00000000-0000-0000-0000-000000000303', 'Love is caring deeply for someone.', 12, '00000000-0000-0000-0000-000000000004', '00000000-0000-0000-0000-000000000203'),
                                                                                                   ('00000000-0000-0000-0000-000000000304', 'Peace of mind is achieved through balance.', 7, '00000000-0000-0000-0000-000000000005', '00000000-0000-0000-0000-000000000204'),
                                                                                                   ('00000000-0000-0000-0000-000000000305', 'Education opens doors and opportunities.', 15, '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000205'),
                                                                                                   ('00000000-0000-0000-0000-000000000306', 'Success requires persistence and hard work.', 10, '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000206'),
                                                                                                   ('00000000-0000-0000-0000-000000000307', 'True love involves mutual respect and care.', 12, '00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000207'),
                                                                                                   ('00000000-0000-0000-0000-000000000308', 'Wisdom comes from experience and learning.', 14, '00000000-0000-0000-0000-000000000004', '00000000-0000-0000-0000-000000000208'),
                                                                                                   ('00000000-0000-0000-0000-000000000309', 'Financial freedom requires smart planning.', 9, '00000000-0000-0000-0000-000000000005', '00000000-0000-0000-0000-000000000209'),
                                                                                                   ('00000000-0000-0000-0000-000000000310', 'A happy life involves fulfillment and joy.', 18, '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000210'),
                                                                                                   ('00000000-0000-0000-0000-000000000311', 'Overcoming fear involves facing it directly.', 11, '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000211'),
                                                                                                   ('00000000-0000-0000-0000-000000000312', 'True friendship is built on trust.', 13, '00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000212'),
                                                                                                   ('00000000-0000-0000-0000-000000000313', 'Kindness involves being considerate to others.', 15, '00000000-0000-0000-0000-000000000004', '00000000-0000-0000-0000-000000000213'),
                                                                                                   ('00000000-0000-0000-0000-000000000314', 'Inner strength is developed through challenges.', 10, '00000000-0000-0000-0000-000000000005', '00000000-0000-0000-0000-000000000214'),
                                                                                                   ('00000000-0000-0000-0000-000000000315', 'Family provides support and love.', 12, '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000215'),
                                                                                                   ('00000000-0000-0000-0000-000000000316', 'Work-life balance involves setting boundaries.', 14, '00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000216'),
                                                                                                   ('00000000-0000-0000-0000-000000000317', 'Good leadership involves guiding others.', 16, '00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000217'),
INSERT INTO `stackoverflow_lite`.`UserQuestionViews` (`UserId`, `QuestionId`) VALUES
    ('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000101'),
    ('00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000102'),
    ('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000103'),
    ('00000000-0000-0000-0000-000000000003', '00000000-0000-0000-0000-000000000104'),
    ('00000000-0000-0000-0000-000000000004', '00000000-0000-0000-0000-000000000105');

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;