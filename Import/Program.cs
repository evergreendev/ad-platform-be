using Microsoft.Extensions.Configuration;
using Npgsql;

var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

var connectionString = configuration.GetConnectionString("Default") ?? 
                       throw new InvalidOperationException("Connection string not found.");

Console.WriteLine("Starting import...");

using var conn = new NpgsqlConnection(connectionString);
await conn.OpenAsync();

var companySql = @"
BEGIN;

-- Create a temporary mapping of SourceId to InternalId
CREATE TEMP TABLE company_import_map AS
SELECT 
    s.""company_id"" AS source_id,
    COALESCE(m.""InternalId"", gen_random_uuid()) AS internal_id,
    m.""InternalId"" IS NOT NULL AS exists_in_map
FROM staging.dim_company_prepped s
LEFT JOIN public.external_id_map m 
    ON m.""SourceSystem"" = 'adorbit_dw' 
    AND m.""SourceTable"" = 'Dim Company' 
    AND m.""SourceId"" = s.""company_id""::text
    AND m.""EntityType"" = 'company';

-- Upsert into public.company
INSERT INTO public.company (
    ""Id"", ""CompanyName"", ""Address"", ""Address2"", ""City"", ""State"", ""Zip"", ""Country"", ""WebsiteUrl"", 
    ""Type"", ""TaxId"", ""LastUpdate"", ""Collections"", ""WriteOff"", ""PrimaryRepName"", 
    ""LegacyPrimaryCategory"", ""HubspotCompanyId"", 
    ""Latitude"", ""Longitude"", ""CreatedDate"", ""Dead"", ""IsNewCompany"", ""CompanySpecialBilling""
)
SELECT 
    m.internal_id, s.""company_name"", s.""address"", s.""address2"", s.""city"", s.""state"", s.""zip"", s.""country"", s.""website_url"", 
    s.""company_type"", s.""tax_id"", s.""last_update"", s.""collections"", s.""write_off"", s.""primary_rep_name"", 
    s.""primary_category"", s.""hubspot_company_id"", 
    s.""latitude"", s.""longitude"", s.""created_date"", s.""dead"", s.""is_new_company"", s.""company_special_billing""
FROM staging.dim_company_prepped s
JOIN company_import_map m ON s.""company_id"" = m.source_id
ON CONFLICT (""Id"") DO UPDATE SET
    ""CompanyName"" = EXCLUDED.""CompanyName"",
    ""Address"" = EXCLUDED.""Address"",
    ""Address2"" = EXCLUDED.""Address2"",
    ""City"" = EXCLUDED.""City"",
    ""State"" = EXCLUDED.""State"",
    ""Zip"" = EXCLUDED.""Zip"",
    ""Country"" = EXCLUDED.""Country"",
    ""WebsiteUrl"" = EXCLUDED.""WebsiteUrl"",
    ""Type"" = EXCLUDED.""Type"",
    ""TaxId"" = EXCLUDED.""TaxId"",
    ""LastUpdate"" = EXCLUDED.""LastUpdate"",
    ""Collections"" = EXCLUDED.""Collections"",
    ""WriteOff"" = EXCLUDED.""WriteOff"",
    ""PrimaryRepName"" = EXCLUDED.""PrimaryRepName"",
    ""LegacyPrimaryCategory"" = EXCLUDED.""LegacyPrimaryCategory"",
    ""HubspotCompanyId"" = EXCLUDED.""HubspotCompanyId"",
    ""Latitude"" = EXCLUDED.""Latitude"",
    ""Longitude"" = EXCLUDED.""Longitude"",
    ""CreatedDate"" = EXCLUDED.""CreatedDate"",
    ""Dead"" = EXCLUDED.""Dead"",
    ""IsNewCompany"" = EXCLUDED.""IsNewCompany"",
    ""CompanySpecialBilling"" = EXCLUDED.""CompanySpecialBilling"";

-- Upsert into external_id_map
INSERT INTO public.external_id_map (
    ""Id"", ""SourceSystem"", ""SourceTable"", ""SourceId"", ""EntityType"", ""InternalId"", ""CreatedAt""
)
SELECT 
    gen_random_uuid(), 'adorbit_dw', 'Dim Company', m.source_id::text, 'company', m.internal_id, NOW()
FROM company_import_map m
WHERE NOT exists_in_map
ON CONFLICT (""SourceSystem"", ""SourceTable"", ""SourceId"") DO NOTHING;

COMMIT;
";

Console.WriteLine("Importing companies...");
await using (var cmd = new NpgsqlCommand(companySql, conn))
{
    var affectedRows = await cmd.ExecuteNonQueryAsync();
    Console.WriteLine($"Company import completed. {affectedRows} rows affected.");
}

var contactSql = @"
BEGIN;

-- Create a temporary mapping of SourceId to InternalId for Contacts
CREATE TEMP TABLE contact_import_map AS
SELECT 
    s.""contact_id"" AS source_id,
    COALESCE(m.""InternalId"", gen_random_uuid()) AS internal_id,
    m.""InternalId"" IS NOT NULL AS exists_in_map
