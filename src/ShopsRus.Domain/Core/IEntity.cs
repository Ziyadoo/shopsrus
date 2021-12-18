namespace ShopsRus.Domain.Core
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    public interface IEntity : IEntity<int>
    {
        
    }
}