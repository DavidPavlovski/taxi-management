using System;
using System.Collections.Generic;
using System.Linq;
using Taxi_Manager.DataAccess.Interfaces;
using Taxi_Manager.Domain.Entities;

namespace Taxi_Manager.DataAccess
{
    public class TaxiDB<T> : ITaxiDB<T> where T : BaseEntity
    {
        public int IdCounter { get; set; }
        public List<T> Db;
        public TaxiDB()
        {
            Db = new List<T>();
            IdCounter = 1;
        }
        public int Add(T entity)
        {
            entity.Id = IdCounter++;
            Db.Add(entity);
            return entity.Id;
        }
        public List<T> GetAll()
        {
            return Db;
        }
        public T GetById(int id)
        {
            return Db.Single(e => e.Id == id);
        }
        public bool RemoveById(int Id)
        {
            try
            {
                T entity = Db.Single(e => e.Id == Id);
                Db.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Update(T entity)
        {
            try
            {
                T dbEntity = Db.Single(e => e.Id == entity.Id);
                Db.Remove(dbEntity);
                Db.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