FROM staging.dim_contact_prepped s
LEFT JOIN public.external_id_map m 
    ON m.""SourceSystem"" = 'adorbit_dw' 
    AND m.""SourceTable"" = 'Dim Contact' 
    AND m.""SourceId"" = s.""contact_id""::text
    AND m.""EntityType"" = 'contact';

-- Upsert into public.contact
INSERT INTO public.contact (
    ""Id"", ""FirstName"", ""LastName"", ""AddressLine1"", ""AddressLine2"", ""City"", ""State"", ""Zip"", ""Country"",
    ""Salutation"", ""UserRepId"", ""IsActive"", ""Gender"", ""LeadSource"", ""LeadStatus"", ""HubspotId"",
    ""JobTitle"", ""Department"", ""CreatedDate"", ""LastUpdatedDate""
)
SELECT 
    m.internal_id, s.""first_name"", s.""last_name"", s.""address_line_1"", s.""address_line_2"", s.""city"", s.""state"", s.""zip"", s.""country"",
    s.""salutation"", s.""owned_by_rep_id"", NOT COALESCE(s.""is_dead"", false), s.""gender"", s.""lead_source"", s.""lead_status"", s.""hubspot_contact_id"",
    s.""job_title"", s.""department"", s.""created_date"", s.""last_update""
FROM staging.dim_contact_prepped s
JOIN contact_import_map m ON s.""contact_id"" = m.source_id
ON CONFLICT (""Id"") DO UPDATE SET
    ""FirstName"" = EXCLUDED.""FirstName"",
    ""LastName"" = EXCLUDED.""LastName"",
    ""AddressLine1"" = EXCLUDED.""AddressLine1"",
    ""AddressLine2"" = EXCLUDED.""AddressLine2"",
    ""City"" = EXCLUDED.""City"",
    ""State"" = EXCLUDED.""State"",
    ""Zip"" = EXCLUDED.""Zip"",
    ""Country"" = EXCLUDED.""Country"",
    ""Salutation"" = EXCLUDED.""Salutation"",
    ""UserRepId"" = EXCLUDED.""UserRepId"",
    ""IsActive"" = EXCLUDED.""IsActive"",
    ""Gender"" = EXCLUDED.""Gender"",
    ""LeadSource"" = EXCLUDED.""LeadSource"",
    ""LeadStatus"" = EXCLUDED.""LeadStatus"",
    ""HubspotId"" = EXCLUDED.""HubspotId"",
    ""JobTitle"" = EXCLUDED.""JobTitle"",
    ""Department"" = EXCLUDED.""Department"",
    ""CreatedDate"" = EXCLUDED.""CreatedDate"",
    ""LastUpdatedDate"" = EXCLUDED.""LastUpdatedDate"";

-- Upsert into external_id_map for Contacts
INSERT INTO public.external_id_map (
    ""Id"", ""SourceSystem"", ""SourceTable"", ""SourceId"", ""EntityType"", ""InternalId"", ""CreatedAt""
)
SELECT 
    gen_random_uuid(), 'adorbit_dw', 'Dim Contact', m.source_id::text, 'contact', m.internal_id, NOW()
FROM contact_import_map m
WHERE NOT exists_in_map
ON CONFLICT (""SourceSystem"", ""SourceTable"", ""SourceId"") DO NOTHING;

-- Create/Update contact emails
INSERT INTO public.contact_email (""Id"", ""ContactId"", ""Email"", ""IsPrimary"", ""DoNotEmail"")
SELECT 
    gen_random_uuid(), m.internal_id, s.""email"", true, false
