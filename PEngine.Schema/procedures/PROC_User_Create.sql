CREATE OR REPLACE PROCEDURE "User_Create" (
    assignedGroup UUID,
    newUsername   VARCHAR(32),
    newPasswordHash VARCHAR(256),
    newPasswordSalt VARCHAR(256),
    newDisplayName VARCHAR(32),
    newEmail VARCHAR(128)
) AS $$
    DECLARE
        newUuid UUID;
        newGroup UUID;
        newKeyring UUID;
    BEGIN
        newUuid := gen_random_uuid();
        newGroup := CASE WHEN assignedGroup IS NULL THEN newUuid ELSE newGroup END;

        INSERT INTO "Keyrings"("Salt", "Algorithm", "Key")
            VALUES (newPasswordSalt, 'SHA512', newPasswordHash) RETURNING "Id"
            INTO newKeyring;

        INSERT INTO "Users" ("Id", "Group", "Username", "DisplayName", "Email", "Keyring") VALUES
            (newUuid, newGroup, newUsername, newDisplayName, newEmail, newKeyring)
            ON CONFLICT DO NOTHING RETURNING "Id" INTO newUuid;

        IF newUuid IS NULL THEN
            ROLLBACK;
        END IF;
        COMMIT;
    END;
$$ language plpgsql;

CALL "User_Create"(NULL, 'asdf', 'asdf2', 'asdf3', 'asdf4', 'asdf@asdf.com')sw;