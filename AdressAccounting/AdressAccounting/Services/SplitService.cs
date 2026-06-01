using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Services
{
    public class SplitService
    {
        AdressAccountingContext _db;

        public SplitService(AdressAccountingContext context)
        {
            _db = context;
        }

        public void SplitStreet(Street oldStreet, List<Street> newStreets, DateOnly date)
        {
            var splitRecord = new SplitRecord
            {
                StreetIdSplittedStreet = oldStreet.Id,
                Date = date
            };
            var splitResults = new List<SplitResult>();
            foreach(var newStreet in newStreets)
            {
                splitRecord.SplitResults.Add(new SplitResult
                {
                    Street = newStreet
                });
                //TODO додати записи історії для утворених вулиць із старої вулиці
                _db.Streets.Add(newStreet);
            }
            _db.SplitRecords.Add(splitRecord);
            _db.SaveChanges();

        }
    }
}
