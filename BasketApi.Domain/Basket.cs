using BasketApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketApi.Application.Services;

public class Basket : Order
{
    public Guid BasketId { get; set; }
}
