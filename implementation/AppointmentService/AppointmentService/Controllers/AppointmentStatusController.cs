using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentStatusController : ControllerBase
    {
        private readonly AppointmentStatusInterface _appointmentStatusRepository;

        public AppointmentStatusController(AppointmentStatusInterface appointmentStatusRepository)
        {
            _appointmentStatusRepository = appointmentStatusRepository;
        }

        // GET api/AppointmentStatus/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentStatus>> Get(int id)
        {
            var appointmentStatus = await _appointmentStatusRepository.GetAppointmentStatusById(id);
            if (appointmentStatus == null)
            {
                return NotFound();
            }
            return Ok(appointmentStatus);
        }

        // POST api/AppointmentStatus (insert new appointment status)
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AppointmentStatus appointmentStatus)
        {
            if (appointmentStatus == null)
            {
                return BadRequest();
            }

            await _appointmentStatusRepository.InsertAppointmentStatus(appointmentStatus);
            return CreatedAtAction(nameof(Get), new { id = appointmentStatus.AppointmentStatusID }, appointmentStatus);
        }

        // PUT api/AppointmentStatus/{id} (update appointment status)
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AppointmentStatus appointmentStatus)
        {
            if (id != appointmentStatus.AppointmentStatusID)
            {
                return BadRequest();
            }

            var existingStatus = await _appointmentStatusRepository.GetAppointmentStatusById(id);
            if (existingStatus == null)
            {
                return NotFound();
            }

            await _appointmentStatusRepository.UpdateAppointmentStatus(appointmentStatus);
            return NoContent();
        }

        // DELETE api/AppointmentStatus/{id} (Delete appointment status)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var appointmentStatus = await _appointmentStatusRepository.GetAppointmentStatusById(id);
            if (appointmentStatus == null)
            {
                return NotFound();
            }

            await _appointmentStatusRepository.DeleteAppointmentStatus(id);
            return NoContent();
        }
    }
}
