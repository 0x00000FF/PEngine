CREATE OR REPLACE FUNCTION "Entry_Create" (
    "ParentId"  UUID,
    "uCreator"  UUID,
    "newName"   VARCHAR(300),
    "newMode"   INTEGER,
    "mHidden" BOOLEAN DEFAULT FALSE,
    "mSealed" BOOLEAN DEFAULT FALSE,
    "sIpAddress" VARCHAR(32) DEFAULT NULL
) RETURNS TEXT AS $$
    DECLARE BEGIN

    $$