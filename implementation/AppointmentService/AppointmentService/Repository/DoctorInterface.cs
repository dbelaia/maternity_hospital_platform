// Interface for Doctor entity
using System.Threading.Tasks; // Ensure you include this for Task
using AppointmentService.Models; // Use the correct namespace for Doctor
namespace AppointmentService.Repository
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