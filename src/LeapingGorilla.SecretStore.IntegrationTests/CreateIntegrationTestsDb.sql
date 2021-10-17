/* You should use this SQL file to generate a test database which will be used for 
Secret Store integration tests. This should be accomplished prior to running the
integration tests.

DROP OWNED BY ss_integration_test_ro CASCADE;
DROP OWNED BY ss_integration_test_rw CASCADE;
DROP USER IF EXISTS ss_integration_test_ro;
DROP USER IF EXISTS ss_integration_test_rw;
DROP DATABASE IF EXISTS secretstore_integrationtests;
*/

-- Run first to create DB
CREATE DATABASE secretstore_integrationtests;
CREATE USER ss_integration_test_ro WITH ENCRYPTED PASSWORD 'concise-cornfield-celery-defiling-brethren-drizzle';
CREATE USER ss_integration_test WITH ENCRYPTED PASSWORD 'managing-octane-job-rebate-scolding-schematic';
GRANT CONNECT ON DATABASE secretstore_integrationtests TO ss_integration_test_ro;
GRANT CONNECT ON DATABASE secretstore_integrationtests TO ss_integration_test;
GRANT ALL PRIVILEGES ON DATABASE secretstore_integrationtests TO ss_integration_test;

-- Run these in the context of the DB as the RW user
SET ROLE ss_integration_test;
GRANT USAGE ON SCHEMA public TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT ON TABLES TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT EXECUTE ON FUNCTIONS TO ss_integration_test_ro;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE ON TYPES TO ss_integration_test_ro;
RESET ROLE;