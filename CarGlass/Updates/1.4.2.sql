ALTER TABLE `CarGlass`.`orders` 
DROP FOREIGN KEY `fk_orders_car_model`;

ALTER TABLE `CarGlass`.`orders` 
DROP FOREIGN KEY `fk_orders_status`;

ALTER TABLE `CarGlass`.`orders` 
CHANGE COLUMN `car_model_id` `car_model_id` INT(10) UNSIGNED NULL DEFAULT NULL ,
CHANGE COLUMN `status_id` `status_id` INT(10) UNSIGNED NULL DEFAULT NULL ,
CHANGE COLUMN `type` `type` ENUM('install', 'tinting', 'repair', 'polishing', 'armoring', 'other') NOT NULL ;

ALTER TABLE `CarGlass`.`orders` ADD CONSTRAINT `fk_orders_car_model`
  FOREIGN KEY (`car_model_id`)
  REFERENCES `CarGlass`.`models` (`id`)
  ON DELETE RESTRICT
  ON UPDATE CASCADE,
ADD CONSTRAINT `fk_orders_status`
  FOREIGN KEY (`status_id`)
  REFERENCES `CarGlass`.`status` (`id`)
  ON DELETE RESTRICT
  ON UPDATE CASCADE;