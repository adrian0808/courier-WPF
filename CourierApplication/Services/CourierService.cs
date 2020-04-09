using CourierApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CourierApplication.DAL
{
    public class CourierService : IService<Courier>
    {
        private CourierDbContext db;
        public CourierService()
        {
            db = new CourierDbContext();
        }

        public CourierService(CourierDbContext db)
        {
            this.db = db;
        }

        public void AddData(Courier model)
        {
            db.Add(model);
            db.SaveChanges();
        }

        public void DeleteData(Courier model)
        {
            Courier courier = db.Couriers.Where(c => c.CourierId == model.CourierId).SingleOrDefault();
            db.Remove(courier);
            db.SaveChanges();
        }

        public List<Courier> LoadData()
        {
            return db.Couriers.ToList();
        }

        public void UpdateData(Courier model)
        {          
            Courier courier = db.Couriers.Where(c => c.CourierId == model.CourierId).Single();
            courier.FirstName = model.FirstName;
            courier.LastName = model.LastName;
            courier.isFree = model.isFree;
            db.SaveChanges();           
       }
    }
}
