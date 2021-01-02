using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    /// Interacts with the Toys table
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //only authorized person can access anything in the controller
    public class ToysController : ControllerBase
    {
        private readonly IToyRepository _toyRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;


        public ToysController(IToyRepository toyRepository, ILoggerService logger, IMapper mapper, IWebHostEnvironment env)
        {
            _toyRepository = toyRepository;
            _logger = logger;
            _mapper = mapper;
            _env = env;
        }
        private string GetImagePath(string fileName)
            => ($"{_env.ContentRootPath}\\uploads\\{fileName}");


        /// <summary>
        /// Get all Toys
        /// </summary>
        /// <returns>A List of Toys</returns>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetToys()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call");
                var toys = await _toyRepository.FindAll();
                var response = _mapper.Map<IList<ToyDTO>>(toys);
                _logger.LogInfo($"{location}: Successful");
                foreach (var item in response)
                {
                    if (!string.IsNullOrEmpty(item.Image))
                    {
                        var imgPath = GetImagePath(item.Image);
                        if (System.IO.File.Exists(imgPath))
                        {
                            byte[] imgBytes = System.IO.File.ReadAllBytes(imgPath);
                            item.File = Convert.ToBase64String(imgBytes);
                        }
                    }
                }
                _logger.LogInfo($"{location}: Successful");


                return Ok(response);
            }
            catch (Exception e)
            {

                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
             
        }

        /// <summary>
        /// Gets a Toy by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> A List of Toys</returns>

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetToy(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call for id: {id}");
                var toy = await _toyRepository.FindById(id);
                if (toy == null)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var response = _mapper.Map<ToyDTO>(toy);
                if (!string.IsNullOrEmpty(response.Image))
                {
                    var imgPath = GetImagePath(toy.Image);
                    if (System.IO.File.Exists(imgPath))
                    {
                        byte[] imgBytes = System.IO.File.ReadAllBytes(imgPath);
                        response.File = Convert.ToBase64String(imgBytes);
                    }
                }

                _logger.LogInfo($"{location}: Successfully got record with id: {id}");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Creates a new toy
        /// </summary>
        /// <param name="toyDTO"></param>
        /// <returns>Toys Object</returns>

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ToyCreateDTO toyDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Create Attempted");
                if (toyDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty Request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);
                }
                var toy = _mapper.Map<Toy>(toyDTO);
                var isSuccess = await _toyRepository.Create(toy);
                if (!isSuccess)
                {
                    return internalError($"{location}: Creation failed");
                }
                if(!string.IsNullOrEmpty(toyDTO.File))
                {
                    var imgPath = GetImagePath(toyDTO.Image);
                    byte[] imageBytes = Convert.FromBase64String(toyDTO.File);
                    System.IO.File.WriteAllBytes(imgPath, imageBytes);
                }
                _logger.LogInfo($"{location}: Creation was successful");
                return Created("Create", new { toy });
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Update a Toy
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toyDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Update(int id, [FromBody] ToyUpdateDTO toyDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Updated Attempted on record with id: {id}");
                if (id < 1 || toyDTO == null || id != toyDTO.Id)
                {
                    _logger.LogWarn($"{location}:Update failed with bad data - id: {id}");
                    return BadRequest();
                }
                var isExists = await _toyRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was Incomplete");
                    return BadRequest(ModelState);

                }
                var oldImage = await _toyRepository.GetImageFileName(id);
                var book = _mapper.Map<Toy>(toyDTO);
                var isSuccess = await _toyRepository.Update(book);
                if (!isSuccess)
                {
                    return internalError($"{location}: Update failed for record with id: {id}");
                }

                if (!toyDTO.Image.Equals(oldImage))
                {
                    if (System.IO.File.Exists(GetImagePath(oldImage)))
                    {
                        System.IO.File.Delete(GetImagePath(oldImage));
                    }
                }

                if (!string.IsNullOrEmpty(toyDTO.File))
                {
                    byte[] imageBytes = Convert.FromBase64String(toyDTO.File);
                    System.IO.File.WriteAllBytes(GetImagePath(toyDTO.Image), imageBytes);
                }

                _logger.LogInfo($"{location}: Record with id: {id} successfully updated");
                return NoContent();
            }
            catch (Exception e)
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Removes an toy by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: delete Attempted on record with id: {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"{location}:Delete failed with bad data - id: {id}");
                    return BadRequest();
                }
                var isExists = await _toyRepository.isExists(id);
                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Failed to retrieve record with id: {id}");
                    return NotFound();
                }
                var toy = await _toyRepository.FindById(id);
                var isSuccess = await _toyRepository.Delete(toy);
                if (!isSuccess)
                {
                    return internalError($"{location}: Delete  failed for record with id: {id}"); 
                }
                _logger.LogInfo($"{location} : Record with id: {id} successfully deleted");
                return NoContent();

            }  
            catch (Exception e) 
            {
                return internalError($"{location}: {e.Message} - {e.InnerException}");
            }
               
            }  

        



        private string GetControllerActionNames()
        {

            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }
        private ObjectResult internalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong. Please reach out to the Administrator. Thank You!");
        }

    }
}
