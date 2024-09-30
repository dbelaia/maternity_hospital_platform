using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentHistoryController : ControllerBase
    {
        private readonly AppointmentHistoryInterface _appointmentHistoryRepository;

        public AppointmentHistoryController(AppointmentHistoryInterface appointmentHistoryRepository)
        {
            _appointmentHistoryRepository = appointmentHistoryRepository;
        }

        // GET api/AppointmentHistory/{id} - Get appointment history by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentHistory>> Get(int id)
        {
            var appointmentHistory = await _appointmentHistoryRepository.GetAppointmentHistoryById(id);
            if (appointmentHistory == null)
            {
                return NotFound();
            }
            return Ok(appointmentHistory);
        }

        // GET api/AppointmentHistory/Status/{statusID} - Get all appointments by status ID
        [HttpGet("Status/{statusID}")]
        public async Task<ActionResult<IEnumerable<AppointmentHistory>>> GetAppointmentsByStatus(int statusID)
        {
            var appointments = await _appointmentHistoryRepository.GetAppointmentsByStatusId(statusID);
            if (appointments == null || !appointments.Any())
            {
                return NotFound();
            }
            return Ok(appointments);
        }

        // POST api/AppointmentHistory - Insert new appointment history
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AppointmentHistory appointmentHistory)
        {
            if (appointmentHistory == null)
            {
                return BadRequest();
            }

            await _appointmentHistoryRepository.InsertAppointmentHistory(appointmentHistory);
            return CreatedAtAction(nameof(Get), new { id = appointmentHistory.AppointmentHistoryID }, appointmentHistory);
        }

        // PUT api/AppointmentHistory/{id} - Update appointment history
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AppointmentHistory appointmentHistory)
        {
            if (id != appointmentHistory.AppointmentHistoryID)
            {
                return BadRequest();
            }

            var existingAppointmentHistory = await _appointmentHistoryRepository.GetAppointmentHistoryById(id);
            if (existingAppointmentHistory == null)
            {
                return NotFound();
            }

            await _appointmentHistoryRepository.UpdateAppointmentHistory(appointmentHistory);
            return NoContent();
        }

        // DELETE api/AppointmentHistory/{id} - Delete appointment history
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var appointmentHistory = await _appointmentHistoryRepository.GetAppointmentHistoryById(id);
            if (appointmentHistory == null)
            {
                return NotFound();
            }

            await _appointmentHistoryRepository.DeleteAppointmentHistory(id);
            return NoContent();
        }
    }
}
