SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

DROP SCHEMA IF EXISTS `CarGlass` ;
CREATE SCHEMA IF NOT EXISTS `CarGlass` DEFAULT CHARACTER SET utf8 ;
USE `CarGlass` ;

-- -----------------------------------------------------
-- Table `CarGlass`.`users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`users` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `login` VARCHAR(45) NOT NULL,
  `description` TEXT NULL DEFAULT NULL,
  `admin` TINYINT(1) NOT NULL DEFAULT FALSE,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `CarGlass`.`base_parameters`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`base_parameters` (
  `name` VARCHAR(20) NOT NULL,
  `str_value` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`name`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `CarGlass`.`marks`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`marks` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`models`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`models` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `mark_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_models_1_idx` (`mark_id` ASC),
  CONSTRAINT `fk_models_1`
    FOREIGN KEY (`mark_id`)
    REFERENCES `CarGlass`.`marks` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`manufacturers`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`manufacturers` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`stocks`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`stocks` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`status` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `color` VARCHAR(15) NULL DEFAULT NULL,
  `usedtypes` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `index2` (`name` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`orders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`orders` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `date` DATE NOT NULL,
  `hour` TINYINT NOT NULL,
  `type` ENUM('install', 'tinting','repair') NOT NULL,
  `car_model_id` INT UNSIGNED NOT NULL,
  `car_year` YEAR NULL DEFAULT NULL,
  `phone` VARCHAR(45) NULL,
  `status_id` INT UNSIGNED NOT NULL,
  `manufacturer_id` INT UNSIGNED NULL DEFAULT NULL,
  `stock_id` INT UNSIGNED NULL DEFAULT NULL,
  `eurocode` VARCHAR(45) NULL DEFAULT NULL,
  `comment` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_orders_car_model_idx` (`car_model_id` ASC),
  INDEX `fk_orders_status_idx` (`status_id` ASC),
  INDEX `fk_orders_manufacturer_idx` (`manufacturer_id` ASC),
  INDEX `fk_orders_stock_idx` (`stock_id` ASC),
  CONSTRAINT `fk_orders_car_model`
    FOREIGN KEY (`car_model_id`)
    REFERENCES `CarGlass`.`models` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_orders_status`
    FOREIGN KEY (`status_id`)
    REFERENCES `CarGlass`.`status` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `fk_orders_manufacturer`
    FOREIGN KEY (`manufacturer_id`)
    REFERENCES `CarGlass`.`manufacturers` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE,
  CONSTRAINT `fk_orders_stock`
    FOREIGN KEY (`stock_id`)
    REFERENCES `CarGlass`.`stocks` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`services`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`services` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `order_type` ENUM('install', 'tinting','repair') NOT NULL,
  `price` DECIMAL UNSIGNED NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`glass`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`glass` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 1;


-- -----------------------------------------------------
-- Table `CarGlass`.`order_pays`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`order_pays` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `order_id` INT UNSIGNED NOT NULL,
  `service_id` INT UNSIGNED NOT NULL,
  `cost` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  INDEX `fk_order_pays_order_idx` (`order_id` ASC),
  INDEX `fk_order_pays_service_idx` (`service_id` ASC),
  CONSTRAINT `fk_order_pays_order`
    FOREIGN KEY (`order_id`)
    REFERENCES `CarGlass`.`orders` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_order_pays_service`
    FOREIGN KEY (`service_id`)
    REFERENCES `CarGlass`.`services` (`id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `CarGlass`.`order_glasses`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `CarGlass`.`order_glasses` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `order_id` INT UNSIGNED NOT NULL,
  `glass_id` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `order_glass` (`order_id` ASC, `glass_id` ASC),
  INDEX `fk_table1_glass_idx` (`glass_id` ASC),
  CONSTRAINT `fk_table1_order`
    FOREIGN KEY (`order_id`)
    REFERENCES `CarGlass`.`orders` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_table1_glass`
    FOREIGN KEY (`glass_id`)
    REFERENCES `CarGlass`.`glass` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
AUTO_INCREMENT = 1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `CarGlass`.`base_parameters`
-- -----------------------------------------------------
START TRANSACTION;
USE `CarGlass`;
INSERT INTO `CarGlass`.`base_parameters` (`name`, `str_value`) VALUES ('product_name', 'CarGlass');
INSERT INTO `CarGlass`.`base_parameters` (`name`, `str_value`) VALUES ('version', '1.0');
INSERT INTO `CarGlass`.`base_parameters` (`name`, `str_value`) VALUES ('edition', 'gpl');

COMMIT;

