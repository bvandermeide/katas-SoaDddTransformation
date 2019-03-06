using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kata.Api.Models
{
  public class Order
  {
    public Order()
    {
    }

    public int OrderId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; }
    public int CustomerId { get; set; }

    public double GrandTotal { get; set; }
    public double SubTotal { get; set; }
    public double TaxAmount { get; set; }
    public OrderStatus State { get; set; }
  }
}