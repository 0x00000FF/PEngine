DROP TYPE IF EXISTS "EntryPathItem" CASCADE;

CREATE TYPE "EntryPathItem" AS (
    "Id" UUID, "Type" INTEGER, "Perm" INTEGER,
    "Name" VARCHAR(300), "Created" TIMESTAMP, "Modified" TIMESTAMP,
    "CreatorId" UUID, "CreatorName" VARCHAR(32), "OwnerId" UUID, "OwnerName" VARCHAR(32),
    "Hidden" BOOLEAN, "IpAddress" varchar(32)
    );

DROP FUNCTION IF EXISTS "EntryPath_Lookup"(p_path TEXT);

CREATE OR REPLACE FUNCTION "EntryPath_Lookup"(
        p_path TEXT
) RETURNS SETOF "EntryPathItem" AS $$
DECLARE
    target_item  uuid;
BEGIN
    target_item := (SELECT "Id"
                FROM "EntryPath_StrToChain"(p_path)
                ORDER BY "Index" DESC LIMIT 1);

    IF target_item IS NULL THEN
        RETURN;
    END IF;

    RETURN QUERY SELECT
        et."Id",
            cast((cast(et."Mode" as bit(16)) >> 9) as INTEGER) As "Type",
            cast((cast(et."Mode" as bit(16)) & x'01FF') as INTEGER) As "Perm",
            et."Name", et."Created", et."Modified",
            ut."Id" As "CreatorId", ut."DisplayName" As "CreatorName",
            ut2."Id" As "OwnerId", ut2."DisplayName" As "OwnerName",
            et."Hidden", et."IpAddress"
        FROM "Entries" et
        LEFT JOIN "Users" ut ON et."Creator" = ut."Id"
        LEFT JOIN "Users" ut2 ON et."Owner" = ut2."Id"
        WHERE
            et."Parent" = target_item;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM "EntryPath_Lookup"('/usr');