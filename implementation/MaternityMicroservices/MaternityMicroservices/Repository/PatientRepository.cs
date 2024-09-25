using MaternityMicroservices.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MaternityMicroservices.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MaternityAppointmentServiceContext _dbContext;

        public PatientRepository(MaternityAppointmentServiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Retrieve all patients from the database
        public IEnumerable<Patient> GetPatients()
        {
            return _dbContext.Patients.ToList();
        }

        // Retrieve a patient by their ID
        public Patient GetPatientById(int patientId)
        {
            return _dbContext.Patients.Find(patientId);
        }

        // Insert a new patient into the database
        public void InsertPatient(Patient patient)
        {
            _dbContext.Patients.Add(patient);
            Save();
        }

        // Update an existing patient record in the database
        public void UpdatePatient(Patient patient)
        {
            _dbContext.Entry(patient).State = EntityState.Modified;
            Save();
        }

        // Save the changes to the database
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
