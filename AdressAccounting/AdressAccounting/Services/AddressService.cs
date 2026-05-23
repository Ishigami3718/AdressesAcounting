using System;
using System.Collections.Generic;
using System.Text;

namespace AdressAccounting.Services
{
    public class AddressService
    {
        AdressAccountingContext _db;

        public AddressService(AdressAccountingContext context)
        {
            _db = context;
        }

        public IQueryable<Adress> GetAdressByNumber(int number)
        {
            return _db.Adresses.Where(a => a.Number == number);
        }

        public IQueryable<Adress> GetActualAdresses()
        {
            return _db.Adresses.Where(a => a.IsActual == true);
        }

        public IQueryable<AdressRecord> GetAdressHistory(Adress adress)
        {
            return _db.AdressRecords.Where(ar => ar.AdressId == adress.Id);
        }
        public void CreateAdress(Adress adress)
        {
            _db.Adresses.Add(adress);
            _db.SaveChanges();
        }

        public void UpdateAdress(Adress adress, int newNumber)
        {

            var existingAdress = _db.Adresses.Find(adress.Id);
            if(existingAdress == null) throw new Exception("Adress not found");
            var adressRecord = _db.AdressRecords.Where(ar => ar.AdressId == adress.Id)
                .OrderByDescending(ar =>ar.Id)
                .LastOrDefault();
            if (adressRecord != null)
            {
                adressRecord.DateTo = DateOnly.FromDateTime(DateTime.Now);
            }
            else throw new Exception("Adress record not found");


            int oldNumber = existingAdress.Number ?? 0;
            existingAdress.Number = newNumber;
            AdressRecord newRecord = new AdressRecord
            {
                Number = newNumber,
                DateFrom = DateOnly.FromDateTime(DateTime.Now),
                DateTo = null,
                AreaId = existingAdress.AreaId
            };
            existingAdress.AdressRecords.Add(newRecord);
            _db.SaveChanges();
        }
    }
}
