namespace EventMicroService.Domain.Common;

public abstract class IdentifiedEntity<Key> : BaseEntity
{
    public Key Id { get; set; } = default!;
}

public abstract class IdentifiedEntity : IdentifiedEntity<long>
{

}
