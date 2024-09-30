using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Repository
{
    public class PatientRepository : PatientInterface
    {
        private readonly AnalyticsServiceContext _dbContext;
        private readonly TaskManager _taskManager; // To handle timeouts
        private readonly SemaphoreSlim _semaphore; // Semaphore for limiting concurrent tasks
        private const int MaxConcurrentTasks = 5; // Concurrent tasks limit is 5

        public PatientRepository(AnalyticsServiceContext dbContext, TaskManager taskManager)
        {
            _dbContext = dbContext;
            _taskManager = taskManager;
            _semaphore = new SemaphoreSlim(MaxConcurrentTasks); // Initialize the semaphore
        }

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            await _semaphore.WaitAsync(); // Wait for an available slot
            TimeSpan timeout = TimeSpan.FromSeconds(10); // Set a timeout for the operation

            try
            {
                return await _taskManager.RunWithTimeout<Patient>(async token =>
                {
                    return await _dbContext.Patient.AsNoTracking()
                        .FirstOrDefaultAsync(p => p.PatientID == patientId, token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task InsertPatientAsync(Patient patient)
        {
            await _semaphore.WaitAsync();
            TimeSpan timeout = TimeSpan.FromSeconds(10); 

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    await _dbContext.Patient.AddAsync(patient, token);
                    await SaveAsync(); 
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var existingPatient = await _dbContext.Patient.FindAsync(new object[] { patient.PatientID }, token);
                    if (existingPatient != null)
                    {
                        _dbContext.Entry(existingPatient).CurrentValues.SetValues(patient);
                    }
                    else
                    {
                        _dbContext.Patient.Attach(patient);
                        _dbContext.Entry(patient).State = EntityState.Modified;
                    }
                    await SaveAsync(); 
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }

        public async Task DeletePatientAsync(int patientId)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10); 

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var patient = await _dbContext.Patient.FindAsync(new object[] { patientId }, token);
                    if (patient != null)
                    {
                        _dbContext.Patient.Remove(patient);
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
