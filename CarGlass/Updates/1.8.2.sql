ALTER TABLE `employee_service_work` 
DROP FOREIGN KEY `fk_employee_service_work_2`;

ALTER TABLE `sms_history` 
DROP FOREIGN KEY `fk_sms_history_1`;

ALTER TABLE `employee_service_work` 
ADD CONSTRAINT `fk_employee_service_work_2`
  FOREIGN KEY (`id_order_pay`)
  REFERENCES `order_pays` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;

ALTER TABLE `sms_history` 
ADD CONSTRAINT `fk_sms_history_1`
  FOREIGN KEY (`order_id`)
  REFERENCES `orders` (`id`)
  ON DELETE CASCADE
  ON UPDATE CASCADE;