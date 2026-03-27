# Instructions for importing data from Metabase(Ad Orbit)

Run SQL scripts in the following order

## Importing Contact and Company
Company and contacts should be imported in parallel
1) metabase_dim_contact_create.sql ad_orbit_dim_company_create.sql creates the tables for your metabase export to be imported
2) Import metabase data into dim_contact and dim_company
3) ad_orbit_prepare_company.sql ad_orbit_prepare_contact.sql moves raw data from dim tables to prepared tables
4) ad_orbit_company_contact_import.sql imports data won't duplicate records