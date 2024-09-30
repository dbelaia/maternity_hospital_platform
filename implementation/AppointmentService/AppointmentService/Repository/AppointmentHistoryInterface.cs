using AppointmentService.Models;

namespace AppointmentService.Repository
{
    public interface AppointmentHistoryInterface
    {
        Task<AppointmentHistory> GetAppointmentHistoryById(int AppointmentHistoryID); // Get appointment historical record
        Task<IEnumerable<AppointmentHistory>> GetAppointmentsByStatusId(int StatusID); // Get appointment historical record by StatusID
        Task InsertAppointmentHistory(AppointmentHistory appointmentHistory); // Insert new appointment historical record
        Task UpdateAppointmentHistory(AppointmentHistory appointmentHistory); // Update existing historical record
        Task DeleteAppointmentHistory(int AppointmentHistoryID); // Delete appointment historical reord
        Task SaveAsync();
    }
}
