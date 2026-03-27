create table staging.dim_contact
(
    s_id               varchar(256),
    channel_id         varchar(256),
    instance_key       varchar(256),
    company_name       varchar(256),
    first_name         varchar(256),
    last_name          varchar(256),
    address_line_1     varchar(256),
    address_line_2     varchar(256),
    city               varchar(256),
    state              varchar(256),
    zip                varchar(256),
    country            varchar(256),
    salutation         varchar(256),
    email              varchar(256),
    phone              varchar(256),
    contact_id         varchar(256),
    company_id         varchar(256),
    owned_by_rep_id    varchar(256),
    is_dead            varchar(256),
    gender             varchar(256),
    lead_source        varchar(256),
    lead_status        varchar(256),
    contact_type       varchar(256),
    contact_xref       varchar(256),
    hubspot_contact_id varchar(256),
    job_title          varchar(256),
    department         varchar(256),
    latitude           varchar(256),
    longitude          varchar(256),
    created_date       varchar(256),
    instance_link      varchar(256),
    last_update        varchar(256)
);

alter table staging.dim_contact
    owner to postgres;