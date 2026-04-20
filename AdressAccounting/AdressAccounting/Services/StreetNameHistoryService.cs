using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Services
{
    public class StreetNameHistoryService
    {
        AdressAccountingContext db;

        public StreetNameHistoryService(AdressAccountingContext context)
        {
            db = context;
        }
        public List<StreetNameRecord> GetNameHistory(Street street)
        {
            var sql = @"
                   SELECT s.*
                   FROM StreetNameRecords s
                   JOIN StreetNameRecordsStreet snrs ON s.Id = snrs.streetNameRecordsId
                   WHERE snrs.streetId = @streetId";
            return db.StreetNameRecords
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id))
                    .ToList();
        }
    }
}
