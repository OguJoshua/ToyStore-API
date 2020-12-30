using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToyStore_API.Contracts;
using ToyStore_API.Data;
using ToyStore_API.DTOs;



namespace ToyStore_API.Controllers
{

    /// <summary>
    /// Endpoint used to interact with the Manufacturers in the Toy store's database
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class ManufacturersController : ControllerBase
    {
        
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;


        public ManufacturersController(IManufacturerRepository manufacturerRepository, ILoggerService logger, IMapper mapper)
        {
            _manufacturerRepository = manufacturerRepository;
            _logger = logger;
             _mapper = mapper;
    }
        /// <summary>
        /// Get all manufacturers
        /// </summary>
        /// <returns>LIST OF MANUFACTURERS</returns>

        [HttpGet]
        [Authorize(Roles = "Customer, Administrators")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       

        public async Task<IActionResult> GetManufacturers()
        {
            var location = GetControllerActionNames();

            try
            {
                _logger.LogInfo($"{location}Attempted call");
                var manufacturers = await _manufacturerRepository.FindAll();
                var response = _mapper.Map<IList<ManufacturerDTO>>(manufacturers);
                _logger.LogInfo("Successfully got all Manufacturers");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
               
            }
            
        }
        /// <summary>
        /// Get a manufacturer by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Manufacturer's record</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer, Administrators")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetManufacturer(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call for  id:{id}");
                var manufacturer = await _manufacturerRepository.FindById(id);
                if (manufacturer == null)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id:{id}");
                    return NotFound();
                }
                var response = _mapper.Map<ManufacturerDTO>(manufacturer);
                _logger.LogInfo($"{location} : Succesfully got record with id:{id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
         }
        /// <summary>
        /// Creates a manufacturer
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ManufacturerCreateDTO manufacturerDTO)
        {
            var location = GetControllerActionNames();

            try
            {
                _logger.LogInfo($"{location}: Create Attempted");
                if (manufacturerDTO == null)
                {
                    _logger.LogWarn("Empty Request was submitted");
                    return BadRequest(ModelState);
                }
                if(!ModelState.IsValid)
                {

                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var manufacturer = _mapper.Map<Manufacturer>(manufacturerDTO);
                var isSuccess = await _manufacturerRepository.Create(manufacturer);
                if (!isSuccess)
                {
                    return internalError($"{location}: Creation failed! ");
                }
                _logger.LogInfo($"{location}: Creation was successful");               
                return Created("Create", new { manufacturer });
            }
            catch (Exception e)
            {

                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
            
        }

        /// <summary>
        /// Updates a manufacturer
        /// </summary>
        /// <param name="id"></param>param>
        /// <param name="manufacturerDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, Customer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ManufacturerUpdateDTO manufacturerDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"Manufacturer with id : {id} Update Attempted");
                if (id < 1 || manufacturerDTO == null || id != manufacturerDTO.Id)
                {
                    _logger.LogWarn($"{location}: Update failed with bad data - id: {id}");
                    return BadRequest();
                }
                var isExists = await _manufacturerRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}:  failed  to retrieve record with id: {id}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Data was Incomplete ");
                    return BadRequest(ModelState);
                }
                var manufacturer = _mapper.Map<Manufacturer>(manufacturerDTO);
                var isSuccess = await _manufacturerRepository.Update(manufacturer);
                if (!isSuccess)
                {
                    return internalError($"{location}: Update  Failed for record with id: {id} ");
                }
                _logger.LogInfo($"{location}: Record with id:  {id} successfully updated");
                return NoContent();
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");

            }
        }

        /// <summary>
        /// Removes a manufacturer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Delete Attempted on record with id: {id}");
                if(id < 1)
                {
                    _logger.LogWarn($"{location}: Delete failed with bad data - id:{id}");
                    return BadRequest();

                }
                var isExists = await _manufacturerRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id:  {id}");
                    return NotFound();
                }

                var manufacturer = await _manufacturerRepository.FindById(id);
               
                var isSuccess = await _manufacturerRepository.Delete(manufacturer);
                if(!isSuccess)
                {
                    return internalError($"{location}: Delete Failed for record  with id:  {id}");

                }
                _logger.LogInfo($"{location}: Record with id:  {id} successfully delete");
                return NoContent();
            }
            catch (Exception e)
            {

                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        
        private ObjectResult internalError(string message)
        {
        _logger.LogError(message);
        return StatusCode(500, "Something went wrong. Please contact the Administrator. Thank You!");
        }



        private string GetControllerActionNames()
        {

            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }






    }


}