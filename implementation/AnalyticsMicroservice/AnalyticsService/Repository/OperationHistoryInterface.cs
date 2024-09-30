// Interface to handle OperationHistory
using AnalyticsService.Models;

namespace AnalyticsService.Repository
{
    public interface OperationHistoryInterface
    {
        Task<OperationHistory> GetOperationHistoryById(int operationHistoryID); // Get a historical record by ID
        
        // Get all historical records within some time period
        // In future can be used for performance metrics of a doctor in reports
        Task<IEnumerable<OperationHistory>> GetOperationsByDateRange(DateTime startDateTime, DateTime endDateTime); 
        Task InsertOperationHistory(OperationHistory operationHistory); // Insert new historical record
        Task UpdateOperationHistory(OperationHistory operationHistory); // Update historical record
        Task DeleteOperationHistory(int operationHistoryID); // Delete historical record
        Task SaveAsync();
    }
}
