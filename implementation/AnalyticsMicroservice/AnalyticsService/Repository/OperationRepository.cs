using AnalyticsService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyticsService.Repository
{
    public class OperationRepository : OperationInterface
    {
        private readonly AnalyticsServiceContext _dbContext;
        private readonly TaskManager _taskManager; // To handle timeouts
        private readonly SemaphoreSlim _semaphore; // Semaphore for limiting concurrent tasks
        private const int MaxConcurrentTasks = 5; // Concurrent task limit is 5

        public OperationRepository(AnalyticsServiceContext dbContext, TaskManager taskManager)
        {
            _dbContext = dbContext;
            _taskManager = taskManager;
            _semaphore = new SemaphoreSlim(MaxConcurrentTasks); // Initialize the semaphore
        }

        public async Task<Operation> GetOperationByIdAsync(int operationID)
        {
            await _semaphore.WaitAsync(); // Wait for an available slot
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                return await _taskManager.RunWithTimeout<Operation>(async token =>
                {
                    return await _dbContext.Operation.AsNoTracking().FirstOrDefaultAsync(o => o.OperationID == operationID, token);
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); // Release the slot
            }
        }

        public async Task InsertOperationAsync(Operation operation)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    await _dbContext.Operation.AddAsync(operation, token);
                    await SaveAsync(); 
                }, timeout);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateOperationAsync(Operation operation)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var existingOperation = await _dbContext.Operation.FindAsync(new object[] { operation.OperationID }, token);

                    if (existingOperation != null)
                    {
                        _dbContext.Entry(existingOperation).CurrentValues.SetValues(operation);
                    }
                    else
                    {
                        _dbContext.Operation.Attach(operation);
                        _dbContext.Entry(operation).State = EntityState.Modified;
                    }

                    await SaveAsync(); 
                }, timeout);
            }
            finally
            {
                _semaphore.Release(); 
            }
        }

        public async Task DeleteOperationAsync(int operationID)
        {
            await _semaphore.WaitAsync(); 
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                await _taskManager.RunWithTimeout(async token =>
                {
                    var operation = await _dbContext.Operation.FindAsync(new object[] { operationID }, token);
                    if (operation != null)
                    {
                        _dbContext.Operation.Remove(operation);
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
