using AppointmentService.Models;
using AppointmentService.Repository;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Repository
{
    public class DoctorRepository : DoctorInterface
    {
        private readonly AppointmentServiceContext _dbContext;
        private readonly TaskManager _taskManager; // To handle timeouts
        private readonly SemaphoreSlim _semaphore; // Semaphore for limiting concurrent tasks
        private const int MaxConcurrentTasks = 5; // Maximum number of concurrent tasks is 5

        public DoctorRepository(AppointmentServiceContext dbContext, TaskManager taskManager)
        {
            _dbContext = dbContext;
            _taskManager = taskManager;
            _semaphore = new SemaphoreSlim(MaxConcurrentTasks);
        }

        public async Task<Doctor> GetDoctorById(int doctorID)
        {
            await _semaphore.WaitAsync(); // Wait for an available slot
            TimeSpan timeout = TimeSpan.FromSeconds(10); // Time limit for the task is 10 sec

            try
            {
                return await _taskManager.RunWithTimeout<Doctor>(async token =>
                {
                    return await _dbContext.Doctor
                        .AsNoTracking()
                        .FirstOrDefaultAsync(d => d.DoctorID == doctorID, token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task InsertDoctor(Doctor doctor)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    await _dbContext.Doctor.AddAsync(doctor, token);
                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateDoctor(Doctor doctor)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var existingDoctor = await _dbContext.Doctor.FindAsync(new object[] { doctor.DoctorID }, token);
                    if (existingDoctor != null)
                    {
                        _dbContext.Entry(existingDoctor).CurrentValues.SetValues(doctor);
                    }
                    else
                    {
                        _dbContext.Doctor.Attach(doctor);
                        _dbContext.Entry(doctor).State = EntityState.Modified;
                    }

                    await SaveAsync();
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteDoctor(int doctorID)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var doctor = await _dbContext.Doctor.FindAsync(new object[] { doctorID }, token);
                    if (doctor != null)
                    {
                        _dbContext.Doctor.Remove(doctor);
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
