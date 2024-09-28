using MaternityMicroservices.Models;
using System.Collections.Generic;

namespace MaternityMicroservices.Repository
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetPatients();          // Get all patients
        Patient GetPatientById(int patientId);       // Get a patient by ID
        void InsertPatient(Patient patient);         // Insert a new patient
        void Save();                                 // Save changes
    }
}
