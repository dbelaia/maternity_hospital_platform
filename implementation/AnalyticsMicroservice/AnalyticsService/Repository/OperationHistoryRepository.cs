using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Repository
{
    public class OperationHistoryRepository : OperationHistoryInterface
    {
        private readonly AnalyticsServiceContext _dbContext;
        private readonly SemaphoreSlim _semaphore; // Semaphore for limiting concurrent tasks
        private const int MaxConcurrentTasks = 5; // Limit for concurrent tasks is 5
        private readonly TaskManager _taskManager; // To handle timeouts

        public OperationHistoryRepository(AnalyticsServiceContext dbContext, TaskManager taskManager)
        {
            _dbContext = dbContext;
            _taskManager = taskManager;
            _semaphore = new SemaphoreSlim(MaxConcurrentTasks); // Initialize the semaphore
        }

        public async Task<OperationHistory> GetOperationHistoryById(int operationHistoryID)
        {
            await _semaphore.WaitAsync(); // Wait for an available slot
            TimeSpan timeout = TimeSpan.FromSeconds(10); // Time limit is 10 sec

            try
            {
                return await _taskManager.RunWithTimeout<OperationHistory>(async token =>
                {
                    return await _dbContext.OperationHistory
                        .AsNoTracking()
                        .FirstOrDefaultAsync(oh => oh.OperationHistoryID == operationHistoryID, token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task<IEnumerable<OperationHistory>> GetOperationsByDateRange(DateTime startDateTime, DateTime endDateTime)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10); 

            try
            {
                return await _taskManager.RunWithTimeout<IEnumerable<OperationHistory>>(async token =>
                {
                    return await _dbContext.OperationHistory
                        .AsNoTracking()
                        .Where(oh => oh.StartDateTime >= startDateTime && oh.EndDateTime <= endDateTime)
                        .ToListAsync(token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }

        public async Task InsertOperationHistory(OperationHistory operationHistory)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10); 

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    await _dbContext.OperationHistory.AddAsync(operationHistory, token);
                    await SaveAsync(); 
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }

        public async Task UpdateOperationHistory(OperationHistory operationHistory)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10); 

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var existingHistory = await _dbContext.OperationHistory.FindAsync(new object[] { operationHistory.OperationHistoryID }, token);
                    if (existingHistory != null)
                    {
                        _dbContext.Entry(existingHistory).CurrentValues.SetValues(operationHistory);
                    }
                    else
                    {
                        _dbContext.OperationHistory.Attach(operationHistory);
                        _dbContext.Entry(operationHistory).State = EntityState.Modified;
                    }
                    await SaveAsync(); 
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }

        public async Task DeleteOperationHistory(int operationHistoryID)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10); 

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var operationHistory = await _dbContext.OperationHistory.FindAsync(new object[] { operationHistoryID }, token);
                    if (operationHistory != null)
                    {
                        _dbContext.OperationHistory.Remove(operationHistory);
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
