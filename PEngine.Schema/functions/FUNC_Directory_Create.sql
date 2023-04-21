DROP FUNCTION IF EXISTS "Directory_Create"(ParentId UUID, uCreator UUID, newName VARCHAR(300), newMode INTEGER, mHidden BOOLEAN, mSealed BOOLEAN, sIpAddress VARCHAR(32));
CREATE OR REPLACE FUNCTION "Directory_Create" (
    "ParentId"    UUID,
    "uCreator"    UUID,
    "newName"     VARCHAR(300),
    "newMode"     INTEGER DEFAULT 420,
    "mHidden"     BOOLEAN DEFAULT FALSE,
    "mSealed"     BOOLEAN DEFAULT FALSE,
    "sIpAddress"  VARCHAR(32) DEFAULT NULL
) RETURNS TEXT AS $$
    DECLARE
        parent_check RECORD;
        result       UUID;
    BEGIN
        IF "newMode" > 511 OR "newMode" < 0 THEN
            RETURN 'EBADPERM';
        END IF;

        "newMode" := "newMode" + 8192;

        SELECT e."Id", e."Name", e."Type" INTO parent_check
        FROM "EntryLookupView" e
        WHERE e."Id" = "ParentId";

        IF parent_check IS NULL THEN
            RETURN 'ENOENT';
        ELSIF parent_check."Type" <> 16 THEN -- 16 => Directory
            RETURN 'ENODIR';
        END IF;

        INSERT INTO "Entries" (
            "Parent", "Name", "Mode", "Creator", "Owner", "Hidden", "Sealed", "IpAddress"
        ) VALUES (
            "ParentId", "newName", "newMode", "uCreator", "uCreator", "mHidden", "mSealed", "sIpAddress"
        ) ON CONFLICT DO NOTHING
          RETURNING "Id" INTO result;

        IF result IS NULL THEN
            RETURN 'EDUPLENT';
        END IF;

        RETURN result::TEXT;
    END;
$$ LANGUAGE plpgsql;