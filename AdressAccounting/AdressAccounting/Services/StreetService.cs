using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Services
{
    public class StreetService
    {
        AdressAccountingContext db;
        public StreetService(AdressAccountingContext context)
        {
            db = context;
        }

        public void AddStreet(Street street)
        {
            db.Streets.Add(street);
            db.SaveChanges();
        }

        public List<Street> SearchByName(string name)
        {
            return db.Streets.Where(s => s.Name.ToLower().Contains(name.ToLower())).ToList();
        }

        public List<Street> FilterByHasNameHistory()
        {
            return db.Streets.Where(s => s.StreetNameRecordsStreets.Any()).ToList();
            /*
            SELECT s.*
            FROM Streets s
            JOIN StreetNameRecordsStreets r
            ON s.Id = r.StreetId
             */
        }

        public List<Street> FilterByHasMergeHistory()
        {
            return db.Streets.Where(s => s.MergeRecords.Any()).ToList();
            /*
            SELECT s.*
            FROM Streets s
            JOIN MergeRecords m
            ON s.Id = m.OldStreetId OR s.Id = m.NewStreetId
             */
        }

        public List<Street> FilterByHasSplitHistory()
        {
            return db.Streets.Where(s => s.SplitResults.Any()).ToList();
            /*
            SELECT s.*
            FROM Streets s
            LEFT JOIN SplitRecords r
            ON s.Id = r.OldStreetId
            LEFT JOIN SplitResults res
            ON s.Id = res.NewStreetId
            WHERE r.Id IS NOT NULL OR res.Id IS NOT NULL
             */
        }

        public List<Street> GetAllStreets()
        {
            return db.Streets.ToList();
        }

        public List<Street> SortByName()
        {
            return db.Streets.OrderBy(s => s.Name).ToList();
        }

        public Street GetParentStreetFromSplit(Street street)
        {
            /*string id = street.Id.ToString();
            string sql = @"
                SELECT s.id 
                FROM Streets s
                WHERE s.id =
                (SELECT sr.StreetIdSplittedStreet
                FROM StreetsRecord sr, SplitResults r
                WHERE sr.Id = (
                    SELECT  sres.SplitRecordsId
                    FROM SplitResults sres
                    WHERE Street.id = " + id+"))";
            return db.Streets.FromSqlRaw(sql).ToList().FirstOrDefault();*/
            var sql = @"
                   SELECT s.*
                   FROM Streets s
                   JOIN SplitRecord sr ON s.id = sr.streetIdSplittedStreet
                   JOIN SplitResults r ON r.splitRecordsId = sr.id
                   WHERE r.streetId = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id))
                    .FirstOrDefault();
        }

        public List<Street> GetChildStreetsFromSplit(Street street)
        {
            //TODO: Rename fields and relations in db
            var sql = @"
                   SELECT s.*
                   FROM Street s
                   JOIN SplitRecords r ON sr.splitRecordsId = r.id
                   JOIN SplitResults sr ON s.id = sr.streetId
                   WHERE r.streetIdSplittedStreet = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id))
                    .ToList();
        }

        public List<Street> GetParentStreetsFromMerge(Street street)
        {
            //TODO: Rename fields and relations in db and add double "" for table names and fields
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""MergedStreets"" ms ON s.Id = ms.""streetId""
                   JOIN ""MergeRecords"" mr ON mr.id = ms.""mergeRecordsId""
                   WHERE mr.""streetId(result of merging)"" = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id))
                    .ToList();
        }

        public Street GetChildStreetFromMerge(Street street)
        {
            var sql = @"
                   SELECT s.*
                   FROM Street s
                   JOIN MergeRecords m ON s.id = m.streetIdResultOfMerging
                   JOIN MergedStreets ms ON ms.mergeRecordsId = m.id
                   WHERE ms.streetId = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id))
                    .FirstOrDefault();
        }



        public List<Street> GetStreetsNameChangedAfterDate(DateOnly date)
        {
            var sql = @"
                   SELECT s.*
                   FROM Street s
                   JOIN StreetNameRecordsStreets snrs ON s.id = snrs.streetId
                   JOIN StreetNameRecords snr ON snr.id = snrs.streetNameRecordsId
                   WHERE snr.dateFrom >= @date";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("date", date))
                    .ToList();
        }

        public List<Street> GetStreetsMergedAfterDate(DateOnly date)
        {
            var sql = @"
                   SELECT s.*
                   FROM Street s
                   JOIN MergedStreets ms ON s.id = ms.streetId
                   JOIN MergeRecords mr ON mr.id = ms.mergeRecordsId
                   WHERE mr.date >= @date";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("date", date))
                    .ToList();
        }

        public List<Street> GetStreetsSplitAfterDate(DateOnly date)
        {
            var sql = @"
                   SELECT s.*
                   FROM Street s
                   JOIN SplitRecord sr ON s.id = sr.streetIdSplittedStreet
                   JOIN SplitResults r ON r.splitRecordsId = sr.id
                   WHERE sr.date >= @date";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("date", date))
                    .ToList();
        }

        public List<Street> GetStreetsNameChangedInPeriod(DateOnly startDate, DateOnly endDate)
        {
            var sql = @"
                   SELECT s.*
                   FROM Street s
                   JOIN StreetNameRecordsStreets snrs ON s.id = snrs.streetId
                   JOIN StreetNameRecords snr ON snr.id = snrs.streetNameRecordsId
                   WHERE snr.dateFrom >= @startDate AND snr.dateFrom <= @endDate";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("startDate", startDate), new Npgsql.NpgsqlParameter("endDate", endDate))
                    .ToList();
        }
    }
}
