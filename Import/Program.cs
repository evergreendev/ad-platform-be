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

var sql = @"
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

using var cmd = new NpgsqlCommand(sql, conn);
var affectedRows = await cmd.ExecuteNonQueryAsync();

Console.WriteLine($"Import completed. {affectedRows} rows affected.");