using CourierApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CourierApplication.DAL
{
    public class AdressService : IService<Adress>
    {
        private CourierDbContext db;
        public AdressService()
        {
            db = new CourierDbContext();
        }
        public AdressService(CourierDbContext db)
        {
            this.db = db;
        }

        public void AddData(Adress model)
        {
            db.Add(model);
            db.SaveChanges();
        }

        public void DeleteData(Adress model)
        {
            Adress adress = db.Adresses.Where(a => a.AdressId == model.AdressId).Single();
            db.Remove(adress);
            db.SaveChanges();
        }

        public List<Adress> LoadData()
        {
            return db.Adresses.ToList();
        }
        public void UpdateData(Adress model)
        {
            Adress adress = db.Adresses.Where(a => a.AdressId == model.AdressId).Single();
            adress.Name = model.Name;
            adress.Latitude = model.Latitude;
            adress.Longitude = model.Longitude;
            db.SaveChanges(); 
        }
    }
}
