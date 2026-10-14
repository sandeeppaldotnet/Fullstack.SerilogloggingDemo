using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fullstack.SerilogloggingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            _logger.LogInformation(
                "Fetching all products at {RequestTime}",
                DateTimeOffset.UtcNow);

            var products = new[]
            {
            new { Id = 1, Name = "Laptop", Price = 75000 },
            new { Id = 2, Name = "Keyboard", Price = 2500 }
        };

            _logger.LogInformation(
                "Retrieved {ProductCount} products",
                products.Length);

            return Ok(products);
        }

        /// <summary>
        /// Get a product by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id)
        {
            using (_logger.BeginScope(
                new Dictionary<string, object>
                {
                    ["ProductId"] = id
                }))
            {
                _logger.LogInformation(
                    "Processing product request");

                if (id <= 0)
                {
                    _logger.LogWarning(
                        "Invalid product ID received");

                    return BadRequest(new
                    {
                        message = "Product ID must be greater than zero."
                    });
                }

                if (id == 999)
                {
                    _logger.LogError(
                        "Product was not found");

                    return NotFound(new
                    {
                        message = "Product not found."
                    });
                }

                return Ok(new
                {
                    Id = id,
                    Name = "Laptop",
                    Price = 75000
                });
            }
        }
        //[HttpGet("{id:int}")]
        //public IActionResult GetProduct(int id)
        //{
        //    _logger.LogInformation(
        //        "Fetching product with ID {ProductId}",
        //        id);

        //    if (id <= 0)
        //    {
        //        _logger.LogWarning(
        //            "Invalid product ID received: {ProductId}",
        //            id);

        //        return BadRequest(new
        //        {
        //            message = "Product ID must be greater than zero."
        //        });
        //    }

        //    if (id == 999)
        //    {
        //        _logger.LogError(
        //            "Product {ProductId} was not found",
        //            id);

        //        return NotFound(new
        //        {
        //            message = "Product not found."
        //        });
        //    }

        //    return Ok(new
        //    {
        //        Id = id,
        //        Name = "Laptop",
        //        Price = 75000
        //    });
        //}

        [HttpGet("error")]
        public IActionResult GenerateError()
        {
            try
            {
                throw new InvalidOperationException(
                    "Simulated product processing failure.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while processing products");

                return StatusCode(500, new
                {
                    message = "An unexpected error occurred."
                });
            }
        }
    }
}

