CREATE TYPE log_property_type AS (
    id bigint,
    log_id bigint,
    key varchar(200),
    value varchar(2048),
    create_time timestamp
);

CREATE TYPE log_trace_type AS (
    id bigint,
    log_id bigint,
    trace varchar(2048),
    create_time timestamp
);