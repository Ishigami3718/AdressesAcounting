using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static AdressAccounting.UI.AdressRenumeringWindow;

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
        public void MergeStreets(List<Street> oldStreets, Street newStreet, DateOnly date, int[] newNumbers = null)
        {
            /*newStreet.IsActual = true;
            _db.Streets.Add(newStreet);
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
            _db.Streets.Add(newStreet);
            //foreach in oldstreets adresses and update them with method
            if(newNumbers != null)
            {
                IEnumerable<Adress> Adresses = oldStreets.SelectMany(s => s.Adresses.Where(a => a.IsActual.Value && a.StreetId == s.Id)).ToList();
                for (int i = 0; i < newNumbers.Length; i++)
                {
                    Adresses.ElementAt(i).Number = newNumbers[i];
                }
            }
            _db.SaveChanges();*/
            newStreet.IsActual = true;
            _db.Streets.Add(newStreet);
            _db.SaveChanges();

            var oldStreetIds = oldStreets.Select(s => s.Id).ToList();

            var manyToManyEntries = _db.StreetNameRecordsStreets
                .Where(s => oldStreetIds.Contains(s.StreetId.Value)).ToList();

            foreach (var oldLink in manyToManyEntries)
            {
                var newHistoryLink = new StreetNameRecordsStreet
                {
                    StreetId = newStreet.Id,               
                    StreetNameRecordsId = oldLink.StreetNameRecordsId 
                };
                _db.StreetNameRecordsStreets.Add(newHistoryLink);
            }

            var currentNameRecord = new StreetNameRecord
            {
                Name = newStreet.Name,
                DateFrom = date, 
                DateTo = null
            };
            _db.StreetNameRecords.Add(currentNameRecord);
            _db.SaveChanges();

            var dbOldStreets = _db.Streets
                .Include(s => s.Adresses)
                .Where(s => oldStreetIds.Contains(s.Id))
                .ToList();

            var mergeRecord = new MergeRecord
            {
                StreetIdResultOfMerging = newStreet.Id, 
                Date = date,
                MergedStreets = dbOldStreets.Select(old => new MergedStreet
                {
                    StreetId = old.Id
                }).ToList()
            };
            _db.MergeRecords.Add(mergeRecord);

            var allAdresses = dbOldStreets.SelectMany(s => s.Adresses.Where(a => a.IsActual == true)).ToList();

            for (int i = 0; i < allAdresses.Count; i++)
            {
                var adress = allAdresses[i];

                var lastRecord = _db.AdressRecords
                    .Where(ar => ar.AdressId == adress.Id)
                    .OrderBy(ar => ar.Id)
                    .LastOrDefault();

                if (lastRecord != null)
                {
                    lastRecord.DateTo = date;
                }

                int oldNumber = adress.Number ?? 0;
                int updatedNumber = (newNumbers != null && i < newNumbers.Length) ? newNumbers[i] : oldNumber;

                adress.StreetId = newStreet.Id;
                adress.Number = updatedNumber;

                var newRecord = new AdressRecord
                {
                    AdressId = adress.Id,
                    Number = updatedNumber,
                    StreetName = newStreet.Name, 
                    AreaId = adress.AreaId,
                    DateFrom = date,
                    DateTo = null
                };
                _db.AdressRecords.Add(newRecord);
            }
            foreach (var oldStreet in dbOldStreets)
            {
                oldStreet.IsActual = false;
            }

            _db.SaveChanges();
        }

    }
}
