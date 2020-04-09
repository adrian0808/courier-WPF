using CourierApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierApplication.DAL
{
    public interface IService<T> where T: class
    {
        List<T> LoadData();
        void UpdateData(T model);
        void AddData(T model);
        void DeleteData(T model);
    }
}
