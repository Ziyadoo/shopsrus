namespace ShopsRus.Domain.Core
{
    public class Entity : Entity<int>, IEntity
    {
        
    }

    public class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}