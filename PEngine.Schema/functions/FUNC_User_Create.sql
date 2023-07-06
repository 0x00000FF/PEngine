DROP FUNCTION "User_Create"(assignedGroup UUID, newUsername VARCHAR(32), newPasswordHash VARCHAR(256), newPasswordSalt VARCHAR(256), newDisplayName VARCHAR(32), newEmail VARCHAR(128))
CREATE OR REPLACE FUNCTION "User_Create" (
    assignedGroup UUID,
    newUsername   VARCHAR(32),
    newPasswordHash VARCHAR(256),
    newPasswordSalt VARCHAR(256),
    newDisplayName VARCHAR(32),
    newEmail VARCHAR(128)
) RETURNS uuid AS $$
    DECLARE
        newUuid UUID;
        newGroup UUID;
        newKeyring UUID;
    BEGIN
        SAVEPOINT UserCreateSavepoint;

        newUuid := gen_random_uuid();
        newGroup := CASE WHEN assignedGroup IS NULL THEN newUuid ELSE newGroup END;

        BEGIN
            INSERT INTO "Keyrings"("Salt", "Algorithm", "Key")
                   VALUES (newPasswordSalt, 'SHA512', newPasswordHash) RETURNING "Id"
                   INTO newKeyring;

            IF newKeyring IS NULL THEN
                RETURN NULL;
            END IF;

            INSERT INTO "Users" ("Id", "Group", "Username", "DisplayName", "Email", "Keyring") VALUES
                (newUuid, newGroup, newUsername, newDisplayName, newEmail, newKeyring)
                ON CONFLICT DO NOTHING RETURNING "Id" INTO newUuid;

            IF newUuid IS NULL THEN
                ROLLBACK;
            END IF;
        COMMIT;

        RETURN newUuid;
    END;
$$ LANGUAGE plpgsql;

SELECT