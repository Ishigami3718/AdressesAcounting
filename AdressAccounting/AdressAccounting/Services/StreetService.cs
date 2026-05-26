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

        private void AddStreet(Street street)
        {
            db.Streets.Add(street);
            db.SaveChanges();
        }

        public void CreateStreet(string name)
        {
            AddStreet(new Street { Name = name});
        }

        public IQueryable<Street> GetStreetByfilters(string name, bool isActual, 
            bool hasSplitParent, bool hasMergeParents, bool hasHistory, DateOnly? dateFrom, DateOnly? dateTo)
        {
            var query = GetAllStreets();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.Name.ToLower().Contains(name.ToLower()));
            }
            if (isActual)
            {
                query = query.Where(s => !s.StreetNameRecordsStreets.Any() || s.StreetNameRecordsStreets.All(r => r.StreetNameRecords.DateTo >= DateOnly.FromDateTime(DateTime.Now)));
            }
            if (hasSplitParent)
            {
                query = query.Where(s => s.SplitResults.Any());
            }
            if (hasMergeParents)
            {
                query = query.Where(s => s.MergeRecords.Any());
            }
            if (hasHistory)
            {
                query = query.Where(s => s.StreetNameRecordsStreets.Any());
            }
            if(dateFrom.HasValue)
            {

            }
            if(dateTo.HasValue)
            {

            }
            return query;
        }
        public IQueryable<Street> SearchByName(string name)
        {
            return db.Streets.Where(s => s.Name.ToLower().Contains(name.ToLower()));
        }

        public IQueryable<Street> FilterByHasNameHistory()
        {
            return db.Streets.Where(s => s.StreetNameRecordsStreets.Any());
            /*
            SELECT s.*
            FROM Streets s
            JOIN StreetNameRecordsStreets r
            ON s.Id = r.StreetId
             */
        }

        public IQueryable<Street> FilterByHasMergeHistory()
        {
            return db.Streets.Where(s => s.MergeRecords.Any());
            /*
            SELECT s.*
            FROM Streets s
            JOIN MergeRecords m
            ON s.Id = m.OldStreetId OR s.Id = m.NewStreetId
             */
        }

        public IQueryable<Street> FilterByHasSplitHistory()
        {
            return db.Streets.Where(s => s.SplitResults.Any());
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

        public IQueryable<Street> GetAllStreets()
        {
            return db.Streets;
        }

        public IQueryable<Street> SortByName()
        {
            return db.Streets.OrderBy(s => s.Name);
        }

        public IQueryable<Street> GetParentStreets(Street street)
        {
            return GetParentStreetFromSplit(street).Union(GetParentStreetsFromMerge(street));
        }

        public IQueryable<Street> GetChildStreets(Street street)
        {
            return GetChildStreetsFromSplit(street).Union(GetChildStreetFromMerge(street));
        }

        public IQueryable<Street> GetParentStreetFromSplit(Street street)
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
                   FROM ""Street"" s
                   JOIN ""SplitRecords"" sr ON s.id = sr.""streetId(splitted street)""
                   JOIN ""SplitResults"" r ON r.""splitRecordsId"" = sr.id
                   WHERE r.""streetId"" = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id));
        }

        public IQueryable<Street> GetChildStreetsFromSplit(Street street)
        {
            //TODO: Rename fields and relations in db
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""SplitResults"" sr ON s.id = sr.""streetId""
                   JOIN ""SplitRecords"" r ON sr.""splitRecordsId"" = r.id                   
                   WHERE r.""streetId(splitted street)"" = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id));
        }

        public IQueryable<Street> GetParentStreetsFromMerge(Street street)
        {
            //TODO: Rename fields and relations in db and add double "" for table names and fields
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""MergedStreets"" ms ON s.Id = ms.""streetId""
                   JOIN ""MergeRecords"" mr ON mr.id = ms.""mergeRecordsId""
                   WHERE mr.""streetId(result of merging)"" = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id));
        }

        public IQueryable<Street> GetChildStreetFromMerge(Street street)
        {
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""MergeRecords"" m ON s.id = m.""streetId(result of merging)""
                   JOIN ""MergedStreets"" ms ON ms.""mergeRecordsId"" = m.id
                   WHERE ms.""streetId"" = @streetId";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("streetId", street.Id));
        }



        public IQueryable<Street> GetStreetsNameChangedAfterDate(DateOnly date)
        {
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""StreetNameRecordsStreets"" snrs ON s.id = snrs.""streetId""
                   JOIN ""StreetNameRecords"" snr ON snr.id = snrs.""streetNameRecordsId""
                   WHERE snr.""dateFrom"" >= @date";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("date", date));
        }

        public IQueryable<Street> GetStreetsMergedAfterDate(DateOnly date)
        {
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""MergedStreets"" ms ON s.id = ms.""streetId""
                   JOIN ""MergeRecords"" mr ON mr.id = ms.""mergeRecordsId""
                   WHERE mr.""date"" >= @date";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("date", date));
        }

        public IQueryable<Street> GetStreetsSplitAfterDate(DateOnly date)
        {
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""SplitRecord"" sr ON s.id = sr.""streetIdSplittedStreet""
                   JOIN ""SplitResults"" r ON r.""splitRecordsId"" = sr.id
                   WHERE sr.""date"" >= @date";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("date", date));
        }

        public IQueryable<Street> GetStreetsNameChangedInPeriod(DateOnly startDate, DateOnly endDate)
        {
            var sql = @"
                   SELECT s.*
                   FROM ""Street"" s
                   JOIN ""StreetNameRecordsStreets"" snrs ON s.id = snrs.""streetId""
                   JOIN ""StreetNameRecords"" snr ON snr.id = snrs.""streetNameRecordsId""
                   WHERE snr.""dateFrom"" between @startDate AND @endDate";
            return db.Streets
                    .FromSqlRaw(sql, new Npgsql.NpgsqlParameter("startDate", startDate),
                    new Npgsql.NpgsqlParameter("endDate", endDate));
        }

        public void UpdateStreetName(Street street, string newName, DateOnly date)
        {
            //TODO: Спитати у Іллі Миколайовича, чи потрібно додавати запис про зміну назви в історію,
            //якщо назва не змінилася і чи треба дата створення вулиці у філдах вулиці
            //спитати про 28 запитів з методички

            //TODO: make checking if already exists record 
            string name = street.Name;  
            street.Name = newName;
            StreetNameRecord record = new StreetNameRecord()
            {
                DateFrom = date,
                DateTo = DateOnly.FromDateTime(DateTime.Now),
                Name = name
            };
            db.StreetNameRecords.Add(record);
            StreetNameRecordsStreet snrs = new StreetNameRecordsStreet()
            {
                Street = street,
                StreetNameRecords = record
            };
            db.StreetNameRecordsStreets.Add(snrs);
            db.SaveChanges();
        }
    }
}
