CREATE TYPE log_property_type AS (
    id bigint,
    log_id bigint,
    key character(200),
    value character(2048),
    create_time timestamp
);

CREATE TYPE log_trace_type AS (
    id bigint,
    log_id bigint,
    trace character(2048),
    create_time timestamp
);