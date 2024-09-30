using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AppointmentService.Repository
{
    public class AppointmentHistoryRepository : AppointmentHistoryInterface
    {
        private readonly AppointmentServiceContext _dbContext;
        private readonly TaskManager _taskManager; // To handle timeouts
        private readonly SemaphoreSlim _semaphore; // Semaphore for limiting concurrent tasks
        private const int MaxConcurrentTasks = 5; // Maximum number of concurrent tasks is 5

        public AppointmentHistoryRepository(AppointmentServiceContext dbContext, TaskManager taskManager)
        {
            _dbContext = dbContext;
            _taskManager = taskManager;
            _semaphore = new SemaphoreSlim(MaxConcurrentTasks);
        }

        public async Task<AppointmentHistory> GetAppointmentHistoryById(int appointmentHistoryID)
        {
            await _semaphore.WaitAsync(); // Wait for an available slot
            TimeSpan timeout = TimeSpan.FromSeconds(10); // Time limit for the task is 10 seconds

            try
            {
                return await _taskManager.RunWithTimeout<AppointmentHistory>(async token =>
                {
                    return await _dbContext.AppointmentHistory
                        .AsNoTracking()
                        .FirstOrDefaultAsync(a => a.AppointmentHistoryID == appointmentHistoryID, token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task<IEnumerable<AppointmentHistory>> GetAppointmentsByStatusId(int statusID)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10); 

            try
            {
                return await _taskManager.RunWithTimeout<IEnumerable<AppointmentHistory>>(async token =>
                {
                    return await _dbContext.AppointmentHistory
                        .AsNoTracking()
                        .Where(a => a.StatusID == statusID)
                        .ToListAsync(token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task InsertAppointmentHistory(AppointmentHistory appointmentHistory)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    await _dbContext.AppointmentHistory.AddAsync(appointmentHistory, token);
                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateAppointmentHistory(AppointmentHistory appointmentHistory)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var existingAppointmentHistory = await _dbContext.AppointmentHistory
                        .FindAsync(new object[] { appointmentHistory.AppointmentHistoryID }, token);

                    if (existingAppointmentHistory != null)
                    {
                        _dbContext.Entry(existingAppointmentHistory).CurrentValues.SetValues(appointmentHistory);
                    }
                    else
                    {
                        _dbContext.AppointmentHistory.Attach(appointmentHistory);
                        _dbContext.Entry(appointmentHistory).State = EntityState.Modified;
                    }

                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteAppointmentHistory(int appointmentHistoryID)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var appointmentHistory = await _dbContext.AppointmentHistory
                        .FindAsync(new object[] { appointmentHistoryID }, token);

                    if (appointmentHistory != null)
                    {
                        _dbContext.AppointmentHistory.Remove(appointmentHistory);
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
