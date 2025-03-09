INSERT INTO "Users" ("Id") 
SELECT generate_series(1, 1000);

INSERT INTO "UserConnections" ("UserId", "IpAddress", "ConnectedAt")
SELECT 
    (i % 1000) + 1,
    '192.168.' || (random() * 255)::int || '.' || (random() * 255)::int,
    NOW() - (random() * interval '30 days')
FROM generate_series(1, 500000) AS i;


--psql -U postgres -d UserIPTrackerDB -f sql/seed_test_data.sql
