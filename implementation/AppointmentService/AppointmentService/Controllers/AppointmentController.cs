using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentInterface _appointmentRepository;

        public AppointmentController(AppointmentInterface appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        // GET api/Appointment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> Get(int id)
        {
            await Task.Delay(11000);
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        // POST api/Appointment (insert new appointment)
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            await _appointmentRepository.InsertAppointment(appointment);
            return CreatedAtAction(nameof(Get), new { id = appointment.AppointmentID }, appointment);
        }

        // PUT api/Appointment/{id} (update appointment)
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Appointment appointment)
        {
            if (id != appointment.AppointmentID)
            {
                return BadRequest();
            }

            var existingAppointment = await _appointmentRepository.GetAppointmentById(id);
            if (existingAppointment == null)
            {
                return NotFound();
            }

            await _appointmentRepository.UpdateAppointment(appointment);
            return Ok(appointment);
        }

        // DELETE api/Appointment/{id} (Delete appointment)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }

            await _appointmentRepository.DeleteAppointment(id);
            return Ok(appointment);
        }
    }
}
