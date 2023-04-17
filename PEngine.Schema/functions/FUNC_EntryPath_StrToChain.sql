DROP TYPE IF EXISTS "EntryPathChainNode" CASCADE;

CREATE TYPE "EntryPathChainNode" AS (
    "Index" INTEGER, "Id" UUID, "Parent" UUID, "Name" VARCHAR(300));

DROP FUNCTION IF EXISTS "EntryPath_StrToChain"(p_path TEXT);

CREATE OR REPLACE FUNCTION "EntryPath_StrToChain"(p_path TEXT)
    RETURNS SETOF "EntryPathChainNode" AS $$
DECLARE
    path_elements  TEXT[];
    current_entry  RECORD;
    current_path   VARCHAR(300);
    current_parent UUID;
    index          INTEGER;
BEGIN
    index := 0;

    CREATE TEMPORARY TABLE result (
        "Index" INTEGER, "Id" UUID, "Parent" UUID, "Name" VARCHAR(300) )
    ON COMMIT DROP;

    path_elements := string_to_array(ltrim(rtrim(p_path, '/'), '/'), '/');
    current_parent := '00000000-0000-0000-0000-000000000000'::uuid;

    IF path_elements = '{}' THEN
        RETURN QUERY SELECT
                         0::INTEGER           As "Index",
                         current_parent::uuid As "Id",
                         NULL::uuid           As "Parent",
                         ''::VARCHAR(300)     As "Name";
    END IF;

    RAISE NOTICE '%', path_elements;

    FOREACH current_path IN ARRAY path_elements
    LOOP
        SELECT "Id", "Parent", "Name"
            INTO current_entry
            FROM "Entries"
            WHERE "Parent" = current_parent AND "Name" = current_path;

        IF current_entry IS NULL THEN
            RETURN;
        END IF;

        current_parent := current_entry."Id";

        INSERT INTO result (
            SELECT index As "Index", current_entry."Id", current_entry."Parent", current_entry."Name");

        index := index + 1;

    END LOOP;

    RETURN QUERY SELECT * FROM result;
END;
$$ LANGUAGE plpgsql;
