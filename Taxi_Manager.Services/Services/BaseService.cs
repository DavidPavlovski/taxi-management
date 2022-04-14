using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.DataAccess;
using Taxi_Manager.DataAccess.Interfaces;
using Taxi_Manager.Domain.Entities;
using Taxi_Manager.Services.Interfaces;

namespace Taxi_Manager.Services.Services
{
    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        public ITaxiDB<T> Db;
        public BaseService()
        {
            Db = new TaxiDB<T>();
        }
        public bool Add(T entity)
        {
            try
            {
                Db.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<T> GetAll()
        {
            return Db.GetAll();
        }
        public T GetById(int id)
        {
            return Db.GetById(id);
        }
        public bool Remove(int id)
        {
            return Db.RemoveById(id);
        }
    }
}
