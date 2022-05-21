namespace Taxi_Manager.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public abstract string Print();
    }
}
