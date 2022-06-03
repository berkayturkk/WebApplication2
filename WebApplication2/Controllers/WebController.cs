
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2;

namespace MathAPI.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class WebController : ControllerBase
    {
        private readonly ILogger<WebController> _logger;
        public WebController(ILogger<WebController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ActionName("GetCustomers")]
        public List<Customer> Get()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Northwind;Integrated Security=true";

            SqlCommand cmd = new SqlCommand("SELECT CustomerID FROM Customers");
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = cn;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Customer> CustomerList = new List<Customer>();

            while (dr.Read())
            {
                Customer customer = new Customer();
                customer.CustomerID = dr.GetString(0);
                CustomerList.Add(customer);
            }
            cn.Close();
            return CustomerList;
        }

        [HttpGet]
        [ActionName("GetOrder")]
        public List<Order> GetOrders(string CustomerId, int OrderId)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Northwind;Integrated Security=true";

            SqlCommand cmd = new SqlCommand("SELECT o.CustomerID,o.OrderID FROM Customers c inner join Orders o on c.CustomerID=o.CustomerID inner join [Order Details] od on o.OrderID=od.OrderID where o.CustomerID='" + CustomerId + "' group by  o.CustomerID,o.OrderID");
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = cn;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            List<Order> OrdersList = new List<Order>();

            while (dr.Read())
            {
                Order order = new Order();
                order.CustomerID = dr.GetString(0);
                order.OrderID = dr.GetInt32(1);
                OrdersList.Add(order);
            }
            cn.Close();
            return OrdersList;
        }


        [HttpGet]
        [ActionName("GetTotalPrice")]
        public TotalPrices GetTotalPrice(string CustomerId, int OrderId)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Northwind;Integrated Security=true";

            SqlCommand cmd = new SqlCommand("SELECT o.CustomerID,o.OrderID,Sum(od.Quantity*od.UnitPrice) FROM Customers c inner join Orders o on c.CustomerID=o.CustomerID inner join [Order Details] od on o.OrderID=od.OrderID where o.CustomerID='"+CustomerId+"' and o.OrderID = "+OrderId+" group by  o.CustomerID,o.OrderID");
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = cn;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            
            TotalPrices TotalPrice = new TotalPrices();
            while (dr.Read())
            {
                
                TotalPrice.CustomerID = dr.GetString(0);
                TotalPrice.OrderID = dr.GetInt32(1);
                TotalPrice.TotalPrice = dr.GetDecimal(2);
                
            }
            cn.Close();
            return TotalPrice;
        }





    }
}