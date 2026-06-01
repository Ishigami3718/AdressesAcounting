using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Services
{
    public class MergeService
    {
        AdressAccountingContext _db;

        public MergeService(AdressAccountingContext context)
        {
            _db = context;
        }

        //Після спліта чи мерджа треба просто робити адреми неактуальнимиі юзер вручну оновлює, чи автоматично
        // якось зробити щоб юзер у вьюшці злиття мерджа обирав те як адреси оновляться
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
            _db.MergeRecords.Add(mergeRecord);
            //foreach in oldstreets adresses and update them with method
            _db.SaveChanges();
        }

    }
}
