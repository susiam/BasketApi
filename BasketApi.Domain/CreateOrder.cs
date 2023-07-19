namespace BasketApi.Domain;

public class CreateOrder
{
    public string? UserEmail { get; set; }
    public double TotalAmount { get; set; }
    public IEnumerable<OrderLine> OrderLines { get; set; }
}
