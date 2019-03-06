using System;
using System.Linq;

namespace Kata.Api.Models
{
  public class AddItemDto
  {
    public int ProductId { get; set; }
    public uint Qty { get; set; }
  }
}
