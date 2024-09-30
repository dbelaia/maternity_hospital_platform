using AnalyticsService.Repository;
using Microsoft.AspNetCore.Mvc;


namespace AnalyticsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly OperationInterface _operationRepository;

        public OperationController(OperationInterface operationRepository)
        {
            _operationRepository = operationRepository;
        }

        // GET api/Operation/{id} - get operation by id
        [HttpGet("{id}")]
        public async Task<ActionResult<AnalyticsService.Models.Operation>> Get(int id)
        {
            var operation = await _operationRepository.GetOperationByIdAsync(id); 
            if (operation == null)
            {
                return NotFound();
            }
            return Ok(operation);
        }

        // POST api/Operation - insert new operation
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AnalyticsService.Models.Operation operation)
        {
            if (operation == null)
            {
                return BadRequest();
            }

            await _operationRepository.InsertOperationAsync(operation); 
            return CreatedAtAction(nameof(Get), new { id = operation.OperationID }, operation);
        }

        // PUT api/Operation/{id} - update operation
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AnalyticsService.Models.Operation operation)
        {
            if (id != operation.OperationID)
            {
                return BadRequest();
            }

            var existingOperation = await _operationRepository.GetOperationByIdAsync(id); 
            if (existingOperation == null)
            {
                return NotFound();
            }

            await _operationRepository.UpdateOperationAsync(operation);
            return Ok(operation);
        }

        // DELETE api/Operation/{id} - delete operation
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var operation = await _operationRepository.GetOperationByIdAsync(id); 
            if (operation == null)
            {
                return NotFound();
            }

            await _operationRepository.DeleteOperationAsync(id);
            return Ok(operation);
        }
    }
}
