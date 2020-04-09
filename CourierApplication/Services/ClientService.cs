using CourierApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CourierApplication.DAL
{
    class ClientService : IService<Client>
    {
        private CourierDbContext db;
        public ClientService()
        {
            db = new CourierDbContext();
        }

        public ClientService(CourierDbContext db)
        {
            this.db = db;
        }

        public void AddData(Client model)
        {
            db.Add(model);
            db.SaveChanges();
        }

        public void DeleteData(Client model)
        {
            Client client = db.Clients.Where(c => c.ClientId == model.ClientId).Single();
            db.Remove(client);
            db.SaveChanges();
        }

        public List<Client> LoadData()
        {
            return db.Clients.ToList();
        }

        public void UpdateData(Client model)
        {
            Client client = db.Clients.Where(c => c.ClientId == model.ClientId).Single();
            client.AdressId = model.AdressId;
            db.SaveChanges();          
        }
    }
}
