DROP FUNCTION IF EXISTS f_log_get_list;

CREATE FUNCTION f_log_get_list(
    prop_application_id bigint,
    prop_page int,
    prop_page_size int = 30
) AS $$
DECLARE
    v_offset int;
    BEGIN

        prop_page = prop_page - 1;
        prop_page = CASE WHEN prop_page <= 0 
                THEN 0 
                ELSE prop_page 
            END;
        DECLARE v_offset bigint = prop_page * prop_page_size;
    
        RETURN QUERY
            SELECT
                logT.*,
                lp.*,
                lt.*,
                (
                    SELECT COUNT(id) FROM logs WHERE application_id = prop_application_id
                ) AS sumCount
                FROM (
                    SELECT * FROM logs
                        WHERE application_id = prop_application_id
                        ORDER BY create_time DESC
                        LIMIT prop_page_size
                        OFFSET v_offset
                ) AS logT
                LEFT JOIN LogProperties AS lp ON lp.LogId =  logT.Id
                LEFT JOIN LogTraces AS lt ON lt.LogId =  logT.Id
    END;
$$ LANGUAGE plpgsql;  