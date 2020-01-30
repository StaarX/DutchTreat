using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController :Controller
    {
        
            private readonly IDutchRepository repository;
            private readonly ILogger logger;
        private readonly IMapper mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger
                                    ,IMapper mapper)
            {
                this.repository = repository;
                this.logger = logger;
            this.mapper = mapper;
        }

            [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Get(bool includeItems=true)
            {
                try
                {

                var results = repository.GetAllOrders(includeItems);

                return Ok(this.mapper.Map<IEnumerable<Order>,IEnumerable<OrderViewModel>>(results));
                }
                catch (Exception ex)
                {
                    logger.LogError($"Failed to get oreders: {ex}");
                return BadRequest("Failed to get Orders");
                }
            }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = this.repository.GetOrderById(id);
                if (order != null) return Ok(this.mapper.Map<Order,OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get Orders");
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid) {
                    var newOrder = this.mapper.Map<OrderViewModel, Order>(model);
                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }
                this.repository.AddEntity(newOrder);
                if (this.repository.SaveAll())
                {
                    return Created($"api/orders/{newOrder.Id}", this.mapper.Map<Order,OrderViewModel>(newOrder));
                }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to save a new order: {ex}");
            }
            return BadRequest("Failed to save new order");
        }
    }
    }

