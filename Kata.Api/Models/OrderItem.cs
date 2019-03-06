using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kata.Api.Models
{
  public class OrderItem
  {
    public OrderItem()
    {
    }
    public int OrderItemId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; }
    public int OrderId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }
    public int ProductId { get; set; }
    public uint Quantity { get; set; }
    public double ItemPrice { get; set; }
  }
}