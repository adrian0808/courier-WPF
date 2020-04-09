using CourierApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CourierApplication.DAL
{
    public class OrderService : IService<Order>
    {
        private CourierDbContext db;
        public OrderService()
        {
            db = new CourierDbContext();
        }

        public OrderService(CourierDbContext db)
        {
            this.db = db;
        }

        public void AddData(Order model)
        {
            db.Add(model);
            db.SaveChanges();
        }

        public void DeleteData(Order model)
        {
            Order order = db.Orders.Where(o => o.OrderId == model.OrderId).Single();
            db.Remove(order);
            db.SaveChanges();
        }

        public List<Order> LoadData()
        {
            return db.Orders.ToList();
        }

        public void UpdateData(Order model)
        {
            Order order = db.Orders.Where(o => o.OrderId == model.OrderId).Single();
            order.AdressId = model.AdressId;
            order.isCompleted = model.isCompleted;
            db.SaveChanges();          
        }
    }
}
