CREATE TABLE users
(
    user_id serial PRIMARY key,
    login text NOT NULL UNIQUE,
    password text NOT NULL,
    CHECK (btrim(login) <> '' AND btrim(password) <> '')
)