using Kata.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Kata.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrderingController : ControllerBase
  {
    readonly OrderingDbContext orderingDbContext;
    public OrderingController(OrderingDbContext dbContext)
    {
      this.orderingDbContext = dbContext;
    }


    // POST api/ordering/new
    [HttpPost("new")]
    public ActionResult Create()
    {

      using (new TransactionScope())
      {
        Customer customer;
        if (!TryGetCustomerFromContext(out customer)) {
          return Unauthorized();
        }
        var order = new Order()
        {
          Customer = customer
        };

        this.orderingDbContext.Orders.Add(order);
        this.orderingDbContext.SaveChanges();

        return new JsonResult(new { OrderId = order.OrderId });
      }
    }

    // PUT api/ordering [body=itemDto]
    [HttpPut("{orderId}")]
    public ActionResult AddItem(int orderId, [FromBody] AddItemDto item)
    {
      using (new TransactionScope())
      {
        Customer customer;
        if (!TryGetCustomerFromContext(out customer))
          return Unauthorized();

        Order order;
        if (!TryGetOrder(orderId, out order))
          return BadRequest();

        Product product;
        if (!TryGetProduct(item.ProductId, out product))
          return BadRequest();

        AddItemToOrder(item, order, product);
      }

      return Ok();
    }

    private void AddItemToOrder(AddItemDto item, Order order, Product product)
    {
      var orderItems = this.orderingDbContext
        .Items
        .Where(x => x.OrderId == order.OrderId)
        .ToList();

      var orderItem = orderItems.Where(x => x.ProductId == product.ProductId).SingleOrDefault();
      if (orderItem == null)
      {
        orderItem = new OrderItem()
        {
          OrderId = order.OrderId,
          ItemPrice = product.SalePrice,
          Quantity = item.Qty,
          ProductId = product.ProductId
        };
        orderItems.Add(orderItem);
        this.orderingDbContext.Items.Add(orderItem);
      }
      else
      {
        orderItem.Quantity += item.Qty;
      }

      RecalculateOrderTotals(order, orderItems);
      this.orderingDbContext.SaveChanges();
    }
    void RecalculateOrderTotals(Order order, List<OrderItem> orderItems)
    {
      order.SubTotal = orderItems.Sum(x => x.ItemPrice * Convert.ToDouble(x.Quantity));
      order.TaxAmount = order.SubTotal * 0.1d;
      order.GrandTotal = order.SubTotal + order.TaxAmount;
    }

    private bool TryGetCustomerFromContext(out Customer result)
    {
      result = null;
      int customerId;
      if(!TryGetCustomerIdFromContext(out customerId))
      {
        return false;
      }

      var customer = this.orderingDbContext.Customers.Find(customerId);
      result = customer;
      return result != null;
    }

    private bool TryGetCustomerIdFromContext(out int id)
    {
      int customerId = 0;
      var customerName = this.User?.Identity?.Name;
      if (!int.TryParse(customerName, out customerId))
      {
        id = 0;
        return false;
      }
      id = customerId;
      return true;
    }
    bool TryGetOrder(int orderId, out Order order)
    {
      order = this.orderingDbContext.Orders.Find(orderId);
      return order != null;
    }

    private bool TryGetProduct(int productId, out Product product)
    {
      product = this.orderingDbContext.Products.Find(productId);
      return product != null;      
    }

  }
}