using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AppointmentService.Repository
{
    public class AppointmentRepository : AppointmentInterface
    {
        private readonly AppointmentServiceContext _dbContext;
        private readonly TaskManager _taskManager; // To handle timeouts
        private readonly SemaphoreSlim _semaphore; // Semaphore for limiting concurrent tasks
        private const int MaxConcurrentTasks = 5; // Maximum number of concurrent tasks is 5

        public AppointmentRepository(AppointmentServiceContext dbContext, TaskManager taskManager)
        {
            _dbContext = dbContext;
            _taskManager = taskManager;
            _semaphore = new SemaphoreSlim(MaxConcurrentTasks);
        }

        public async Task<Appointment> GetAppointmentById(int appointmentId)
        {
            await _semaphore.WaitAsync(); // Wait for an available slot
            TimeSpan timeout = TimeSpan.FromSeconds(10); // Time limit for the task is 10 sec

            try
            {
                return await _taskManager.RunWithTimeout<Appointment>(async token =>
                {
                    return await _dbContext.Appointment
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.AppointmentID == appointmentId, token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task InsertAppointment(Appointment appointment)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    await _dbContext.Appointment.AddAsync(appointment, token);
                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateAppointment(Appointment appointment)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var existingAppointment = await _dbContext.Appointment.FindAsync(new object[] { appointment.AppointmentID }, token);
                    if (existingAppointment != null)
                    {
                        _dbContext.Entry(existingAppointment).CurrentValues.SetValues(appointment);
                    }
                    else
                    {
                        _dbContext.Appointment.Attach(appointment);
                        _dbContext.Entry(appointment).State = EntityState.Modified;
                    }

                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteAppointment(int appointmentId)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var appointment = await _dbContext.Appointment.FindAsync(new object[] { appointmentId }, token);
                    if (appointment != null)
                    {
                        _dbContext.Appointment.Remove(appointment);
                        await SaveAsync();
                    }
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
