// Interface to handle Patient entity
using AnalyticsService.Models;

namespace AnalyticsService.Repository
{
    public interface PatientInterface
    {
        Task<Patient> GetPatientByIdAsync(int patientId);       // Get a patient by ID 
        Task InsertPatientAsync(Patient patient);                // Insert a new patient 
        Task UpdatePatientAsync(Patient patient);                // Update an existing patient 
        Task DeletePatientAsync(int patientId);                  // Delete a patient by ID 
        Task SaveAsync();                                     
    }
}
