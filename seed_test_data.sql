-- Bulk Insert 500,000 Test Records for UserId = 1

INSERT INTO "UserConnections" ("UserId", "IpAddress", "ConnectedAt")
SELECT 1, '192.168.1.' || (random() * 255)::int, NOW()
FROM generate_series(1, 500000);

--psql -U postgres -d UserIPTrackerDB -f sql/seed_test_data.sql
