DROP TYPE IF EXISTS "EntryPathChainNode" CASCADE;

CREATE TYPE "EntryPathChainNode" AS (
    "Id" UUID, "Parent" UUID, "Name" VARCHAR(300) );

DROP FUNCTION IF EXISTS "EntryPath_StrToChain"(p_path TEXT);

CREATE OR REPLACE FUNCTION "EntryPath_StrToChain"(p_path TEXT)
    RETURNS SETOF "EntryPathChainNode" AS $$
DECLARE
    path_elements  TEXT[];
    current_entry  RECORD;
    current_path   VARCHAR(300);
    current_parent UUID;
BEGIN
    CREATE TEMPORARY TABLE result (
        "Id" UUID, "Parent" UUID, "Name" VARCHAR(300) )
    ON COMMIT DROP;

    path_elements := string_to_array(rtrim(ltrim(p_path, '/'), '/'), '/');
    current_parent := NULL;

    RAISE NOTICE '%', path_elements;

    FOREACH current_path IN ARRAY path_elements
    LOOP
        IF current_parent IS NULL THEN
            SELECT "Id", "Parent", "Name"
                INTO current_entry
                FROM "Entries"
                WHERE "Name" = current_path;
        ELSE
            SELECT "Id", "Parent", "Name"
                INTO current_entry
                FROM "Entries"
                WHERE "Parent" = current_parent AND "Name" = current_path;
        END IF;

        RAISE NOTICE '%,%,%:%', current_path, current_entry, current_parent, current_entry IS NULL;

        IF current_entry IS NULL THEN
            RETURN;
        END IF;

        current_parent := current_entry."Id";
        INSERT INTO result (
            SELECT current_entry."Id", current_entry."Parent", current_entry."Name");
    END LOOP;

    RETURN QUERY SELECT * FROM result;
END;
$$ LANGUAGE plpgsql;