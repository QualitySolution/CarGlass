-- -----------------------------------------------------
-- Table `CarGlass`.`order_types`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `order_type` (
 `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `is_calculate_salary` BOOLEAN NOT NULL DEFAULT 0,
  `position_in_tabs` varchar(1000) NULL DEFAULT null,
  `is_show_main_widgets` BOOLEAN NOT NULL DEFAULT 1,
  `is_show_additional_widgets` BOOLEAN NOT NULL DEFAULT 0,
  `is_install_type` BOOLEAN NOT NULL DEFAULT 0,
  `is_other_type` BOOLEAN NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

-- -----------------------------------------------------
-- Table `CarGlass`.`service_order_type`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `service_order_type` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_service` INT(10) UNSIGNED NOT NULL,
  `id_type_order` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_service_order_type_1_idx` (`id_service` ASC),
  INDEX `fk_service_order_type_2_idx` (`id_type_order` ASC),
  CONSTRAINT `fk_service_order_type_1`
    FOREIGN KEY (`id_service`)
    REFERENCES `services` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_service_order_type_2`
    FOREIGN KEY (`id_type_order`)
    REFERENCES `order_type` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

-- -----------------------------------------------------
-- Alter table `CarGlass`.`orders`
-- -----------------------------------------------------

ALTER TABLE `orders` 
ADD COLUMN `id_order_type` INT(10) UNSIGNED NULL DEFAULT NULL AFTER `comment`,
ADD INDEX `fk_order_type_idx` (`id_order_type` ASC);
ALTER TABLE `orders` 
ADD CONSTRAINT `fk_order_type`
  FOREIGN KEY (`id_order_type`)
  REFERENCES `order_type` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

-- -----------------------------------------------------
-- Alter table `CarGlass`.`note`
-- -----------------------------------------------------

CREATE TABLE IF NOT EXISTS `note` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `date` DATETIME NOT NULL,
  `point_number` TINYINT(4) NOT NULL,
  `calendar_number` TINYINT(4) NOT NULL,
  `message` VARCHAR(2000) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

-- В заказы добавить order_type
INSERT INTO order_type (name, is_calculate_salary, position_in_tabs, is_show_main_widgets,
is_show_additional_widgets, is_install_type, is_other_type) 
VALUES( 'Установка стекол', false, 'Установка пригородный Заказные', true, true, true, false);

INSERT INTO order_type (name, is_calculate_salary, position_in_tabs, is_show_main_widgets,
is_show_additional_widgets, is_install_type, is_other_type) 
VALUES( 'Тонировка', true, 'Тонировка пригородный Тонировка въезд ', true, false, false, false);

INSERT INTO order_type (name, is_calculate_salary, position_in_tabs, is_show_main_widgets,
is_show_additional_widgets, is_install_type, is_other_type) 
VALUES( 'Полировка', false, 'Установка пригородный Тонировка пригородный Заказные Тонировка въезд ', true, false, false, false);

INSERT INTO order_type (name, is_calculate_salary, position_in_tabs, is_show_main_widgets,
is_show_additional_widgets, is_install_type, is_other_type) 
VALUES( 'Бронировка', false, 'Установка пригородный Тонировка пригородный Заказные Тонировка въезд ', true, false, false, false);

INSERT INTO order_type (name, is_calculate_salary, position_in_tabs, is_show_main_widgets,
is_show_additional_widgets, is_install_type, is_other_type) 
VALUES( 'Прочее', false, 'Установка пригородный Тонировка пригородный Заказные Тонировка въезд ', true, false, false, true);

-- Для обновления данных в таблице заказов. 
UPDATE orders ord
INNER JOIN order_type ordt ON ord.type = ordt.name
SET ord.id_order_type = ordt.id;

-- В service_order_type вставить все виды работ с их типами из таблицы server
INSERT INTO service_order_type (id_service, id_type_order) 
 select ser.id, ord.id from services ser, order_type ord
 where ser.order_type = 'install' and ord.name = 'Установка стекол';
 
INSERT INTO service_order_type (id_service, id_type_order) 
 select ser.id, ord.id from services ser, order_type ord
 where ser.order_type = 'tinting' and ord.name = 'Тонировка';
 
 INSERT INTO service_order_type (id_service, id_type_order) 
 select ser.id, ord.id from services ser, order_type ord
 where ser.order_type = 'polishing' and ord.name = 'Полировка';
 
 INSERT INTO service_order_type (id_service, id_type_order) 
 select ser.id, ord.id from services ser, order_type ord
 where ser.order_type = 'armoring' and ord.name = 'Бронировка';

-- Изменить в таблице status столбец usedtypes. Исправить наименования типов заказов на те, что будут в справочниках.
update status set usedtypes = 'Установка стекол' where name = 'Заказан';
update status set usedtypes = 'Установка стекол,Тонировка' where name = 'На складе';
update status set usedtypes = 'Установка стекол' where name = 'Не пришло';
update status set usedtypes = 'Установка стекол,Ремонт сколов' where name = 'Ремонт скола';
update status set usedtypes = 'Установка стекол,Полировка' where name = 'Полировка';
update status set usedtypes = 'Установка стекол' where name = 'Скол';
update status set usedtypes = 'Тонировка,Бронировка' where name = 'Броня';
update status set usedtypes = 'Установка стекол' where name = 'Не приехал';
update status set usedtypes = 'Прочее' where name = 'Прочее';

DELETE FROM base_parameters WHERE name = 'micro_updates';
UPDATE base_parameters SET str_value = '1.6' WHERE name = 'version';