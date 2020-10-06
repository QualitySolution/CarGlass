-- -----------------------------------------------------
-- Alter table `CarGlass`.`orders`
-- -----------------------------------------------------
ALTER TABLE `orders` 
CHANGE COLUMN `warranty_install` `warranty_install` ENUM('None', 'SixMonth', 'OneYear', 'TwoYear', 'ThreeYear', 'Indefinitely', 'NoWarranty') NOT NULL DEFAULT 'None' ,
CHANGE COLUMN `warranty_tinting` `warranty_tinting` ENUM('None', 'SixMonth', 'OneYear', 'TwoYear', 'ThreeYear', 'Indefinitely', 'NoWarranty') NOT NULL DEFAULT 'None' ,
CHANGE COLUMN `warranty_armoring` `warranty_armoring` ENUM('None', 'SixMonth', 'OneYear', 'TwoYear', 'ThreeYear', 'Indefinitely', 'NoWarranty') NOT NULL DEFAULT 'None' ,
CHANGE COLUMN `warranty_pasting` `warranty_pasting` ENUM('None', 'SixMonth', 'OneYear', 'TwoYear', 'ThreeYear', 'Indefinitely', 'NoWarranty') NOT NULL DEFAULT 'None' ;
