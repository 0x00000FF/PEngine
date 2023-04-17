DROP VIEW IF EXISTS "EntryLookupView";
CREATE VIEW "EntryLookupView" AS SELECT
            et."Id",
            et."Parent",
            cast((cast(et."Mode" as bit(16)) >> 9) as INTEGER) As "Type",
            cast((cast(et."Mode" as bit(16)) & x'01FF') as INTEGER) As "Perm",
            et."Name", et."Created", et."Modified",
            ut."Id" As "CreatorId", ut."DisplayName" As "CreatorName",
            ut2."Id" As "OwnerId", ut2."DisplayName" As "OwnerName",
            et."Hidden", et."IpAddress"
        FROM "Entries" et
        LEFT JOIN "Users" ut ON et."Creator" = ut."Id"
        LEFT JOIN "Users" ut2 ON et."Owner" = ut2."Id";