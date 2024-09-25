using MaternityMicroservices.Models;
using MaternityMicroservices.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace MaternityMicroservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        // GET api/patient/{id}
        [HttpGet("{id}", Name = "GetPatient")]
        public IActionResult Get(int id)
        {
            var patient = _patientRepository.GetPatientById(id);
            if (patient == null)
            {
                return NotFound(); // Return 404 if the patient is not found
            }
            return Ok(patient); // Return 200 with the patient data
        }

        // POST api/patient
        [HttpPost]
        public IActionResult Post([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest(); // Return 400 if the request body is invalid
            }

            using (var scope = new TransactionScope())
            {
                _patientRepository.InsertPatient(patient);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = patient.PatientID }, patient); // Use patient.PatientId to create the resource location
            }
        }

        // PUT api/patient
        [HttpPut]
        public IActionResult Put([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest(); // Return 400 if the request body is invalid
            }

            var existingPatient = _patientRepository.GetPatientById(patient.PatientID);
            if (existingPatient == null)
            {
                return NotFound(); // Return 404 if the patient does not exist
            }

            using (var scope = new TransactionScope())
            {
                _patientRepository.UpdatePatient(patient); // Update the patient directly
                scope.Complete();
                return Ok(); // Return 200 OK if the update was successful
            }
        }
    }
}
