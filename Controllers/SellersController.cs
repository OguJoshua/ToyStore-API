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
    /// Endpoint used to interact with the sellers in the Toy store's database
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class sellersController : ControllerBase
    {
        
        private readonly ISellerRepository _sellerRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;


        public sellersController(ISellerRepository sellerRepository, ILoggerService logger, IMapper mapper)
        {
            _sellerRepository = sellerRepository;
            _logger = logger;
             _mapper = mapper;
    }
        /// <summary>
        /// Get all sellers
        /// </summary>
        /// <returns>LIST OF sellerS</returns>

        [HttpGet]
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       

        public async Task<IActionResult> Getsellers()
        {
            var location = GetControllerActionNames();

            try
            {
                _logger.LogInfo($"{location}: Attempted call");
                var sellers = await _sellerRepository.FindAll();
                var response = _mapper.Map<IList<SellerDTO>>(sellers);
                _logger.LogInfo($"{location}: Successfully");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
               
            }
            
        }
        /// <summary>
        /// Get a seller by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A seller's record</returns>
        [HttpGet("{id}")]
       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Getseller(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call for  id:{id}");
                var seller = await _sellerRepository.FindById(id);
                if (seller == null)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id:{id}");
                    return NotFound();
                }
                var response = _mapper.Map<SellerDTO>(seller);
                _logger.LogInfo($"{location} : Succesfully got record with id:{id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
         }
        /// <summary>
        /// Creates a seller
        /// </summary>
        /// <param name="seller"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles ="Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] SellerCreateDTO sellerDTO)
        {
            var location = GetControllerActionNames();

            try
            {
                _logger.LogInfo($"{location}: Create Attempted");
                if (sellerDTO == null)
                {
                    _logger.LogWarn("Empty Request was submitted");
                    return BadRequest(ModelState);
                }
                if(!ModelState.IsValid)
                {

                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var seller = _mapper.Map<Seller>(sellerDTO);
                var isSuccess = await _sellerRepository.Create(seller);
                if (!isSuccess)
                {
                    return internalError($"{location}: Creation failed! ");
                }
                _logger.LogInfo($"{location}: Creation was successful");               
                return Created("Create", new { seller });
            }
            catch (Exception e)
            {

                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
            
        }

        /// <summary>
        /// Updates a seller
        /// </summary>
        /// <param name="id"></param>param>
        /// <param name="sellerDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, Customer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] SellerUpdateDTO sellerDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"seller with id : {id} Update Attempted");
                if (id < 1 || sellerDTO == null || id != sellerDTO.Id)
                {
                    _logger.LogWarn($"{location}: Update failed with bad data - id: {id}");
                    return BadRequest();
                }
                var isExists = await _sellerRepository.isExists(id);
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
                var seller = _mapper.Map<Seller>(sellerDTO);
                var isSuccess = await _sellerRepository.Update(seller);
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
        /// Removes a seller by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
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
                var isExists = await _sellerRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id:  {id}");
                    return NotFound();
                }

                var seller = await _sellerRepository.FindById(id);
               
                var isSuccess = await _sellerRepository.Delete(seller);
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