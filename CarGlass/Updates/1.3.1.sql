ALTER TABLE `CarGlass`.`orders` 
ADD COLUMN `created_by` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `point_number`,
CHANGE COLUMN `type` `type` ENUM('install', 'tinting', 'repair', 'polishing', 'armoring') NOT NULL,
CHANGE COLUMN `point_number` `point_number` TINYINT(3) UNSIGNED NOT NULL DEFAULT 1 ,
ADD COLUMN `calendar_number` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0 AFTER `point_number`,
ADD INDEX `fk_orders_1_idx` (`created_by` ASC);

ALTER TABLE `CarGlass`.`orders` 
ADD CONSTRAINT `fk_orders_1`
  FOREIGN KEY (`created_by`)
  REFERENCES `CarGlass`.`users` (`id`)
  ON DELETE SET NULL
  ON UPDATE CASCADE;

INSERT INTO `glass` VALUES (11,'Молдинги'),(12,'Дворники');

ALTER TABLE `CarGlass`.`users` 
ADD COLUMN `last_read_chat` DATETIME NULL DEFAULT NULL AFTER `admin`;

ALTER TABLE `CarGlass`.`services` 
CHANGE COLUMN `order_type` `order_type` ENUM('install', 'tinting', 'repair', 'polishing', 'armoring') NOT NULL;

UPDATE orders SET orders.calendar_number =( CASE orders.type WHEN 'install' THEN 1 WHEN 'tinting' THEN 2 WHEN 'repair' THEN 2 END );