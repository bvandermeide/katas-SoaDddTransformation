using System;
using System.Linq;

namespace Kata.Api.Models
{
  public class AddItemDto
  {
    public int ProductId { get; }
    public uint Qty { get; set; }
  }
}
