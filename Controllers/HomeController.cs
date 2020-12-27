using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToyStore_API.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToyStore_API.Controllers
{

    /// <summary>
    /// This is a test API controller
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILoggerService _logger;

        public HomeController(ILoggerService logger)

        {
            _logger = logger;
        }
        /// <summary>
        /// Gets Values
        /// </summary>
        /// <returns></returns>
        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Accesed Home Controller");
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Get a Value
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        // GET api/<HomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogDebug("Get a Value");
            return "value";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>

        // POST api/<HomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _logger.LogError("This is an Error");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _logger.LogWarn("This is a warning");
        }
    }
}
