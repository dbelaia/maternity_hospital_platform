using AppointmentService.Models; // Use the correct namespace for Doctor
namespace AppointmentService.Repository

{
    public interface AppointmentStatusInterface
    {
        Task<AppointmentStatus> GetAppointmentStatusById(int AppointmentStatusID); // Get appointment status by id
        Task InsertAppointmentStatus(AppointmentStatus appointmentStatus); // Insert new appointment status
        Task UpdateAppointmentStatus(AppointmentStatus appointmentStatus); // Update existing appointment status
        Task DeleteAppointmentStatus(int AppointmentStatusID); // Delete appointment status
        Task SaveAsync();
    }
}
