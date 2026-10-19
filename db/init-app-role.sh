#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username postgres --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE ROLE mopjesshopjes_app WITH LOGIN PASSWORD '$APP_DB_PASSWORD';
    GRANT CONNECT ON DATABASE $POSTGRES_DB TO mopjesshopjes_app;
    GRANT USAGE, CREATE ON SCHEMA public TO mopjesshopjes_app;
EOSQL