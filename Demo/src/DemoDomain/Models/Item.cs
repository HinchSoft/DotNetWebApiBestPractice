using Hinchsoft.BestPractices.Domain;

namespace DemoDomain.Models;

public record Item(
    ItemId Id,
    string Name,
    string Description,
    int QuantityOnHand,
    decimal UnitPrice
);

public record ItemId(Guid Value) : EntityId(Value);
