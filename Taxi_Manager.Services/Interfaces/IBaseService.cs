using System;
using System.Collections.Generic;
using System.Text;
using Taxi_Manager.Domain.Entities;

namespace Taxi_Manager.Services.Interfaces
{
    public interface IBaseService<T> where T : BaseEntity
    {
        bool Add(T entity);
        bool Remove(int id);
        T GetById(int id);
        List<T> GetAll();
        void Seed(List<T> items);
        bool Update(T entity);
    }
}
