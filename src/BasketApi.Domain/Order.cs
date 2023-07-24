namespace BasketApi.Domain;

public class Order
{
    public string? OrderId { get; set;}
    public double TotalAmount { get;set;}
    public List<OrderLine> OrderLines { get; set;}
}