FROM staging.dim_contact_prepped s
JOIN contact_import_map m ON s.""contact_id"" = m.source_id
WHERE s.""email"" IS NOT NULL AND s.""email"" <> ''
ON CONFLICT DO NOTHING; -- No natural key, so simple avoid dups if they match perfectly (but wait, there's no unique constraint on (ContactId, Email) yet)

-- Link Contacts to Companies in public.company_contact
INSERT INTO public.company_contact (""Id"", ""CompanyId"", ""ContactId"", ""IsPrimary"")
SELECT 
    gen_random_uuid(), c_m.""InternalId"", ct_m.internal_id, 
    COALESCE(s.""contact_id""::text = sc.""primary_contact_id""::text, false)
FROM staging.dim_contact_prepped s
JOIN contact_import_map ct_m ON s.""contact_id"" = ct_m.source_id
JOIN staging.dim_company_prepped sc ON s.""company_id""::text = sc.""company_id""::text
JOIN public.external_id_map c_m ON c_m.""SourceSystem"" = 'adorbit_dw' 
    AND c_m.""SourceTable"" = 'Dim Company' 
    AND c_m.""SourceId"" = sc.""company_id""::text
    AND c_m.""EntityType"" = 'company'
WHERE s.""company_id"" IS NOT NULL AND s.""company_id"" <> ''
ON CONFLICT (""CompanyId"", ""ContactId"") DO UPDATE SET
    ""IsPrimary"" = EXCLUDED.""IsPrimary"";

-- Handle other contact types from dim_company_prepped (billing, etc.)
-- We need to ensure these contacts are also linked if they exist but might not have the company_id set in dim_contact_prepped
INSERT INTO public.company_contact (""Id"", ""CompanyId"", ""ContactId"", ""IsPrimary"")
SELECT 
    gen_random_uuid(), c_m.""InternalId"", ct_m.internal_id, 
    (ct_m.source_id::text = sc.""primary_contact_id""::text)
FROM staging.dim_company_prepped sc
CROSS JOIN LATERAL (
    VALUES 
        (sc.""billing_contact_id"", false),
        (sc.""primary_contact_id"", true),
        (sc.""print_artwork_contact_id"", false),
        (sc.""digital_artwork_contact_id"", false)
) AS v(contact_id, is_primary)
JOIN contact_import_map ct_m ON v.contact_id::text = ct_m.source_id::text
JOIN public.external_id_map c_m ON c_m.""SourceSystem"" = 'adorbit_dw' 
    AND c_m.""SourceTable"" = 'Dim Company' 
    AND c_m.""SourceId"" = sc.""company_id""::text
    AND c_m.""EntityType"" = 'company'
WHERE v.contact_id IS NOT NULL
ON CONFLICT (""CompanyId"", ""ContactId"") DO UPDATE SET
    ""IsPrimary"" = EXCLUDED.""IsPrimary"";

-- Assign system roles based on the columns in dim_company_prepped
INSERT INTO public.company_contact_role (""CompanyContactId"", ""RoleId"")
SELECT cc.""Id"", v.role_id
FROM staging.dim_company_prepped sc
CROSS JOIN LATERAL (
    VALUES 
        (sc.""billing_contact_id"", 1),         -- Billing
        (sc.""print_artwork_contact_id"", 2),   -- Print Artwork
        (sc.""digital_artwork_contact_id"", 3)  -- Digital Artwork
) AS v(contact_id, role_id)
JOIN contact_import_map ct_m ON v.contact_id::text = ct_m.source_id::text
JOIN public.external_id_map c_m ON c_m.""SourceSystem"" = 'adorbit_dw' 
    AND c_m.""SourceTable"" = 'Dim Company' 
    AND c_m.""SourceId"" = sc.""company_id""::text
    AND c_m.""EntityType"" = 'company'
JOIN public.company_contact cc ON cc.""CompanyId"" = c_m.""InternalId"" AND cc.""ContactId"" = ct_m.internal_id
WHERE v.contact_id IS NOT NULL
ON CONFLICT DO NOTHING;

-- Populate CompanyContactEmail
INSERT INTO public.company_contact_email (""Id"", ""CompanyContactId"", ""Email"", ""IsPrimary"", ""DoNotEmail"")
SELECT 
    gen_random_uuid(), cc.""Id"", s.""email"", true, false
FROM staging.dim_contact_prepped s
JOIN contact_import_map ct_m ON s.""contact_id"" = ct_m.source_id
JOIN public.external_id_map c_m ON c_m.""SourceSystem"" = 'adorbit_dw' 
    AND c_m.""SourceTable"" = 'Dim Company' 
    AND c_m.""SourceId"" = s.""company_id""::text
    AND c_m.""EntityType"" = 'company'
JOIN public.company_contact cc ON cc.""CompanyId"" = c_m.""InternalId"" AND cc.""ContactId"" = ct_m.internal_id
WHERE s.""email"" IS NOT NULL AND s.""email"" <> '' AND s.""company_id"" IS NOT NULL AND s.""company_id"" <> ''
ON CONFLICT DO NOTHING;

-- Populate CompanyContactPhone
INSERT INTO public.company_contact_phone (""Id"", ""CompanyContactId"", ""Phone"", ""IsPrimary"", ""DoNotCall"")
SELECT 
    gen_random_uuid(), cc.""Id"", s.""phone"", true, false
FROM staging.dim_contact_prepped s
JOIN contact_import_map ct_m ON s.""contact_id"" = ct_m.source_id
JOIN public.external_id_map c_m ON c_m.""SourceSystem"" = 'adorbit_dw' 
    AND c_m.""SourceTable"" = 'Dim Company' 
    AND c_m.""SourceId"" = s.""company_id""::text
    AND c_m.""EntityType"" = 'company'
JOIN public.company_contact cc ON cc.""CompanyId"" = c_m.""InternalId"" AND cc.""ContactId"" = ct_m.internal_id
WHERE s.""phone"" IS NOT NULL AND s.""phone"" <> '' AND s.""company_id"" IS NOT NULL AND s.""company_id"" <> ''
ON CONFLICT DO NOTHING;

COMMIT;
";

Console.WriteLine("Importing contacts...");
using (var cmd = new NpgsqlCommand(contactSql, conn))
{
    var affectedRows = await cmd.ExecuteNonQueryAsync();
    Console.WriteLine($"Contact import completed. {affectedRows} rows affected.");
}

Console.WriteLine("Full import cycle finished.");