using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Services
{
    public class MergeService
    {
        AdressAccountingContext db;

        public MergeService(AdressAccountingContext context)
        {
            db = context;
        }


        public void MergeStreets(List<Street> oldStreets, Street newStreet, DateOnly date)
        {
            //TODO: add bool isActive updating when it will be added to the model
            var mergeRecord = new MergeRecord
            {
                StreetIdResultOfMergingNavigation = newStreet,
                Date = date,
                MergedStreets = oldStreets.Select(old => new MergedStreet
                {
                    StreetId = old.Id
                }).ToList()
            };
            db.MergeRecords.Add(mergeRecord);
            db.SaveChanges();
        }

    }
}
