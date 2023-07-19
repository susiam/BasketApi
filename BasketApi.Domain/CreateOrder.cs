namespace BasketApi.Domain;

public class CreateOrder
{
    public string UserEmail { get;}
    public double TotalAmount { get;}
    public IEnumerable<OrderLine> OrderLines { get;}
}
