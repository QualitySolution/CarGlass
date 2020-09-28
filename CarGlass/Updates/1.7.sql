-- -----------------------------------------------------
-- Table `CarGlass`.`settings`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `settings` (
  `id` INT(11) NOT NULL AUTO_INCREMENT,  
  `parametr` VARCHAR(45) NOT NULL,
  `value` VARCHAR(45) NOT NULL,
  `description` VARCHAR(45) NULL DEFAULT NULL,
  `date_edit` DATETIME NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

-- -----------------------------------------------------
-- Alter table `CarGlass`.`users`
-- -----------------------------------------------------

ALTER TABLE `users` 
ADD COLUMN `manager` TINYINT(1) NULL DEFAULT FALSE ,
ADD COLUMN `worker` TINYINT(1) NULL DEFAULT FALSE ;

ALTER TABLE `services` 
DROP COLUMN `order_type`;

DELETE FROM base_parameters WHERE name = 'micro_updates';
UPDATE base_parameters SET str_value = '1.7' WHERE name = 'version';