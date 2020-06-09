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
--where ord.date > '2020-04-01';

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