ALTER TABLE `CarGlass`.`orders` 
ADD COLUMN `created_by` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `point_number`,
ADD INDEX `fk_orders_1_idx` (`created_by` ASC);

ALTER TABLE `CarGlass`.`orders` 
ADD CONSTRAINT `fk_orders_1`
  FOREIGN KEY (`created_by`)
  REFERENCES `CarGlass`.`users` (`id`)
  ON DELETE SET NULL
  ON UPDATE CASCADE;
