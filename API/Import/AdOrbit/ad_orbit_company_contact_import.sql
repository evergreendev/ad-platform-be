BEGIN;

CREATE TEMP TABLE company_source_rows ON COMMIT DROP AS
SELECT company_id, contact_id, (contact_source.contact_id = company_source.primary_contact_id) as is_primary
FROM staging.dim_company_prepped company_source
         LEFT JOIN staging.dim_contact_prepped contact_source
                   USING (company_id);

CREATE TEMP TABLE company_resolved ON COMMIT DROP AS
SELECT
    cp.company_id AS source_company_id,
    COALESCE(m.internal_id, gen_random_uuid()) AS internal_company_id,
    m.internal_id IS NOT NULL AS mapping_exists,
    cp.company_name,
    cp.address,
    cp.address2,
    cp.city,
    cp.state,
    cp.zip,
    cp.country,
    cp.website_url,
    cp.company_type as "type",
    cp.tax_id,
    cp.last_update,
    cp.collections,
    cp.write_off,
    cp.primary_rep_name,
    cp.primary_category as "legacy_primary_category",
    cp.hubspot_company_id,
    cp.latitude,
    cp.longitude,
    cp.created_date,
    NOT COALESCE(cp.dead, false) as "is_active",
    cp.is_new_company,
    cp.company_special_billing,
    cp.billing_contact_id,
    cp.print_artwork_contact_id,
    cp.digital_artwork_contact_id
FROM staging.dim_company_prepped cp
         LEFT JOIN public.external_id_map m
                   ON m."source_system" = 'adorbit_dw'
                       AND m."source_table" = 'Dim Company'
                       AND m."source_id" = cp.company_id::text
                       AND m."entity_type" = 'company';


CREATE TEMP TABLE contact_resolved ON COMMIT DROP AS
SELECT
    ctp.contact_id AS source_contact_id,
    m.internal_id IS NOT NULL AS mapping_exists,
    COALESCE(m.internal_id, gen_random_uuid()) AS internal_contact_id,
    ctp.first_name,
    ctp.last_name,
    ctp.address_line_1,
    ctp.address_line_2,
    ctp.city,
    ctp.state,
    ctp.zip,
    ctp.country,
    ctp.salutation,
    ctp.owned_by_rep_id as "user_rep_id",
    NOT COALESCE(ctp.is_dead, false) as "is_active",
    ctp.email,
    ctp.phone,
    gender,
    lead_source,
    lead_status,
    hubspot_contact_id,
    job_title,
    department
FROM staging.dim_contact_prepped ctp
         LEFT JOIN public.external_id_map m
                   ON m.source_system = 'adorbit_dw'
                       AND m.source_table = 'Dim Contact'
                       AND m.source_id = ctp.contact_id::text
                       AND m.entity_type = 'contact';

CREATE TEMP TABLE company_contact_resolved ON COMMIT DROP AS
SELECT DISTINCT
    COALESCE(cc.id, gen_random_uuid()) AS id,
    cr.internal_company_id AS company_id,
    ctr.internal_contact_id AS contact_id,
    COALESCE(csr.is_primary, false) AS is_primary,
    (cc.id IS NOT NULL) AS already_exists
FROM company_source_rows csr
         JOIN company_resolved cr
              ON csr.company_id = cr.source_company_id
         JOIN contact_resolved ctr
              ON csr.contact_id = ctr.source_contact_id
         LEFT JOIN public.company_contact cc
                   ON cc.company_id = cr.internal_company_id
                       AND cc.contact_id = ctr.internal_contact_id;


INSERT INTO public.company(id, company_name, address, address2, city, state, zip, country, website_url, type, tax_id, last_update, collections, write_off, primary_rep_name, legacy_primary_category, hubspot_company_id, latitude, longitude, created_date, is_active, is_new_company, company_special_billing)
SELECT internal_company_id, company_name, address, address2, city, state, zip, country, website_url, type, tax_id, last_update, collections, write_off, primary_rep_name, legacy_primary_category, hubspot_company_id, latitude, longitude, created_date, is_active, is_new_company, company_special_billing
FROM company_resolved
WHERE mapping_exists = FALSE;

