namespace BasketApi.Domain;

public class CreateOrderRequest
{
    public double TotalAmount { get; set; }
    public IEnumerable<OrderLine> OrderLines { get; set; }
}
