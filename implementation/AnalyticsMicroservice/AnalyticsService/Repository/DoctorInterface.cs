// Interface for Doctor entity
using AnalyticsService.Models;

namespace AnalyticsService.Repository
{
    public interface DoctorInterface
    {
        Task<Doctor> GetDoctorById(int DoctorID); // Get doctor by ID
        Task InsertDoctor(Doctor doctor); // Insert new doctor
        Task UpdateDoctor(Doctor doctor); // Update existing doctor
        Task DeleteDoctor(int DoctorID); // Delete doctor
        Task SaveAsync();
    }
}

