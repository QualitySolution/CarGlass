CREATE TABLE IF NOT EXISTS `employees` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `first_name` VARCHAR(45) NOT NULL,
  `last_name` VARCHAR(45) NULL DEFAULT NULL,
  `patronymic` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `shedule_works` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `date_work` DATE NOT NULL,
  `point_number` TINYINT(4) NOT NULL,
  `calendar_number` TINYINT(4) NOT NULL,
  `id_creator` INT(10) UNSIGNED NOT NULL,
  `date_create` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC),
  INDEX `fk_shedule_works_2_idx` (`id_creator` ASC),
  CONSTRAINT `fk_shedule_works_2`
    FOREIGN KEY (`id_creator`)
    REFERENCES `users` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `shedule_employee_works` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_shedule_works` INT(10) UNSIGNED NOT NULL,
  `id_employee` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_shedule_employee_works_1_idx` (`id_shedule_works` ASC),
  INDEX `fk_shedule_employee_works_2_idx` (`id_employee` ASC),
  CONSTRAINT `fk_shedule_employee_works_1`
    FOREIGN KEY (`id_shedule_works`)
    REFERENCES `shedule_works` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_shedule_employee_works_2`
    FOREIGN KEY (`id_employee`)
    REFERENCES `employees` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `status_employee` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` INT(11) NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `comment` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `employee_service_work` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_employee` INT(10) UNSIGNED NOT NULL,
  `id_order_pay` INT(10) UNSIGNED NOT NULL,
  `date_work` DATE NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_employee_service_work_1_idx` (`id_employee` ASC),
  INDEX `fk_employee_service_work_2_idx` (`id_order_pay` ASC),
  CONSTRAINT `fk_employee_service_work_1`
    FOREIGN KEY (`id_employee`)
    REFERENCES `employees` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_employee_service_work_2`
    FOREIGN KEY (`id_order_pay`)
    REFERENCES `order_pays` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `employee_status_history` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_employee` INT(10) UNSIGNED NOT NULL,
  `id_status` INT(10) UNSIGNED NOT NULL,
  `date_create` DATETIME NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_employee_status_history_1_idx` (`id_employee` ASC),
  INDEX `fk_employee_status_history_2_idx` (`id_status` ASC),
  CONSTRAINT `fk_employee_status_history_1`
    FOREIGN KEY (`id_employee`)
    REFERENCES `employees` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_employee_status_history_2`
    FOREIGN KEY (`id_status`)
    REFERENCES `status_employee` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `coefficients` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(15) NOT NULL,
  `comment` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `salary_formulas` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_service` INT(10) UNSIGNED NOT NULL,
  `comment` VARCHAR(45) NULL DEFAULT NULL,
  `formula` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_salary_formulas_1_idx` (`id_service` ASC),
  CONSTRAINT `fk_salary_formulas_1`
    FOREIGN KEY (`id_service`)
    REFERENCES `services` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE IF NOT EXISTS `employee_coeff` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `id_employee` INT(10) UNSIGNED NOT NULL,
  `id_coeff` INT(10) UNSIGNED NOT NULL,
  `value` DECIMAL NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_employee_coeff_2_idx` (`id_coeff` ASC),
  INDEX `fk_employee_coeff_1_idx` (`id_employee` ASC),
  CONSTRAINT `fk_employee_coeff_1`
    FOREIGN KEY (`id_employee`)
    REFERENCES `employees` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_employee_coeff_2`
    FOREIGN KEY (`id_coeff`)
    REFERENCES `coefficients` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

DELETE FROM base_parameters WHERE name = 'micro_updates';
UPDATE base_parameters SET str_value = '1.5' WHERE name = 'version';