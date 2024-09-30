using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppointmentService.Repository
{
    public class AppointmentStatusRepository : AppointmentStatusInterface
    {
        private readonly AppointmentServiceContext _dbContext;
        private readonly TaskManager _taskManager; // To handle timeouts
        private readonly SemaphoreSlim _semaphore; // Semaphore for limiting concurrent tasks
        private const int MaxConcurrentTasks = 5; // Maximum number of concurrent tasks is 5

        public AppointmentStatusRepository(AppointmentServiceContext dbContext, TaskManager taskManager)
        {
            _dbContext = dbContext;
            _taskManager = taskManager;
            _semaphore = new SemaphoreSlim(MaxConcurrentTasks);
        }

        public async Task<AppointmentStatus> GetAppointmentStatusById(int appointmentStatusID)
        {
            await _semaphore.WaitAsync(); // Wait for an available slot
            TimeSpan timeout = TimeSpan.FromSeconds(10); // Time limit for the task is 10 sec

            try
            {
                return await _taskManager.RunWithTimeout<AppointmentStatus>(async token =>
                {
                    return await _dbContext.AppointmentStatus
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.AppointmentStatusID == appointmentStatusID, token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task InsertAppointmentStatus(AppointmentStatus appointmentStatus)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    await _dbContext.AppointmentStatus.AddAsync(appointmentStatus, token);
                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateAppointmentStatus(AppointmentStatus appointmentStatus)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var existingStatus = await _dbContext.AppointmentStatus.FindAsync(new object[] { appointmentStatus.AppointmentStatusID }, token);
                    if (existingStatus != null)
                    {
                        _dbContext.Entry(existingStatus).CurrentValues.SetValues(appointmentStatus);
                    }
                    else
                    {
                        _dbContext.AppointmentStatus.Attach(appointmentStatus);
                        _dbContext.Entry(appointmentStatus).State = EntityState.Modified;
                    }

                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteAppointmentStatus(int appointmentStatusID)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var status = await _dbContext.AppointmentStatus.FindAsync(new object[] { appointmentStatusID }, token);
                    if (status != null)
                    {
                        _dbContext.AppointmentStatus.Remove(status);
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
