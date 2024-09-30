using AppointmentService.Models;
using AppointmentService.Repository;
using AppointmentService.Repository;
//using MaternityMicroservices.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientInterface _patientRepository;

        public PatientController(PatientInterface patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // GET api/Patient/{id} - get patient by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> Get(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        // POST api/Patient -- insert new patient
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest();
            }

            await _patientRepository.InsertPatientAsync(patient);
            return CreatedAtAction(nameof(Get), new { id = patient.PatientID }, patient);
        }

        // PUT api/Patient/{id} - update patient
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Patient patient)
        {
            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            var existingPatient = await _patientRepository.GetPatientByIdAsync(id);
            if (existingPatient == null)
            {
                return NotFound();
            }

            await _patientRepository.UpdatePatientAsync(patient);
            return NoContent();
        }

        // DELETE api/Patient/{id} - delete patient
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            await _patientRepository.DeletePatientAsync(id);
            return NoContent();
        }
    }
}
