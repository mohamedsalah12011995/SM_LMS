using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RM.Orders.Dtos;
using RM.Orders.Records;
using RM.Orders.Services;
using System;

namespace RM.Orders.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController 
    {
        private readonly IOrderService _orderHandler;
        public OrdersController(IOrderService orderHandler)
        {
            _orderHandler = orderHandler;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOrderLookups(GetOrderLookupsRecord RequestedData)
        {
            return await _orderHandler.GetOrderLookups(RequestedData.Adapt<Dtos.Order>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddOrder(AddOrderRecord RequestedData)
        {
            return await _orderHandler.AddOrder(RequestedData.Adapt<Dtos.Order>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOrderList(GetOrderListRecord RequestedData)
        {
            return await _orderHandler.GetOrderList(RequestedData.Adapt<Dtos.Order>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOrderDetails(GetOrderDetailsRecord RequestedData)
        {
           return await _orderHandler.GetOrderDetails(RequestedData.Adapt<Dtos.Order>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddOrderAction(AddOrderActionRecord RequestedData)
        {
           return await _orderHandler.AddOrderAction(RequestedData.Adapt<Dtos.OrderActions>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteOrder(DeleteOrderRecord RequestedData)
        {
           return await _orderHandler.DeleteOrder(RequestedData.Adapt<Dtos.Order>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOrder(GetOrderRecord RequestedData)
        {
           return await _orderHandler.GetOrder(RequestedData.Adapt<Dtos.Order>());
        }
    }
}
