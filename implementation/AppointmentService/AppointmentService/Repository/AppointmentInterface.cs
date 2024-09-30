using AppointmentService.Models;

namespace AppointmentService.Repository
{
    public interface AppointmentInterface
    {
        Task<Appointment> GetAppointmentById(int AppAppointmentID); // Get appointment by ID
        Task InsertAppointment(Appointment appointmentId); // Insert new appointment
        Task UpdateAppointment(Appointment appointmentId); // Update existing appointment
        Task DeleteAppointment(int appointmentId); // Delete appointment
        Task SaveAsync();
    }
}
