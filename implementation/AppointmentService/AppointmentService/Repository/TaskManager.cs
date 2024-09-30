// A class which handles timeouts
//The primary puprose of the class TaskManager is to execute task within the defined time limit
namespace AppointmentService.Repository

{
    public class TaskManager
    {
        // Methos for asynchronous operations, used for the tasks which retrive data from database
        public async Task<T> RunWithTimeout<T>(Func<CancellationToken, Task<T>> action, TimeSpan timeout)
        {
            using (var cancellationTokenSource = new CancellationTokenSource(timeout))
            {
                try
                {
                    return await action(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }

        // Method for the tasks which do not return any value (insert, update, delete operations)
        public async Task RunWithTimeout(Func<CancellationToken, Task> action, TimeSpan timeout)
        {
            using (var cancellationTokenSource = new CancellationTokenSource(timeout))
            {
                try
                {
                    await action(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }
}
