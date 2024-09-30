using AnalyticsService.Models;
using AnalyticsService.Repository;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationHistoryController : ControllerBase
    {
        private readonly OperationHistoryInterface _operationHistoryRepository;

        public OperationHistoryController(OperationHistoryInterface operationHistoryRepository)
        {
            _operationHistoryRepository = operationHistoryRepository;
        }

        // GET api/OperationHistory/{id} - get historical record by id
        [HttpGet("{id}")]
        public async Task<ActionResult<OperationHistory>> Get(int id) 
        {
            var operationHistory = await _operationHistoryRepository.GetOperationHistoryById(id); 

            if (operationHistory == null)
            {
                return NotFound($"Operation history with ID {id} not found.");
            }

            return Ok(operationHistory);
        }

        // GET api/OperationHistory/{startDateTime,endDateTime} - get all historical records withn some time period
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationHistory>>> Get([FromQuery] DateTime startDateTime, [FromQuery] DateTime endDateTime) 
        {
            var operations = await _operationHistoryRepository.GetOperationsByDateRange(startDateTime, endDateTime); 

            if (operations == null || !operations.Any())
            {
                return NotFound("No operations found within the specified date range.");
            }

            return Ok(operations);
        }

        // POST api/OperationHistory - insert new historical record
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OperationHistory operationHistory) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _operationHistoryRepository.InsertOperationHistory(operationHistory); 

            return CreatedAtAction(nameof(Get), new { id = operationHistory.OperationHistoryID }, operationHistory);
        }

        // PUT api/OperationHistory/{id} - update historical record
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OperationHistory operationHistory) 
        {
            if (id != operationHistory.OperationHistoryID)
            {
                return BadRequest("OperationHistory ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingOperationHistory = await _operationHistoryRepository.GetOperationHistoryById(id);
            if (existingOperationHistory == null)
            {
                return NotFound($"Operation history with ID {id} not found.");
            }

            await _operationHistoryRepository.UpdateOperationHistory(operationHistory);

            return Ok(operationHistory);
        }

        // DELETE api/OperationHistory/{id} - delete historical record
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var operationHistory = await _operationHistoryRepository.GetOperationHistoryById(id); 
            if (operationHistory == null)
            {
                return NotFound($"Operation history with ID {id} not found.");
            }

            await _operationHistoryRepository.DeleteOperationHistory(id);

            return Ok(operationHistory);
        }
    }
}
