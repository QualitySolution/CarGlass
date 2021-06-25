
ALTER TABLE `order_type` 
ADD COLUMN `name_accusative` VARCHAR(100) NULL DEFAULT NULL AFTER `name`;

CREATE TABLE IF NOT EXISTS `sms_history` (
  `id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `order_id` INT(10) UNSIGNED NOT NULL,
  `user_id` INT(10) UNSIGNED NULL DEFAULT NULL,
  `phone` VARCHAR(12) NOT NULL,
  `message_id` VARCHAR(72) NOT NULL,
  `sent_time` DATETIME NOT NULL,
  `last_status` VARCHAR(20) NULL DEFAULT NULL,
  `last_status_time` DATETIME NULL DEFAULT NULL,
  `text` TEXT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_sms_history_1_idx` (`order_id` ASC),
  INDEX `fk_sms_history_2_idx` (`user_id` ASC),
  CONSTRAINT `fk_sms_history_1`
    FOREIGN KEY (`order_id`)
    REFERENCES `orders` (`id`)
    ON DELETE NO ACTION
    ON UPDATE CASCADE,
  CONSTRAINT `fk_sms_history_2`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

DROP TABLE IF EXISTS `chat_history` ;

DELETE FROM base_parameters WHERE name = 'micro_updates';
DELETE FROM base_parameters WHERE name = 'edition';