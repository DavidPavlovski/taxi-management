using System.Collections.Generic;
using Taxi_Manager.Domain.Entities;

namespace Taxi_Manager.DataAccess.Interfaces
{
    public interface ITaxiDB<T> where T : BaseEntity
    {
        int Add(T Entity);
        bool RemoveById(int Id);
        bool Update(T entity);
        List<T> GetAll();
        T GetById(int id);
    }
}
