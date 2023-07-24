using BasketApi.Domain;

namespace BasketApi.Application.Services;

public sealed class Basket
{
    public Guid BasketId { get; private set; }

    public Basket()
    {
        BasketId = Guid.NewGuid();
        OrderLines = new();
        TotalAmount = 0;
    }
    public double TotalAmount { get; set; }
    public List<OrderLine> OrderLines { get; set; }
}
