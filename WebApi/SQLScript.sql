CREATE DATABASE moduledb
  WITH OWNER = postgres
    ENCODING = 'UTF8'
    TABLESPACE = pg_default;

CREATE TABLE public."Users" (
  "Id" INTEGER NOT NULL,
  "Username" VARCHAR(355) NOT NULL,
  "Firstname" VARCHAR(500) NOT NULL,
  "Salt" VARCHAR(32) NOT NULL,
  "PasswordHash" VARCHAR(255) NOT NULL,
  CONSTRAINT "Users_Username_key" UNIQUE("Username"),
  CONSTRAINT "Users_pkey" PRIMARY KEY("Id")
) ;


ALTER TABLE public."Users"
  OWNER TO postgres;