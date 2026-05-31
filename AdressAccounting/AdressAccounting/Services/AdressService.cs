using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdressAccounting.Services
{
    public class AdressService
    {
        AdressAccountingContext _db;

        public AdressService(AdressAccountingContext context)
        {
            _db = context;
        }

        public IQueryable<Adress> GetAdressByNumber(int number)
        {
            return _db.Adresses.Where(a => a.Number == number);
        }

        public IQueryable<Adress> GetAllAdresses()
        {
            return _db.Adresses;
        }

        public IQueryable<Adress> GetActualAdresses()
        {
            return _db.Adresses.Where(a => a.IsActual == true);
        }

        public IQueryable<AdressRecord> GetAdressHistory(Adress adress)
        {
            return _db.AdressRecords.Where(ar => ar.AdressId == adress.Id);
        }

        public IQueryable<Adress> GetAdressesByStreet(Street street)
        {
            return _db.Adresses.Where(a => a.StreetId == street.Id);
        }

        public IQueryable<Adress> GetAdressesWithHistory()
        {
            return _db.Adresses.Where(a => a.AdressRecords.Any());
            /*
             SELECT a.*
             FROM Adresses a
             JOIN AdressRecords r
             ON a.Id = r.AdressId
              */
        }

        public IQueryable<Adress> GetFilteredAdresses(bool isActual, bool hasHistory, string numberFilter,
            Street selectedStreet, DateOnly? historyFrom, DateOnly? historyTo)
        {
            IQueryable<Adress> query = _db.Adresses.Include(a => a.Street);

            if (isActual)
            {
                query = query.Where(a => a.IsActual == true);
            }

            if (hasHistory)
            {
                query = query.Where(a => a.AdressRecords.Count() > 1);
            }

            if (!string.IsNullOrEmpty(numberFilter) && int.TryParse(numberFilter, out int number))
            {
                query = query.Where(a => a.Number == number);
            }

            if (selectedStreet != null && selectedStreet.Id != 0)
            {
                query = query.Where(a => a.StreetId == selectedStreet.Id);
            }

            if (historyFrom != null)
            {
                query = query.Where(a => a.AdressRecords.Any(r => r.DateFrom >= historyFrom.Value));
            }

            if (historyTo != null)
            {
                query = query.Where(a => a.AdressRecords.Any(r => r.DateTo <= historyTo.Value));
            }

            return query;
        }

        public async Task<bool> BeUniqueNumberOnStreet(Adress address, int? number, CancellationToken cancellationToken)
        {
            bool exists = await _db.Adresses
                .AnyAsync(a => a.StreetId == address.StreetId
                            && a.Number == number
                            && a.Id != address.Id,
                          cancellationToken);
            return !exists;
        }
        public void CreateAdress(Adress adress)
        {
            _db.Adresses.Add(adress);
            _db.SaveChanges();
        }

        public void UpdateAdress(Adress adress, int newNumber)
        {

            var existingAdress = _db.Adresses.Find(adress.Id);
            if (existingAdress == null) throw new Exception("Adress not found");
            var adressRecord = _db.AdressRecords.Where(ar => ar.AdressId == adress.Id)
                .OrderByDescending(ar => ar.Id)
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

        public void RedactAdress(Adress adress)
        {
            var existingAdress = _db.Adresses.Find(adress.Id);
            if (existingAdress == null) throw new Exception("Adress not found");
            existingAdress.Number = adress.Number;
            existingAdress.Street = adress.Street;
            existingAdress.IsActual = adress.IsActual;
            _db.SaveChanges();
        }

        public void DeleteAdress(Adress adress)
        {
            var existingAdress = _db.Adresses.Find(adress.Id);
            if (existingAdress == null) throw new Exception("Adress not found");
            _db.Adresses.Remove(existingAdress);
            _db.SaveChanges();
        }

    }
}
