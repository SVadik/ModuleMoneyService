CREATE DATABASE moduledb
  WITH OWNER = postgres
    ENCODING = 'UTF8'
    TABLESPACE = pg_default;

CREATE SEQUENCE public.users_id_seq
  INCREMENT 1 MINVALUE 1
  MAXVALUE 2147483647 START 1
  CACHE 1;

ALTER SEQUENCE public.users_id_seq RESTART WITH 1;

CREATE TABLE public.users (
  id SERIAL,
  username VARCHAR(355) NOT NULL,
  firstname VARCHAR(500) NOT NULL,
  salt VARCHAR(128) NOT NULL,
  password VARCHAR(255) NOT NULL,
  CONSTRAINT "Users_Username_key" UNIQUE(username),
  CONSTRAINT "Users_pkey" PRIMARY KEY(id)
) ;


ALTER TABLE public.users
  OWNER TO postgres;

CREATE SEQUENCE accounts_number_seq
  INCREMENT 1
  MINVALUE 4000000000
  MAXVALUE 9223372036854775807
  START 4000000000
  CACHE 1;

CREATE TABLE public.accounts (
  number BIGSERIAL,
  user_id INTEGER NOT NULL,
  balance NUMERIC(15,4) NOT NULL,
  CONSTRAINT accounts_pkey PRIMARY KEY(number),
  CONSTRAINT accounts_user_id_fkey FOREIGN KEY (user_id)
    REFERENCES public.users(id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION
    NOT DEFERRABLE
) ;


ALTER TABLE public.accounts
  OWNER TO postgres;

CREATE TABLE public.money_transactions (
  id BIGSERIAL NOT NULL,
  date TIMESTAMP(0) WITH TIME ZONE,
  from_number BIGINT,
  to_number BIGINT,
  from_userid INTEGER,
  to_userid INTEGER,
  amount NUMERIC(15,4),
  PRIMARY KEY(id)
) ;