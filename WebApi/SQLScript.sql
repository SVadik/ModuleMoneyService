CREATE DATABASE moduledb
  WITH OWNER = postgres
    ENCODING = 'UTF8'
    TABLESPACE = pg_default;

CREATE SEQUENCE public.users_id_seq
  INCREMENT 1 MINVALUE 1
  MAXVALUE 2147483647 START 1
  CACHE 1;

ALTER SEQUENCE public.users_id_seq RESTART WITH 1;

CREATE TABLE public."Users" (
  "Id" INTEGER DEFAULT nextval('public.users_id_seq'::text::regclass) NOT NULL,
  "Username" VARCHAR(355) NOT NULL,
  "Firstname" VARCHAR(500) NOT NULL,
  "Salt" VARCHAR(128) NOT NULL,
  "Password" VARCHAR(255) NOT NULL,
  CONSTRAINT "Users_Username_key" UNIQUE("Username"),
  CONSTRAINT "Users_pkey" PRIMARY KEY("Id")
) ;


ALTER TABLE public."Users"
  OWNER TO postgres;