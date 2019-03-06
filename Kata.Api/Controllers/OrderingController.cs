using Kata.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Kata.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrderingController : ControllerBase
  {
    readonly OrderingDbContext orderingDbContext;

    public OrderingController(OrderingDbContext dbContext) => this.orderingDbContext = dbContext;

    // PUT api/ordering/orders/{orderId}/item [body=itemDto]
    [HttpPut("orders/{orderId}/item")]
    public ActionResult AddItem(int orderId, [FromBody] AddItemDto item)
    {
      using (var trx = this.orderingDbContext.Database.BeginTransaction())
      {
        Customer customer;
        if (!TryGetCustomerFromContextOrCookie(out customer))
          return Unauthorized();

        Order order;
        if (!TryGetOrder(orderId, out order))
          return BadRequest();

        Product product;
        if (!TryGetProduct(item.ProductId, out product))
          return BadRequest();

        AddItemToOrder(item, order, product);

        this.orderingDbContext.SaveChanges();
        trx.Commit();

        return Ok(order);
      }

    }

    // POST api/ordering/orders/new
    [HttpPost("orders/new")]
    public ActionResult Create()
    {
      using (var trx = this.orderingDbContext.Database.BeginTransaction())
      {
        Customer customer = EnsureCustomerInContextOrCookie();

        Order order = new Order()
        {
          Customer = customer
        };

        this.orderingDbContext.Orders.Add(order);

        this.orderingDbContext.SaveChanges();
        trx.Commit();

        return Ok(new { OrderId = order.OrderId });
      }
    }

    // POST api/ordering/orders/{orderId}/place
    [HttpPost("orders/{orderId}/place")]
    public ActionResult PlaceOrder(int orderId)
    {
      using (var trx = this.orderingDbContext.Database.BeginTransaction())
      {
        Order order;
        if (!TryGetOrder(orderId, out order))
          return BadRequest();

        if (order.State != OrderStatus.Placing)
          return BadRequest();

        order.State = OrderStatus.Placed;
        this.orderingDbContext.SaveChanges();
        trx.Commit();

        return Ok();
      }
    }

    // GET api/ordering/orders
    [HttpGet("orders")]
    public ActionResult GetOrders()
    {
      var customer = EnsureCustomerInContextOrCookie();
      var orders = this.orderingDbContext.Orders.Where(x => x.Customer == customer).ToList();
      return Ok(orders);
    }

    // GET api/ordering/orders/{orderId}
    [HttpGet("orders/{orderId}")]
    public ActionResult GetOrder(int orderId)
    {
      var customer = EnsureCustomerInContextOrCookie();
      var orders = this.orderingDbContext.Orders.Where(x => x.Customer == customer && x.OrderId == orderId).ToList();
      return Ok(orders);
    }

    // GET api/ordering/version
    [HttpGet("version")]
    public ActionResult Version() => Ok(new { version = "1.0" });

    void AddItemToOrder(AddItemDto item, Order order, Product product)
    {
      List<OrderItem> orderItems = this.orderingDbContext
        .Items
        .Where(x => x.OrderId == order.OrderId)
        .ToList();

      OrderItem orderItem = orderItems.Where(x => x.ProductId == product.ProductId).SingleOrDefault();
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
        orderItem.Quantity += item.Qty;

      RecalculateOrderTotals(order, orderItems);
      this.orderingDbContext.SaveChanges();
    }

    Customer CreateGuestCustomer()
    {
      Customer customer = new Customer()
      {
        IsGuest = true
      };
      this.orderingDbContext.Customers.Add(customer);
      this.orderingDbContext.SaveChanges();
      return customer;
    }

    Customer EnsureCustomerInContextOrCookie()
    {
      Customer customer;
      if (!TryGetCustomerFromContextOrCookie(out customer))
      {
        customer = CreateGuestCustomer();

        Response.Cookies
          .Append(
          "GuestCustomerId",
          customer.CustomerId.ToString(),
          new CookieOptions()
          { HttpOnly = true, Secure = true });
      }

      return customer;
    }

    private bool TryGetCustomerFromContextOrCookie(out Customer customer)
    {
      if (!TryGetCustomerFromContext(out customer))
      {
        int guestCustomerId;
        if (int.TryParse(Request.Cookies["GuestCustomerId"], out guestCustomerId))
        {
          if (!TryGetCustomerById(guestCustomerId, out customer))
            return false;
        }
        else
        {
          return false;
        }
      }
      return true;
    }

    void RecalculateOrderTotals(Order order, List<OrderItem> orderItems)
    {
      order.SubTotal = orderItems.Sum(x => x.ItemPrice * Convert.ToDouble(x.Quantity));
      order.TaxAmount = order.SubTotal * 0.1d;
      order.GrandTotal = order.SubTotal + order.TaxAmount;
    }

    bool TryGetCustomerById(int customerId, out Customer result)
    {
      Customer customer = this.orderingDbContext.Customers.Find(customerId);
      result = customer;
      return result != null;
    }

    bool TryGetCustomerFromContext(out Customer result)
    {
      result = null;
      int customerId;
      if (!TryGetCustomerIdFromContext(out customerId))
        return false;
      return TryGetCustomerById(customerId, out result);
    }

    bool TryGetCustomerIdFromContext(out int id)
    {
      int customerId = 0;
      string customerName = User?.Identity?.Name;
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

    bool TryGetProduct(int productId, out Product product)
    {
      product = this.orderingDbContext.Products.Find(productId);
      return product != null;
    }
  }
}