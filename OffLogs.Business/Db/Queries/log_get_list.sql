SELECT
    logT.*,
    lp.*,
    lt.*,
    (
        SELECT COUNT(id) FROM logs WHERE application_id = 2
    ) AS sumCount
FROM (
         SELECT * FROM logs
         WHERE application_id = 2
         ORDER BY create_time DESC
             LIMIT 1
         OFFSET 2
     ) AS logT
         LEFT JOIN log_properties AS lp ON lp.log_id =  logT.Id
         LEFT JOIN log_traces AS lt ON lt.log_id =  logT.Id;