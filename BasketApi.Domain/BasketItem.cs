using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketApi.Domain;

public class BasketItem : Product
{
    public int Quantity { get; set; }
}