INSERT INTO external_id_map(id, source_system, source_table, source_id, entity_type, internal_id, created_at)
SELECT gen_random_uuid(), 'adorbit_dw', 'Dim Company', source_company_id, 'company', internal_company_id, now()
FROM company_resolved
WHERE mapping_exists = FALSE
    ON CONFLICT (source_system, source_table, source_id, entity_type) DO NOTHING;


INSERT INTO public.contact(id, first_name, last_name, address_line1, address_line2, city, state, zip, country, salutation, user_rep_id, is_active, gender, lead_source, lead_status, hubspot_id, job_title, department, created_date, last_updated_date)
SELECT internal_contact_id, first_name, last_name, address_line_1, address_line_2, city, state, zip, country, salutation, user_rep_id, is_active, gender, lead_source, lead_status,hubspot_contact_id, job_title, department, now(), now()
FROM contact_resolved
WHERE mapping_exists = FALSE;

INSERT INTO external_id_map(id, source_system, source_table, source_id, entity_type, internal_id, created_at)
SELECT gen_random_uuid(), 'adorbit_dw', 'Dim Contact', source_contact_id, 'contact', internal_contact_id, now()
FROM contact_resolved
WHERE mapping_exists = FALSE
    ON CONFLICT (source_system, source_table, source_id, entity_type) DO NOTHING;


INSERT INTO public.company_contact("id", "company_id", "contact_id", "is_primary")
SELECT id, company_id, contact_id, is_primary
FROM company_contact_resolved
    ON CONFLICT (company_id, contact_id) DO NOTHING;

INSERT INTO public.company_contact_email(id, company_contact_id, email, is_primary, do_not_email)
SELECT gen_random_uuid(), id, email, true, false
FROM contact_resolved cr
         JOIN company_contact_resolved ccr ON ccr.contact_id = cr.internal_contact_id
WHERE cr.email IS NOT NULL
    ON CONFLICT (company_contact_id, email) DO NOTHING;

INSERT INTO public.company_contact_phone(id, company_contact_id, phone, is_primary, do_not_call)
SELECT gen_random_uuid(), id,cr.phone, true, false
FROM contact_resolved cr
         JOIN company_contact_resolved ccr ON ccr.contact_id = cr.internal_contact_id
WHERE cr.phone IS NOT NULL
    ON CONFLICT (company_contact_id, phone) DO NOTHING;

INSERT INTO public.company_contact_role(company_contact_id, role_id)
SELECT id, 1
FROM company_contact_resolved ccr
         JOIN company_resolved cr ON cr.internal_company_id = ccr.company_id
         JOIN contact_resolved ctr ON ctr.internal_contact_id = ccr.contact_id
WHERE cr.billing_contact_id = ctr.source_contact_id
    ON CONFLICT (company_contact_id, role_id) DO NOTHING;

INSERT INTO public.company_contact_role(company_contact_id, role_id)
SELECT ccr.id, 2
FROM company_contact_resolved ccr
         JOIN company_resolved cr ON cr.internal_company_id = ccr.company_id
         JOIN contact_resolved ctr ON ctr.internal_contact_id = ccr.contact_id
WHERE cr.print_artwork_contact_id = ctr.source_contact_id
    ON CONFLICT (company_contact_id, role_id) DO NOTHING;

INSERT INTO public.company_contact_role(company_contact_id, role_id)
SELECT ccr.id, 3
FROM company_contact_resolved ccr
         JOIN company_resolved cr ON cr.internal_company_id = ccr.company_id
         JOIN contact_resolved ctr ON ctr.internal_contact_id = ccr.contact_id
WHERE cr.digital_artwork_contact_id = ctr.source_contact_id
    ON CONFLICT (company_contact_id, role_id) DO NOTHING;

COMMIT;