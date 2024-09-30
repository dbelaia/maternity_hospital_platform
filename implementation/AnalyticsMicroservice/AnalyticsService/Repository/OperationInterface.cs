// Interface to handle operations
// Operation table in database which is responsible for types of medical operations and their prices
using AnalyticsService.Models;

namespace AnalyticsService.Repository
{
    public interface OperationInterface
    {
        Task<Operation> GetOperationByIdAsync(int operationID); // Get operation by ID
        Task InsertOperationAsync(Operation operation);           // Insert new operation
        Task UpdateOperationAsync(Operation operation);           // Update existing operation
        Task DeleteOperationAsync(int operationID);              // Delete operation
        Task SaveAsync();                                        
    }
}
