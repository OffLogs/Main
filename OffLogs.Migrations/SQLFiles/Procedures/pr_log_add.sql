﻿DROP PROCEDURE IF EXISTS pr_log_add;

CREATE PROCEDURE pr_log_add(
    prop_application_id bigint,
    prop_message varchar(2048),
    prop_level varchar(20),
    prop_log_time timestamp,
    prop_properties log_property_type,
    prop_traces log_trace_type
) AS $$
DECLARE
    v_log_id BIGINT;
    BEGIN
        INSERT INTO public.logs (
            application_id,
            level,
            message,
            log_time,
            create_time
        ) VALUES (
             prop_application_id,
             prop_level,
             prop_message,
             prop_log_time,
             CURRENT_TIMESTAMP
         );
        v_log_id = lastval();
    
        INSERT INTO public.log_properties (
            log_id,
            key,
            value,
            create_time
        )
        SELECT
            v_log_id,
            t1.Key,
            t1.value,
            t1.create_time
        FROM prop_properties AS t1;
        
        INSERT INTO public.log_traces (
            log_id,
            trace,
            create_time
        )
        SELECT
            v_log_id,
            t1.trace,
            t1.create_time
        FROM prop_traces AS t1;
END;
$$ LANGUAGE plpgsql;  