using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorInterface _doctorRepository;

        public DoctorController(DoctorInterface doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        // GET api/Doctor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> Get(int id)
        {
            // make sleep
            var doctor = await _doctorRepository.GetDoctorById(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return Ok(doctor);
        }

        // POST api/Doctor (insert new doctor)
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest();
            }

            await _doctorRepository.InsertDoctor(doctor);
            return CreatedAtAction(nameof(Get), new { id = doctor.DoctorID }, doctor);
        }

        // PUT api/Doctor/{id} (update doctor)
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.DoctorID)
            {
                return BadRequest();
            }

            var existingDoctor = await _doctorRepository.GetDoctorById(id);
            if (existingDoctor == null)
            {
                return NotFound();
            }

            await _doctorRepository.UpdateDoctor(doctor);
            return NoContent();
        }

        // DELETE api/Doctor/{id} (Delete doctor)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var doctor = await _doctorRepository.GetDoctorById(id);
            if (doctor == null)
            {
                return NotFound();
            }

            await _doctorRepository.DeleteDoctor(id);
            return NoContent();
        }
    }
}
