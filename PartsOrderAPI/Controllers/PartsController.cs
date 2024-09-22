using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartsOrderAPI.Services;
using PartsOrderAPI.DTO;
using Serilog;

namespace PartsOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;
        private readonly Serilog.ILogger _logger;

        public PartsController(IPartService partService)
        {
            _partService = partService;
            _logger = Log.ForContext<PartsController>();
        }

        /// <summary>
        /// Gets the list of parts.
        /// </summary>
        /// <returns>List of parts</returns>
        [HttpGet]
        public IActionResult GetParts()
        {
            _logger.Information("GetParts method called.");

            try
            {
                var parts = _partService.GetParts();
                _logger.Information("Parts retrieved successfully.");
                return Ok(parts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving parts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the parts.");
            }
        }

        /// <summary>
        /// Creates a new part.
        /// </summary>
        /// <param name="partDto">Part data transfer object</param>
        /// <returns>Created part</returns>
        [HttpPost]
        public IActionResult CreatePart([FromBody] PartDto partDto)
        {
            _logger.Information("CreatePart method called.");

            try
            {
                var part = _partService.CreatePart(partDto);
                _logger.Information("Part created successfully with Id: {Id}", part.Id);
                return CreatedAtAction(nameof(GetParts), new { id = part.Id }, part);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating part.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the part.");
            }
        }
    }
}
