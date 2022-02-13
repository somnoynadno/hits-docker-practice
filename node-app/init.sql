CREATE TABLE IF NOT EXISTS db.`data` (
	id INT NOT NULL AUTO_INCREMENT,
	`path` varchar(255) NOT NULL,
	name varchar(255) NOT NULL,
	author varchar(255) NULL,
	description varchar(2048) NULL,
	`date` TIMESTAMP DEFAULT now() NOT NULL,
	CONSTRAINT data_PK PRIMARY KEY (id)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4
COLLATE=utf8mb4_general_ci;
