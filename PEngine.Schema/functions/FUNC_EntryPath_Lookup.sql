DROP TYPE IF EXISTS "EntryPathItem" CASCADE;

CREATE TYPE "EntryPathItem" AS (
    "Id" UUID, "Parent" UUID, "Type" INTEGER, "Perm" INTEGER,
    "Name" VARCHAR(300), "Created" TIMESTAMP, "Modified" TIMESTAMP,
    "CreatorId" UUID, "CreatorName" VARCHAR(32), "OwnerId" UUID, "OwnerName" VARCHAR(32),
    "Hidden" BOOLEAN, "IpAddress" varchar(32)
    );

DROP FUNCTION IF EXISTS "EntryPath_Lookup"(p_path TEXT);

CREATE OR REPLACE FUNCTION "EntryPath_Lookup"(
        p_path TEXT
) RETURNS SETOF "EntryPathItem" AS $$
DECLARE
    target_item  RECORD;
BEGIN
    SELECT "Id", "Parent"
           INTO target_item
           FROM "EntryPath_StrToChain"(p_path)
           ORDER BY "Index" DESC LIMIT 1;

    IF target_item IS NULL THEN
        RETURN;
    END IF;

    RETURN QUERY (SELECT * FROM "EntryLookupView" WHERE "Id" = target_item."Id") UNION
                 (SELECT * FROM "EntryLookupView" WHERE "Parent" = target_item."Id")
                 ORDER BY "Parent", "Name";
END;
$$ LANGUAGE plpgsql;

SELECT * FROM "EntryPath_Lookup"('/usr');