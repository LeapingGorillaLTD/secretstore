#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER ss_integration_test_ro WITH ENCRYPTED PASSWORD 'concise-cornfield-celery-defiling-brethren-drizzle';
    CREATE USER ss_integration_test WITH ENCRYPTED PASSWORD 'managing-octane-job-rebate-scolding-schematic';
    GRANT ss_integration_test_ro TO ss_integration_test;
    
    CREATE DATABASE ss_integration_test_db WITH ENCODING 'UTF8' OWNER ss_integration_test;
    ALTER DATABASE ss_integration_test_db SET TIMEZONE='UTC';

    \c ss_integration_test_db
    GRANT CONNECT ON DATABASE ss_integration_test_db TO ss_integration_test_ro; -- RW will inherit
    GRANT TEMP ON DATABASE ss_integration_test_db TO ss_integration_test_ro;

    GRANT ALL PRIVILEGES ON DATABASE ss_integration_test_db TO ss_integration_test WITH GRANT OPTION;


    GRANT USAGE ON SCHEMA public TO ss_integration_test_ro;
    ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT SELECT,REFERENCES     ON TABLES       TO ss_integration_test_ro;
    ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT ALL                   ON SEQUENCES    TO ss_integration_test_ro;
    ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT EXECUTE               ON FUNCTIONS    TO ss_integration_test_ro;
    ALTER DEFAULT PRIVILEGES FOR USER ss_integration_test IN SCHEMA public GRANT USAGE                 ON TYPES        TO ss_integration_test_ro;
    
    REVOKE ALL ON DATABASE ss_integration_test_db FROM public;
EOSQL
